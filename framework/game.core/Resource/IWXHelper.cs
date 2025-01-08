namespace Game.Core;

public class LoadWXFileCallback
{
    public Action<byte[]> LoadSuccess;
    public Action LoadFail;

    public LoadWXFileCallback(Action<byte[]> loadSuccess, Action loadFail)
    {
        LoadSuccess = loadSuccess;
        LoadFail = loadFail;
    }
}

public class WriteWXFileCallback
{
    public Action WriteSuccess;
    public Action WriteFail;

    public WriteWXFileCallback(Action writeSuccess, Action writeFail)
    {
        WriteSuccess = writeSuccess;
        WriteFail = writeFail;
    }
}
public interface IWXHelper
{
    //void MoveFile(string resource, string destination);
    public string GetWXUserDataPrefix();
    public void ReadFile(string path, LoadWXFileCallback loadCallback);
    public void WriteFile(string path, byte[] data, WriteWXFileCallback writeCallback);
}