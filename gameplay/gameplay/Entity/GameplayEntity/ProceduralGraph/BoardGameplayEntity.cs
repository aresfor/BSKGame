using GameFramework.Entity;

namespace Game.Gameplay
{

    /// <summary>
    /// 程序生成棋盘GameplayEntity逻辑类
    /// @TODO:目前在client中用tilemap代替了
    /// </summary>
    public class BoardGameplayEntity : GameplayEntity
    {
        public BoardGameplayEntity(IEntity entity) : base(entity)
        {
        }
    }
}