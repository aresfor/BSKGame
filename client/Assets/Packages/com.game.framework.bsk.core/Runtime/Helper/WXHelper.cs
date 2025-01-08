
using System;
using System.IO;
using GameFramework;
using WeChatWASM;

namespace Game.Core
{
    public class WXHelper:IWXHelper
    {
        public string GetWXUserDataPrefix() => WX.env.USER_DATA_PATH;
        public string GetWXPath(string path)=>Path.Combine(GetWXUserDataPrefix(), path);
        public const string Encoding= "base64";
        public const string AccessFileSuccess = "access:ok";
        public void ReadFile(string path, LoadWXFileCallback loadCallback)
        {
            var fs = GetFS();

            void OnReadSuccess(WXReadFileResponse response)
            {
                GameFrameworkLog.Info($"ReadWXFileSuccess, Path: {path}");
                loadCallback?.LoadSuccess?.Invoke(response.binData);
            }
            void OnReadFail(FileError error)
            {
                GameFrameworkLog.Error($"ReadWXFileFail ErrorCode: {error.errCode}, Error: {error.errMsg}, Path: {path}");
                loadCallback?.LoadFail?.Invoke();
            }

            // fs.ReadFile(new ReadFileParam()
            // {
            //     success = OnReadSuccess,
            //     fail = OnReadFail,
            //     encoding = Encoding,
            //     filePath = path
            // });

            bool success = false;
            byte[] bytes = null;
            string readFileString = null;
            string error = null;
            try
            {
                if (AccessFileSuccess == fs.AccessSync(path))
                {
                    readFileString = fs.ReadFileSync(path, Encoding);
                    if (false == string.IsNullOrEmpty(readFileString))
                    {
                        bytes = Convert.FromBase64String(readFileString);
                        success = true;
                        //GameFrameworkLog.Error($"readfile success, binDataLength: {bytes.Length}, firstChar: {bytes[0]}, stringLength: {readFileString.Length}");

                    }
                }
            }
            catch (Exception e)
            {
                //GameFrameworkLog.Error($"ReadWXFileSync Fail Error: {e.Message}");
                error = e.Message;
                throw;
            }

            finally
            {
                if (success)
                {
                    OnReadSuccess(new WXReadFileResponse()
                    {
                        binData = bytes,
                        arrayBufferLength = bytes.Length,
                        stringData = readFileString
                    });
                }
                else
                {
                    OnReadFail(new FileError()
                    {
                        errMsg = error
                    });
                }
            }
            

        }

        public void WriteFile(string path, byte[] data, WriteWXFileCallback writeCallback)
        {
            var fs = GetFS();

            void OnWriteSuccess(WXTextResponse response)
            {
                GameFrameworkLog.Info($"WriteWXFileSuccess, Path: {path}");
                writeCallback?.WriteSuccess?.Invoke();

            }
            void OnWriteFail(WXTextResponse response)
            {
                GameFrameworkLog.Error($"WriteWXFileFail, ErrorCode: {response.errCode}, Error: {response.errMsg}, Path: {path}");
                writeCallback?.WriteFail?.Invoke();

            }
            // fs.WriteFile(new WriteFileParam()
            // {
            //     success = OnWriteSuccess,
            //     fail = OnWriteFail,
            //     encoding = Encoding,
            //     data = data,
            //     filePath = path
            // });

            try
            {
                var res = fs.WriteFileSync(path, data, Encoding);
                
                //GameFrameworkLog.Error($"writefilesync res: {res}, binDataLength: {data.Length}, firstChar: {data[0]}");
                OnWriteSuccess(new WXTextResponse()
                {
                    errMsg = res
                });
            }
            catch (Exception e)
            {
                //GameFrameworkLog.Error($"WriteWXFileSync Fail Error: {e.Message}");
                OnWriteFail(new WXTextResponse()
                {
                    errMsg = e.Message
                });
                throw;
            }
        }

        public WXFileSystemManager GetFS() => WX.GetFileSystemManager();
        
        // public void MoveFile(string resource, string destination)
        // {
        //     if (false == File.Exists(resource))
        //     {
        //         GameFrameworkLog.Error($"file resource not exist: {resource}");
        //         return;
        //     }
        //     byte[] inData = File.ReadAllBytes(resource);
        //     var fs = WX.GetFileSystemManager();
        //     if (false == destination.StartsWith('/'))
        //         destination = "/" + destination;
        //     destination = WX.env.USER_DATA_PATH + destination;
        //     fs.WriteFile(new WriteFileParam()
        //     {
        //         fail = response => GameFrameworkLog.Error($"asdsadasfail to write file:{destination} to wx, errorCode: {response.errCode}"),
        //         success = response => GameFrameworkLog.Info($"Write file to wx success: {destination}"),
        //         data = inData,
        //         filePath =  destination
        //     });
        // }

        // void ReadBytes(string path, Action<byte[]> successCall, Action failCall)
        // {
        //     var fs = GetFS();
        //     GameFramework.GameFrameworkLog.Error($"writeToPath: {path}");
        //     fs.ReadFile(new ReadFileParam()
        //     {
        //         success = response => successCall?.Invoke(response.binData),
        //         fail = response => failCall?.Invoke(),
        //         filePath = path
        //     });
        //     
        // }
    }
}