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

  public static class Log
  {
    private static ILogUtility s_LogInstance;

    public static void Initialize(ILogUtility logger) => Log.s_LogInstance = logger;

    public static void Shutdown()
    {
      if (Log.s_LogInstance == null)
        return;
      Log.s_LogInstance = (ILogUtility) null;
    }

    public static void SetCurCommandFrame(uint curFrame)
    {
      Log.s_LogInstance?.SetCurCommandFrame(curFrame);
    }

    public static void SetProperty(string propertyKey, object propertyValue)
    {
      Log.s_LogInstance?.SetProperty(propertyKey, propertyValue);
    }

    public static void Assert(bool expr, string err) => Log.s_LogInstance?.Assert(expr, err);

    [Conditional("LOG_TRACE")]
    public static void Trace(string content) => Log.s_LogInstance?.Output(ELogLevel.Trace, content);

    [Conditional("LOG_TRACE")]
    public static void Trace(string format, params object[] parameters)
    {
      if (parameters == null || parameters.Length == 0)
        return;
      string.Format(format, parameters);
    }

    [Conditional("LOG_DEBUG")]
    public static void Debug(string content) => Log.s_LogInstance?.Output(ELogLevel.Debug, content);

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
      Log.s_LogInstance?.Output(ELogLevel.Publish, content);
    }

    [Conditional("LOG_PUBLISH")]
    public static void Publish(string format, params object[] parameters)
    {
      Log.Publish(parameters == null || parameters.Length == 0 ? format : string.Format(format, parameters));
    }

    [Conditional("LOG_WARNING")]
    public static void Warning(string content)
    {
      Log.s_LogInstance?.Output(ELogLevel.Warning, content);
    }

    [Conditional("LOG_WARNING")]
    public static void Warning(string format, params object[] parameters)
    {
      Log.Warning(parameters == null || parameters.Length == 0 ? format : string.Format(format, parameters));
    }

    [Conditional("LOG_ERROR")]
    public static void Error(string content) => Log.s_LogInstance?.Output(ELogLevel.Error, content);

    [Conditional("LOG_ERROR")]
    public static void Error(string format, params object[] parameters)
    {
      Log.Error(parameters == null || parameters.Length == 0 ? format : string.Format(format, parameters));
    }

    [Conditional("LOG_TRACE")]
    public static void TraceThreaded(string content)
    {
      Log.s_LogInstance?.OutputThreaded(ELogLevel.Trace, content);
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
      Log.s_LogInstance?.OutputThreaded(ELogLevel.Debug, content);
    }

    [Conditional("LOG_DEBUG")]
    public static void DebugThreaded(string format, params object[] parameters)
    {
      string content = parameters == null || parameters.Length == 0 ? format : string.Format(format, parameters);
      Log.s_LogInstance?.OutputThreaded(ELogLevel.Debug, content);
    }

    [Conditional("LOG_PUBLISH")]
    public static void PublishThreaded(string content)
    {
      Log.s_LogInstance?.OutputThreaded(ELogLevel.Publish, content);
    }

    [Conditional("LOG_PUBLISH")]
    public static void PublishThreaded(string format, params object[] parameters)
    {
      Log.Publish(parameters == null || parameters.Length == 0 ? format : string.Format(format, parameters));
    }

    [Conditional("LOG_WARNING")]
    public static void WarningThreaded(string content)
    {
      Log.s_LogInstance?.OutputThreaded(ELogLevel.Warning, content);
    }

    [Conditional("LOG_WARNING")]
    public static void WarningThreaded(string format, params object[] parameters)
    {
      Log.Warning(parameters == null || parameters.Length == 0 ? format : string.Format(format, parameters));
    }

    [Conditional("LOG_ERROR")]
    public static void ErrorThreaded(string content)
    {
      Log.s_LogInstance?.OutputThreaded(ELogLevel.Error, content);
    }

    [Conditional("LOG_ERROR")]
    public static void ErrorThreaded(string format, params object[] parameters)
    {
      Log.Error(parameters == null || parameters.Length == 0 ? format : string.Format(format, parameters));
    }

    public static void Exception(string exception, string stackTrace)
    {
      Log.s_LogInstance?.Exception(exception, stackTrace);
    }
  }
