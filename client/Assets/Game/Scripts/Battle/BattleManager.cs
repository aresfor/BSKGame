using Game.Core;

namespace Game.Client
{
    public class BattleManager:IController
    {
        
        public IArchitecture GetArchitecture()
        {
            return BattleMainArchitecture.Interface;
        }

        public void PushAction(IBattleAction battleAction)
        {
            this.GetSystem<BattleActionSystem>().PushAction(battleAction);
        }

        public bool ContainAnyAction()
        {
            return this.GetSystem<BattleActionSystem>().ContainAnyAction();
        }
        
        public void EndBattle()
        {
            BattleMainArchitecture.Interface.Deinit();
        }
        
    }
}