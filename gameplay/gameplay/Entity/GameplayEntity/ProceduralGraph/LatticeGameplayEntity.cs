using Game.Core;
using GameFramework.Entity;

namespace Game.Gameplay;

public class LatticeGameplayEntity:GameplayEntity
{
    private LatticeEntityModel m_Model;
    public LatticeGameplayEntity(IEntity entity) : base(entity)
    {
    }

    public override void OnInit(EntityData entityData)
    {
        base.OnInit(entityData);
        m_Model = (LatticeEntityModel)entityData;
    }

    public override void OnShow(EntityData entityData)
    {
        base.OnShow(entityData);
        m_Model.LatticeNode.OnShow(this);

        //说明这是最后一个地块加载完毕并且show了
        if (m_Model.Handle.Row == m_Model.BoardGraph.Row - 1 && m_Model.Handle.Column == m_Model.BoardGraph.Column - 1)
        {
            m_Model.BoardGraph.GenerateFinished();
        }
        
    }
    
}