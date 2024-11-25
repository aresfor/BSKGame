namespace Game.Core;

using System.Diagnostics;

#nullable disable
public enum ELogLevel
{
  Trace,
  Debug,
  Publish,
  Warning,
  Error,
  Exception,
}

/// <summary>
/// 暂时改名Logs,防止和GF冲突
/// </summary>
  public static class Logs
  {
    private static ILogUtility s_LogInstance;

    public static void Initialize(ILogUtility logger) => Logs.s_LogInstance = logger;

    public static void Shutdown()
    {
      if (Logs.s_LogInstance == null)
        return;
      Logs.s_LogInstance = (ILogUtility) null;
    }

    public static void SetCurCommandFrame(uint curFrame)
    {
      Logs.s_LogInstance?.SetCurCommandFrame(curFrame);
    }

    public static void SetProperty(string propertyKey, object propertyValue)
    {
      Logs.s_LogInstance?.SetProperty(propertyKey, propertyValue);
    }

    public static void Assert(bool expr, string err) => Logs.s_LogInstance?.Assert(expr, err);

    [Conditional("LOG_TRACE")]
    public static void Trace(string content) => Logs.s_LogInstance?.Output(ELogLevel.Trace, content);

    [Conditional("LOG_TRACE")]
    public static void Trace(string format, params object[] parameters)
    {
      if (parameters == null || parameters.Length == 0)
        return;
      string.Format(format, parameters);
    }

    [Conditional("LOG_DEBUG")]
    public static void Debug(string content) => Logs.s_LogInstance?.Output(ELogLevel.Debug, content);

    [Conditional("LOG_DEBUG")]
    public static void Debug(string format, params object[] parameters)
    {
      if (parameters == null || parameters.Length == 0)
        return;
      string.Format(format, parameters);
    }

    [Conditional("LOG_PUBLISH")]
    public static void Publish(string content)
    {
      Logs.s_LogInstance?.Output(ELogLevel.Publish, content);
    }

    [Conditional("LOG_PUBLISH")]
    public static void Publish(string format, params object[] parameters)
    {
      Logs.Publish(parameters == null || parameters.Length == 0 ? format : string.Format(format, parameters));
    }

    [Conditional("LOG_WARNING")]
    public static void Warning(string content)
    {
      Logs.s_LogInstance?.Output(ELogLevel.Warning, content);
    }

    [Conditional("LOG_WARNING")]
    public static void Warning(string format, params object[] parameters)
    {
      Logs.Warning(parameters == null || parameters.Length == 0 ? format : string.Format(format, parameters));
    }

    [Conditional("LOG_ERROR")]
    public static void Error(string content) => Logs.s_LogInstance?.Output(ELogLevel.Error, content);

    [Conditional("LOG_ERROR")]
    public static void Error(string format, params object[] parameters)
    {
      Logs.Error(parameters == null || parameters.Length == 0 ? format : string.Format(format, parameters));
    }

    [Conditional("LOG_TRACE")]
    public static void TraceThreaded(string content)
    {
      Logs.s_LogInstance?.OutputThreaded(ELogLevel.Trace, content);
    }

    [Conditional("LOG_TRACE")]
    public static void TraceThreaded(string format, params object[] parameters)
    {
      if (parameters == null || parameters.Length == 0)
        return;
      string.Format(format, parameters);
    }

    [Conditional("LOG_DEBUG")]
    public static void DebugThreaded(string content)
    {
      Logs.s_LogInstance?.OutputThreaded(ELogLevel.Debug, content);
    }

    [Conditional("LOG_DEBUG")]
    public static void DebugThreaded(string format, params object[] parameters)
    {
      string content = parameters == null || parameters.Length == 0 ? format : string.Format(format, parameters);
      Logs.s_LogInstance?.OutputThreaded(ELogLevel.Debug, content);
    }

    [Conditional("LOG_PUBLISH")]
    public static void PublishThreaded(string content)
    {
      Logs.s_LogInstance?.OutputThreaded(ELogLevel.Publish, content);
    }

    [Conditional("LOG_PUBLISH")]
    public static void PublishThreaded(string format, params object[] parameters)
    {
      Logs.Publish(parameters == null || parameters.Length == 0 ? format : string.Format(format, parameters));
    }

    [Conditional("LOG_WARNING")]
    public static void WarningThreaded(string content)
    {
      Logs.s_LogInstance?.OutputThreaded(ELogLevel.Warning, content);
    }

    [Conditional("LOG_WARNING")]
    public static void WarningThreaded(string format, params object[] parameters)
    {
      Logs.Warning(parameters == null || parameters.Length == 0 ? format : string.Format(format, parameters));
    }

    [Conditional("LOG_ERROR")]
    public static void ErrorThreaded(string content)
    {
      Logs.s_LogInstance?.OutputThreaded(ELogLevel.Error, content);
    }

    [Conditional("LOG_ERROR")]
    public static void ErrorThreaded(string format, params object[] parameters)
    {
      Logs.Error(parameters == null || parameters.Length == 0 ? format : string.Format(format, parameters));
    }

    public static void Exception(string exception, string stackTrace)
    {
      Logs.s_LogInstance?.Exception(exception, stackTrace);
    }
  }
