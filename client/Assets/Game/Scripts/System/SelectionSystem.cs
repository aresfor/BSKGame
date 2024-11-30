using Game.Core;
using Game.Gameplay;
using GameFramework;
using GameFramework.Event;

namespace Game.Client
{
    public class SelectionSystem:ManagedSystem
    {

        protected override void OnInit()
        {
            base.OnInit();
            Enable = true;
            GameEntry.Event.Subscribe(MouseRayCastEventArgs.EventId, OnMouseRayCast);
        }

        protected override void OnDeinit()
        {
            GameEntry.Event.Unsubscribe(MouseRayCastEventArgs.EventId, OnMouseRayCast);
            //m_LastHitRoleLogic = null;
            Enable = false;
            base.OnDeinit();
        }

        public bool Enable { get; set; }

        private void DeSelect(IPointerHandler pointerHandler, FPointerEventData eventData)
        {
            pointerHandler.PointerUp(eventData);
            
            var selectEntityAction = ReferencePool.Acquire<SaveSelectEntityAction>();
            //设置选中目标为非法
            selectEntityAction.SelectedEntityId = 0;
            selectEntityAction.SelectEntityActionType = ESelectEntityActionType.SelectOwner;
            GameUtils.BattleManager.PushAction(selectEntityAction);
        }
        private void OnMouseRayCast(object sender, GameEventArgs eventArgs)
        {
            if (false == Enable)
                return;
            
            MouseRayCastEventArgs args = eventArgs as MouseRayCastEventArgs;
            var impactInfo = args.ImpactInfo;

            // 鼠标按下检测
            if (InputUtils.GetKeyDown(EKeyCode.Mouse0))
            {
                var lastSelectEntity = GameEntry.Entity.GetEntity(GameUtils.SelectedEntityId);
                IPointerHandler lastSelectPointerHandler = null;
                RoleEntityLogic lastSelectRoleLogic = null;
                if (lastSelectEntity != null)
                {
                    lastSelectPointerHandler = lastSelectEntity.Logic as IPointerHandler;
                    lastSelectRoleLogic = lastSelectEntity.Logic as RoleEntityLogic;
                }
                
                // 击中， 并且，但是是地图节点
                if (args.bIsHit && impactInfo.ActorLayer == LayerType.kTypeGraph)
                {
                    if (lastSelectRoleLogic!= null)
                    {
                        lastSelectRoleLogic.MoveToDestination(impactInfo.HitLocation, GraphUtils.GetGraph<IGraph>());
                    }
                }
                // 击中， 并且是role
                else if (args.bIsHit && impactInfo.ActorLayer == LayerType.kTypePawn)
                {
                    var entity = GameEntry.Entity.GetEntity(impactInfo.HitEntityId);
                    var roleLogic = entity.LogicInterface as RoleEntityLogic;
                    if (null == entity)
                    {
                        //Log.Info($"MouseHitEntity is null, entityId: {impactInfo.HitEntityId}");
                    }
                    else
                    {
                        if (lastSelectPointerHandler != null && roleLogic != lastSelectRoleLogic)
                        {
                            DeSelect(lastSelectPointerHandler, new FPointerEventData()
                            {
                                ImpactInfo = impactInfo
                            });
                        }

                        if (roleLogic != null)
                        {
                            roleLogic.PointerDown(new FPointerEventData()
                            {
                                ImpactInfo = impactInfo
                            });
                            var selectEntityAction = ReferencePool.Acquire<SaveSelectEntityAction>();
                            selectEntityAction.SelectedEntityId = roleLogic.Id;
                            selectEntityAction.SelectEntityActionType = ESelectEntityActionType.SelectOwner;
                            GameUtils.BattleManager.PushAction(selectEntityAction);
                            //selectRoleLogic = roleLogic;
                        }
                    }
                }
                else
                {
                    if (lastSelectRoleLogic != null)
                    {
                        DeSelect(lastSelectRoleLogic, new FPointerEventData()
                        {
                            ImpactInfo = impactInfo
                        });
                    }
                }
                
                
                
            }
            
        }
    }
}