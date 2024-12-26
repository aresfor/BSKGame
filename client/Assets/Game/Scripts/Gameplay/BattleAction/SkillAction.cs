using System.Collections.Generic;
using Game.Core;
using Game.Gameplay;
using Game.Math;
using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Client
{
    public class SkillAction:BattleAction, IUpdateable
    {
        public enum ESKillActionPhase
        {
            ChoseTarget,
            ShouldConfirmRelease,
        }
        public override EBattleActionType[] ActionType { get; }
            = new[] {  EBattleActionType.PushStack };

        public override bool ReplaceSame { get; } = true;

        private int m_SkillId;
        
        public int SkillId
        {
            get => m_SkillId;
            set
            {
                if (value != 0)
                {
                    m_SkillId = value;
                    m_SkillData=GameEntry.DataTable.GetDataTable<DRSkill>().GetDataRow(m_SkillId);
                    if (null == m_SkillData)
                    {
                        Log.Error($"SkillData not found, skill Id: {m_SkillId}");
                    }
                }
            }
        }
        private DRSkill m_SkillData;
        private bool m_Executed = false;
        private List<Vector3Int> m_AvailableCells = new List<Vector3Int>(10);
        private List<Vector3Int> m_SkillEffectCells = new List<Vector3Int>(10);
        private ESKillActionPhase m_Phase;
        private BattleMainModel m_Model;
        
        public override IBattleAction Copy()
        {
            var copy = ReferencePool.Acquire<SkillAction>();
            copy.SkillId = SkillId;
            return copy;
        }
        
        public override void OnPush(BattleMainModel model)
        {
            base.OnPush(model);
            //让初始选择目标系统关闭
            GameUtils.BattleManager.EnableSelectionOwner(false);
            //设置选择目标Layer
            InputUtils.SetMouseRayTraceLayer(PhysicsLayerDefine.GetFlag(PhysicTraceType.Pawn));
            m_Model = model;
            //1.计算可选格子， 将这些格子传递给HighlightTileAction
            CalcAvailableCells(m_AvailableCells, model);
            GraphUtils.Highlight(m_AvailableCells, true);
            
            //2.显示选择目标， 在目前在RoleEntityActor自身逻辑上做
            //后续如果有特殊逻辑可以放到这里
            //GameEntry.Event.Subscribe(MouseRayCastEventArgs.EventId, OnMouseRay);

            //3.Execute时从上下文model读取数据， 并利用表数据来执行逻辑
        }

        // private void OnMouseRay(object sender, GameEventArgs e)
        // {
        //     MouseRayCastEventArgs eventArgs = (MouseRayCastEventArgs)e;
        // }

        public override void Undo(BattleMainModel model)
        {
            base.Undo(model);
        }

        private void CalcAvailableCells(List<Vector3Int> cells, BattleMainModel model)
        {
            int releaseDistance = m_SkillData.Distance;

            var releaseOwner = GameEntry.Entity.GetEntity(model.SelectedOwnerId);
            if (releaseOwner == null)
            {
                Log.Error("技能释放者Entity为空, 检查");
                return;
            }
            
            using var resultNodes = new FPoolWrapper<List<IGraphNode>, IGraphNode>();
            
            float3 ownerPosition = EntityLogicUtils.GetPosition(releaseOwner.LogicInterface);

            GraphUtils.WorldToGraph(ownerPosition, out IGraphNode ownerBelongNode);
            GraphUtils.BFS(resultNodes.Value, ownerBelongNode, releaseDistance);
            
            foreach (var graphNode in resultNodes.Value)
            {
                //@TODO: 这里依赖类型了， 之后考虑把handle去掉直接用Vector3Int
                cells.Add(((TileGraphNodeHandle)graphNode.Handle).CellPos);
            }
            
        }

        public override void Execute(BattleMainModel model)
        {
            m_Executed = true;
            
            float damageNum = m_SkillData.DamageNum;
            var releaseOwner = GameEntry.Entity.GetEntity(m_Model.SelectedOwnerId);

            
            
            foreach (var effectCell in m_SkillEffectCells)
            {
                var standRoleEntity = GraphUtils.GetStandRole(effectCell);
                if (standRoleEntity != null)
                {
                    ApplyDamageInfo applyDamageInfo = ReferencePool.Acquire<ApplyDamageInfo>();
                    applyDamageInfo.DamageNum = damageNum;
                    applyDamageInfo.OweDamageEntityId = releaseOwner.Id;
                    applyDamageInfo.CasterEntityId = releaseOwner.Id;
                    applyDamageInfo.TargetEntityId = standRoleEntity.Id;
                    EntityLogicUtils.ApplyDamageTo(applyDamageInfo);
                }
            }
            
        }


        public override void OnTop(BattleMainModel model, IBattleAction lastTopAction)
        {
            base.OnTop(model, lastTopAction);

            if (m_Phase is ESKillActionPhase.ShouldConfirmRelease)
            {
                if (lastTopAction is SaveSelectEntityAction selectEntityAction 
                    && selectEntityAction.SelectEntityActionType is ESelectEntityActionType.SelectTarget)
                {
                    GraphUtils.Highlight(m_AvailableCells, true);
                    m_Phase = ESKillActionPhase.ChoseTarget;
                }
            }
        }

        public void Update(float deltaTime)
        {
            if (m_Phase is ESKillActionPhase.ChoseTarget)
            {
                //选择目标
                if (InputUtils.GetKeyDown(EKeyCode.Mouse0))
                {
                    PreSelectTarget();
                }
            }
            else if (m_Phase is ESKillActionPhase.ShouldConfirmRelease)
            {
                //如果再次点击的是原来已经选中的PawnEntity，就确认释放
                if (InputUtils.GetKeyDown(EKeyCode.Mouse0))
                {
                    ImpactInfo impactInfo = null;
                    var mouseRay = GameUtils.MouseRay;
                    if (PhysicsUtils.SingleLineCheck(mouseRay.origin.ToFloat3()
                            , mouseRay.direction.ToFloat3()
                            , 100.0f
                            , PhysicsLayerDefine.GetFlag(PhysicTraceType.Pawn)
                            , ref impactInfo))
                    {
                        //@TODO: 需要验证选中Target的合法性吗？
                        if (impactInfo.HitEntityId == m_Model.SelectedTargetId)
                        {
                            var executeAllAction = ReferencePool.Acquire<ExecuteAllAction>();
                            PushAction(executeAllAction);
                        }
                        //如果选中另一个了，就Undo撤回原来的目标选择，再将新的目标作为target
                        
                        //这样即使OnTop导致阶段回到了ChoseTarget，但马上就又会回到ShouldConfirmTarget
                        else
                        {
                            //PushAction(ReferencePool.Acquire<UndoAction>());
                            PreSelectTarget();
                        }
                    }
                    ImpactInfo.Recycle(impactInfo);
                }
            }
        }

        private void PreSelectTarget()
        {
            ImpactInfo impactInfo = null;
            var mouseRay = GameUtils.MouseRay;
            if(PhysicsUtils.SingleLineCheck(mouseRay.origin.ToFloat3()
                   , mouseRay.direction.ToFloat3()
                   ,100.0f
                   , PhysicsLayerDefine.GetFlag(PhysicTraceType.Pawn)
                   , ref impactInfo))
            {
                // @TODO: 依赖具体handle了
                // 只能在技能范围内选择
                if (GraphUtils.WorldToGraph(impactInfo.HitLocation, out IGraphNodeHandle handle) && m_AvailableCells.Contains(((TileGraphNodeHandle)handle).CellPos))
                {
                    GraphUtils.Highlight(m_AvailableCells, false);

                    
                    var selectEntityAction = ReferencePool.Acquire<SaveSelectEntityAction>();
                    selectEntityAction.SelectEntityActionType = ESelectEntityActionType.SelectTarget;
                    selectEntityAction.SelectedEntityId  = impactInfo.HitEntityId;
                    PushAction(selectEntityAction);
                    
                    CalcSkillEffectCells();
                    
                        
                    m_Phase = ESKillActionPhase.ShouldConfirmRelease;
                }

                
                        
            }
            ImpactInfo.Recycle(impactInfo);

        }

        private void CalcSkillEffectCells()
        {
            GraphUtils.Highlight(m_SkillEffectCells, false);

            int rangeType = m_SkillData.DamageRangeType;
            int rangeNum = m_SkillData.DamageRangeNum;
            float damageNum = m_SkillData.DamageNum;
            //取消显示
            //GraphUtils.Highlight(m_AvailableCells, false);

            m_SkillEffectCells.Clear();
            var releaseOwner = GameEntry.Entity.GetEntity(m_Model.SelectedOwnerId);
            if (releaseOwner == null)
            {
                Log.Error("技能释放者Entity为空, 检查");
                return;
            }

            var target = GameEntry.Entity.GetEntity(m_Model.SelectedTargetId);
            if (target == null)
            {
                Log.Error("技能目标为空，检查");
                return;
            }
            
            float3 ownerPosition = EntityLogicUtils.GetPosition(releaseOwner.LogicInterface);
            float3 targetPosition = EntityLogicUtils.GetPosition(target.LogicInterface);
            if (false == GraphUtils.WorldToGraph(targetPosition, out IGraphNode targetCenterNode))
            {
                Log.Error("无法根据target位置找到center位置");
                return;
            }


            switch (rangeType)
            {
                case 1:

                    //@TODO:
                    break;

                case 2:
                    if (false == GraphUtils.GetTilesRange(((TileGraphNodeHandle)targetCenterNode.Handle).CellPos, rangeNum,
                            m_SkillEffectCells))
                    {
                        Log.Error("无法获取TileRange");
                        return;
                    }

                    break;
                default:
                    Log.Error("Missing case, check");
                    break;
            }
            
            GraphUtils.Highlight(m_SkillEffectCells, true);

        }

        
        public override void Clear()
        {
            GraphUtils.Highlight(m_AvailableCells, false);
            GraphUtils.Highlight(m_SkillEffectCells, false);
            InputUtils.ResetMouseRayTraceLayer();
            GameUtils.BattleManager.EnableSelectionOwner(true);

            m_AvailableCells.Clear();
            m_SkillEffectCells.Clear();
            
            m_Model = null;
            m_Phase = ESKillActionPhase.ChoseTarget;
            m_Executed = false;
            m_SkillId = 0;
            m_SkillData = null;
            base.Clear();
        }

        
    }
}