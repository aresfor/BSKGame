using UnityEngine;

namespace Game.Core
{
    public class UnityTimeUtility:ITimeUtility
    {
        public float RealTimeSinceStartup()
        {
            return Time.realtimeSinceStartup;
        }

        public float DeltaTime()
        {
            return Time.deltaTime;
        }

        public float CurrentTime()
        {
            return Time.time;
        }

        public int UpdateFrameExecuteCount()
        {
            return Time.frameCount;
        }
    }
}