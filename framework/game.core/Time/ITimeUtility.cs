namespace Game.Core;

public interface ITimeUtility : IUtility
{
    float RealTimeSinceStartup();

    float DeltaTime();

    float CurrentTime();

    int UpdateFrameExecuteCount();
}