namespace Game.Core
{
    public class LogUtility:ILogUtility
    {
        private string m_FormatStr = "frame {0}:{1}";
        
        public void Initialize()
        {
            Log.Initialize(this);
        }

        public void Shutdown()
        {
            Log.Shutdown();
        }

        public void Reset()
        {
        }


        public void Assert(bool expr, string err)
        {
            UnityEngine.Debug.Assert(expr, err);
        }

        public void Exception(string exception, string stackTrace)
        {
            Output(ELogLevel.Exception, exception + "\n" + stackTrace);
        }

        public void OutputThreaded(ELogLevel level, string content)
        {
            Output(ELogLevel.Publish, content);
        }

        public void SetCurCommandFrame(uint curFrame)
        {
            LogInstance.Instance.SetCurCommandFrame(curFrame);
        }

        public void SetProperty(string propertyKey, object propertyValue)
        {
            //throw new NotImplementedException();
        }

        public void Output(ELogLevel level, string content)
        {
            if (LogInstance.Instance?.m_LogEnable == false)
            {
                return;
            }

            switch (level)
            {
                case ELogLevel.Trace: 
                    LogInstance.Trace(content);
                    break;
                case ELogLevel.Debug: 
                    LogInstance.Debug(content);
                    break;
                case ELogLevel.Publish: 
                    LogInstance.Publish(content);
                    break;
                case ELogLevel.Warning:
                    LogInstance.Warning(content);
                    break;
                case ELogLevel.Error:
                case ELogLevel.Exception:
                    LogInstance.Error(content);
                    break;
            }
        }
    
    }
}