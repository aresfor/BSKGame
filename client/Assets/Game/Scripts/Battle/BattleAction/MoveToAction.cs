using Game.Core;
using Game.Gameplay;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Client
{
    public class MoveToAction:BattleAction
    {
        public override EBattleActionType[] ActionType { get; }
            = new[] { EBattleActionType.ExecuteImmediately };
        
        public override bool ReplaceSame { get; }
        public int SelectEntityId;
        public Vector3 Destination;
        public override void Execute(BattleMainModel model)
        {
            var entity = GameEntry.Entity.GetEntity(SelectEntityId);
            if (entity == null)
            {
                Log.Error("Entity null, check");
                return;
            }

            if (entity.Logic is INavigationAgent agent)
            {
                agent.MoveToDestination(Destination.ToFloat3(), GraphUtils.GetGraph<IGraph>());
            }
        }

        public override IBattleAction Copy()
        {
            var moveToAction = ReferencePool.Acquire<MoveToAction>();
            moveToAction.SelectEntityId = SelectEntityId;
            moveToAction.Destination = Destination;
            return moveToAction;
        }
    }
}