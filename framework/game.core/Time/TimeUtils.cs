namespace Game.Core;

public static class TimeUtils
{
    private static ITimeUtility s_TimeUtilityInstance;

    public static void Initialize(ITimeUtility timeUtility)
    {
        s_TimeUtilityInstance = timeUtility;
    }

    public static float RealTimeSinceStartup()
    {
        return s_TimeUtilityInstance.RealTimeSinceStartup();
    }

    public static float DeltaTime()
    {
        return s_TimeUtilityInstance.DeltaTime();
    }

    public static float CurrentTime()
    {
        return s_TimeUtilityInstance.CurrentTime();
    }

    public static int UpdateFrameExecuteCount()
    {
        return s_TimeUtilityInstance.UpdateFrameExecuteCount();
    }
}