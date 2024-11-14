

namespace Game.Core;

public interface ILogUtility : IUtility
{
    void Assert(bool expr, string err);

    void Output(ELogLevel level, string content);

    void Exception(string exception, string stackTrace);

    void OutputThreaded(ELogLevel level, string content);

    void SetCurCommandFrame(uint curFrame);

    void SetProperty(string propertyKey, object propertyValue);
}