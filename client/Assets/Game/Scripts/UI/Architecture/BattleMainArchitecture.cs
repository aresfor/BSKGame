using Game.Core;

namespace Game.Client
{
    public class BattleMainArchitecture: Architecture<BattleMainArchitecture>
    {
        protected override void Init()
        {
            //注册Model
            this.RegisterModel<BattleMainModel>(new BattleMainModel());
            
            //注册System
            this.RegisterSystem<BattleActionSystem>(new BattleActionSystem());
        }
        
        
    }
}