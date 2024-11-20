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
        m_Model.Lattice.Initialize(m_Model.Board
            , this
            , Entity.EntityAssetName);
        
    }
}