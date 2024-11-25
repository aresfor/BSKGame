using Game.Gameplay;
using GameFramework.Event;

namespace Game.Client
{
    public class MouseRayCastEventArgs: GameEventArgs
    {

        public static readonly int EventId = typeof(MouseRayCastEventArgs).GetHashCode();
        public override int Id { get=>EventId; }
        public bool bIsHit;
        public ImpactInfo ImpactInfo;
        
        public override void Clear()
        {
            ImpactInfo.Recycle(ImpactInfo);
            bIsHit = false;
        }

    }
}