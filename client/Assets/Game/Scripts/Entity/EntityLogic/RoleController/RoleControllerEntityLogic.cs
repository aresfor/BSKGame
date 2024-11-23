using Game.Core;
using Game.Gameplay;
using GameFramework;
using UnityEngine;

namespace Game.Client
{
    public class RoleControllerEntityLogic: GameEntityLogic
    {
        protected override void CreateGameplayEntity()
        {
            base.CreateGameplayEntity();
            GameplayEntity = new RoleControllerGameplayEntity(this.Entity);

        }

        public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            
            //左键
            if (Input.GetMouseButtonDown(0))
            {
                
            }

            //右键
            if (Input.GetMouseButtonDown(1))
            {
                
            }
        }
    }
}