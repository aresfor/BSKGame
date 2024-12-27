using Game.Core;
using GameFramework;
using GameFramework.Entity;

namespace Game.Gameplay
{

    public partial class RoleGameplayEntity : GameplayEntity
    {
        private RoleEntityModel m_RoleModel;

        public RoleGameplayEntity(IEntity entity) : base(entity)
        {
        }

        public override void OnInit(EntityData entityData)
        {
            base.OnInit(entityData);
            m_RoleModel = (RoleEntityModel)entityData;

            InitTeam();
        }

        public override void OnAttachTo(IEntity parentEntity, object userData)
        {
            base.OnAttachTo(parentEntity, userData);
            //m_RoleModel.BelongLatticeId = parentEntity.Id;
            //m_RoleModel.BelongLattice = (parentEntity as IGameEntityLogic).GameplayEntity as LatticeGameplayEntity;
        }

        public override void OnDetachFrom(IEntity parentEntity, object userData)
        {
            base.OnDetachFrom(parentEntity, userData);
            //if(m_RoleModel.BelongLatticeId == parentEntity.Id)
            //    m_RoleModel.BelongLatticeId = 0;

        }
    }
}