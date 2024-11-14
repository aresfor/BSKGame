#if SHIPPING_EXTERNAL
#define LOG_ENCRYPT
#endif

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;
namespace Game.Core
{
    public class LogInstance : MonoSingleton<LogInstance>
    {
        private class LogData
        {
            public string log { get; set; }
            public string trace { get; set; }
            public LogType level { get; set; }
            public uint frame { get; set; }

            public float unscaledTime;
        }
        
        private string m_LogFilePath;
        private string m_LogFileFolderPath;

        private StreamWriter m_LogStreamWriter;
        private ConcurrentQueue<LogData> m_ConcurrentQueue; 
        private ConcurrentQueue<LogData> m_LogDataPool;
        private bool m_WriteFileThreadRunning;
        private Thread m_WriteFileThread;
        private ManualResetEvent m_WriteFileManualResetEvent;
        
        private uint m_LogFrameCount;
        public bool m_LogEnable = true;

        private DateTime m_StartDateTime;
        private float m_StartUnscaledTime;
        private float m_CurrentUnscaledTime;
        private bool m_DiskIsFull;

        private const long FILE_SIZE = 200*1024*1024;
        private const int FILE_COUNT = 20;
        
        private static char[] LogTypeList = new[]
        {
            'E', //Error,
            'A', //Assert,
            'W', //Warning,
            'D', //Log,
            'E', //Exception,
        };
        
        protected override void Init()
        {
#if !UNITY_EDITOR
            if(!m_LogEnable)
            {
                UnityEngine.Debug.unityLogger.filterLogType = LogType.Error;
                return;
            }
            else
            {
                #if LOG_ERRO
                UnityEngine.Debug.unityLogger.filterLogType = LogType.Error;
                #endif
                #if LOG_WARNING
                UnityEngine.Debug.unityLogger.filterLogType = LogType.Warning;
                #endif
                #if LOG_DEBUG
                UnityEngine.Debug.unityLogger.filterLogType = LogType.Log;
                #endif
            }

#else
            SetLogEnable(true);
#endif

#if UNITY_STANDALONE
            m_LogFileFolderPath = string.Format("{0}/_Logs", Application.dataPath.Replace("/Assets", string.Empty));
#else
            m_LogFileFolderPath = string.Format("{0}/_Logs", Application.persistentDataPath);
#endif
            
            
#if LOG_ENCRYPT
            m_LogFilePath = string.Format("{0}/{1}E.log", m_LogFileFolderPath, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"));
#else
            m_LogFilePath = string.Format("{0}/{1}.log", m_LogFileFolderPath, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"));
#endif
            
            if (!Directory.Exists(m_LogFileFolderPath))
            {
                Directory.CreateDirectory(m_LogFileFolderPath);
            }
            else
            {
                Director();
            }

            m_LogStreamWriter = new StreamWriter(m_LogFilePath);
            m_WriteFileManualResetEvent = new ManualResetEvent(false);
            m_ConcurrentQueue = new ConcurrentQueue<LogData>();
            m_LogDataPool = new ConcurrentQueue<LogData>();

            Application.logMessageReceivedThreaded += OnLogMessageReceivedThreaded;
            
            m_WriteFileThreadRunning = true;
            m_WriteFileThread = new Thread(WriteFileThread);
            m_WriteFileThread.Name = "ALogThread";
            m_WriteFileThread.Start();
 
            m_StartDateTime = DateTime.Now;
            m_CurrentUnscaledTime = m_StartUnscaledTime = Time.unscaledTime;
            m_DiskIsFull = false;
            m_CanWriteLogToFile = true;
        }
        
        protected void Director()
        {
            DirectoryInfo d = new DirectoryInfo(m_LogFileFolderPath);
            FileInfo[] files = d.GetFiles();
            SortAsFileName(ref files);
            if (files.Length > FILE_COUNT)
            {
                for (int i = 0; i < files.Length - FILE_COUNT; ++i)
                {
                    File.Delete(files[i].FullName);
                }
            }
        }
        private void SortAsFileName(ref FileInfo[] arrFi)
        {
            Array.Sort(arrFi, delegate (FileInfo x, FileInfo y) { return x.Name.CompareTo(y.Name); });
        }
        
        public void Update()
        {
            m_CurrentUnscaledTime = Time.unscaledTime;
        }

        public void SetLogEnable(bool enable)
        {
            m_LogEnable = enable;
        }

        private void OnLogMessageReceivedThreaded(string logString, string stackTrace, LogType type)
        {
#if LOG_DEBUG
            if (type == LogType.Log)
            {
                EnqueueGeneratedLog(GenerateLogData(logString, stackTrace, type, m_LogFrameCount));
            }
#endif
#if LOG_WARNING
            if (type == LogType.Warning)
            {
                EnqueueGeneratedLog(GenerateLogData(logString, stackTrace, type, m_LogFrameCount));
            }
#endif
#if LOG_ERROR
            if (type == LogType.Error || type == LogType.Assert || type == LogType.Exception)
            {
                EnqueueGeneratedLog(GenerateLogData(logString, stackTrace, type, m_LogFrameCount));
            }
#endif
        }
        
        private bool m_CanWriteLogToFile = true;
        private void GenerateLog(LogType level, string format, params string[] args)
        {
            if (!m_LogEnable || !m_CanWriteLogToFile)
                return;

            string logString = format;
            if (args != null && args.Length > 0)
            {
                logString = string.Format(format, args);
            }
#if UNITY_EDITOR
            LogToEditorConsole(GenerateLogData(logString, string.Empty, level, m_LogFrameCount));
#else
            EnqueueGeneratedLog(GenerateLogData(logString, string.Empty, level, m_LogFrameCount));
#endif
        }
        
        private void EnqueueGeneratedLog(LogData logData)
        {
            if (!m_LogEnable || !m_WriteFileThreadRunning)
            {
                return;
            }            
            m_ConcurrentQueue.Enqueue(logData);
            m_WriteFileManualResetEvent.Set();
        }

       
        private void LogToEditorConsole(LogData logData)
        {
            StringBuilder sb = new StringBuilder();
            //sb.Clear();
            if (logData.frame > 0)
            {
                sb.Append(GetLogTimeStamp(logData.unscaledTime)+"[Frame:"+logData.frame+"]");
            }

            sb.Append(logData.log);
            UnityEngine.Debug.unityLogger.Log(logData.level, sb.ToString());
        }

        public override void OnDestroy()
        {
			base.OnDestroy();
            Application.logMessageReceivedThreaded -= OnLogMessageReceivedThreaded;

            m_WriteFileThreadRunning = false;
            if (m_WriteFileManualResetEvent != null)
            {
                m_WriteFileManualResetEvent.Set();
            }
            Thread.Sleep(1);
            if (m_LogStreamWriter != null)
            {
                m_LogStreamWriter.Close();
                m_LogStreamWriter = null;
            }
            if (m_WriteFileThread != null)
            {
                m_WriteFileThread.Join(10);
                m_WriteFileThread = null;
            }
        }

        private LogData GenerateLogData(string log, string trace, LogType level, uint frame)
        {
            LogData data = null;
            if (!m_LogDataPool.TryDequeue(out data))
            {
                data = new LogData();
            }
            data.frame = frame;
            data.level = level;
            data.trace = trace;
            data.log = log;
            data.unscaledTime = m_CurrentUnscaledTime;
            return data;
        }

        
        private void WriteFileThread()
        {
            int LOG_FLUSH_COUNT = 70;
            StringBuilder builder = new StringBuilder(20480);
            int m_LogCount = 0;
            while (m_WriteFileThreadRunning)
            {
                m_WriteFileManualResetEvent.WaitOne();                
                if (m_LogStreamWriter == null || m_DiskIsFull)
                {
                    break;
                }         
                m_WriteFileManualResetEvent.Reset();
                LogData logData;
                while (m_ConcurrentQueue.Count > 0 && m_ConcurrentQueue.TryDequeue(out logData))
                {                   
                    builder.Clear();
                    int ensureLen = logData.log.Length + logData.trace.Length + 50;
                    if (ensureLen > builder.Capacity)
                    {
                        builder.EnsureCapacity(ensureLen);
                    }
                    if (!string.IsNullOrEmpty(logData.trace) &&
                        (logData.level == LogType.Error || logData.level == LogType.Exception ||
                         logData.level == LogType.Assert))
                    {
                        builder.AppendFormat("[{0}][{1}][{2}]{3}\r\n {4}\r\n ", GetLogTimeStamp(logData.unscaledTime), LogTypeList[(int)logData.level], logData.frame, logData.log, logData.trace);
                    }
                    else
                    {
                        builder.AppendFormat("[{0}][{1}][{2}]{3}\r\n", GetLogTimeStamp(logData.unscaledTime), LogTypeList[(int)logData.level], logData.frame, logData.log);
                    }
#if LOG_ENCRYPT || SHIPPING_EXTERNAL
                    int start = 0;
                    while (start < builder.Length)
                    {
                        int writeLength = Mathf.Min(builder.Length - start, LOG_ENCRYPT_BUFFER);
                        builder.CopyTo(start, CharBuffer, 0, writeLength);
                        int bytesLength = m_LogStreamWriter.Encoding.GetBytes(CharBuffer, 0, writeLength, ByteBuffer, 0);

                        for (int i = 0; i < bytesLength; i++)
                        {
                            ByteBuffer[i] = (byte) (ByteBuffer[i] ^ LOG_ENCRYPT_KEY);
                        }
                        
                        m_LogStreamWriter.BaseStream.Write(ByteBuffer, 0, bytesLength);

                        start += writeLength;
                    }
#else
                    m_LogStreamWriter.Write(builder);
#endif
                    
                    if (m_LogCount >= LOG_FLUSH_COUNT || logData.level == LogType.Error || logData.level == LogType.Exception || logData.level == LogType.Assert)
                    {
                        try
                        {
                            m_LogStreamWriter.Flush();
                        }
                        catch (Exception e)
                        {
                            if (e.Message.ToLowerInvariant().Contains("disk full"))
                            {
                                m_DiskIsFull = true;
                            }
                            else
                            {
                                throw;
                            }
                        }
                        m_LogCount = 0;
                    }
                    m_LogCount++;
                    m_LogDataPool.Enqueue(logData);
                }

                //too much log,TODO split log file
                long len = m_LogStreamWriter.BaseStream.Length;
                if (len > FILE_SIZE)
                {
                    Publish("too much log write to file,log service break");
                    m_WriteFileThreadRunning = false;
                    break;
                }
                Thread.Sleep(10);
            }
        }

#if LOG_ENCRYPT || SHIPPING_EXTERNAL
        private const byte LOG_ENCRYPT_KEY = 0x14;
        private const int LOG_ENCRYPT_BUFFER = 20480;
        byte[] ByteBuffer = new byte[LOG_ENCRYPT_BUFFER*2];
        char[] CharBuffer = new char[LOG_ENCRYPT_BUFFER];
#endif
       
        public void SetCurCommandFrame(uint frame)
        {
            m_LogFrameCount = frame;
        }

        private string GetLogTimeStamp(float unscaledTime)
        {
            float unscaledTimeSinceStart = unscaledTime - m_StartUnscaledTime;
            DateTime now = m_StartDateTime.AddSeconds(unscaledTimeSinceStart);
            return string.Format("{0}:{1}:{2}.{3}", now.Hour, now.Minute, now.Second, now.Millisecond);
        }
        
        public static void Trace(string msg, params string[] paras)
        {
#if UNITY_EDITOR
            Instance?.LogToEditorConsole(Instance.GenerateLogData(msg, string.Empty, LogType.Log, Instance.m_LogFrameCount));
#endif
        }
        
        
        [Conditional("LOG_DEBUG")]
        public static void Debug(string msg, params string[] paras)
        {
            Instance?.GenerateLog(LogType.Log, msg, paras);
        }

        [Conditional("LOG_PUBLISH")]
        public static void Publish(string msg, params string[] paras)
        {
            Instance?.GenerateLog(LogType.Log, msg, paras);
        }
 
        [Conditional("LOG_WARNING")]
        public static void Warning(string msg, params string[] paras)
        {
            Instance?.GenerateLog(LogType.Warning, msg, paras);
        }

        [Conditional("LOG_ERROR")]
        public static void Error(string msg, params string[] paras)
        {
            Instance?.GenerateLog(LogType.Error, msg, paras);
        }
        
        protected override void Dispose()
        {

        }

    }
}