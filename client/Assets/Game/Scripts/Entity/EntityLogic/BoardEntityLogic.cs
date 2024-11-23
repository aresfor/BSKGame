using System.Collections;
using System.Collections.Generic;
using Game.Client;
using Game.Core;
using Game.Gameplay;
using Game.Math;
using GameFramework;
using GameFramework.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using Log = UnityGameFramework.Runtime.Log;

namespace Game.Client
{
    
    
    public class BoardEntityLogic:GameEntityLogic
    {

        [SerializeField] [Range(1, 20)]private int Row = 10;
        [SerializeField] [Range(1, 20)]private int Column = 10;
        
        private BoardGraph m_Board;
        private BoardEntityModel m_Model;

        public BoardGraph Board => m_Board;
        private Ray MouseRay => Camera.main.ScreenPointToRay(Input.mousePosition);

        private LatticeEntityLogic m_LastEnterLattice;
        public override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_Model = (BoardEntityModel)userData;
            m_Board = new BoardGraph(Row, Column);
            m_Board.FinishGenerationCall += OnFinishGeneration;

            //这里可以根据model配置数据来做一些自定义生成
            //m_MonoBoard.Generate();

        }

        protected override void CreateGameplayEntity()
        {
            base.CreateGameplayEntity();
            GameplayEntity = new BoardGameplayEntity(Entity);
        }

        public override void OnShow(object userData)
        {
            base.OnShow(userData);

            GameUtils.BoardEntityId = Id;
            //让棋盘朝上
            CachedTransform.Rotate(new Vector3(90,0,0));
            Generate();

        }

        private void OnFinishGeneration()
        {
            var spawnNode = m_Board.GetNode(Row / 2, Column / 2);
            var latticeData = spawnNode.Value.EntityData;
            var roleModel = new RoleEntityModel()
            {
                Id = EntityId.GenerateSerialId(),
                TypeId = 40000,
                Position = latticeData.Position,
                BelongLatticeId = spawnNode.Value.Entity.Id
                //Rotation = latticeData.Rotation
            };
            GameEntry.Entity.ShowGameplayEntity("Role", roleModel);
        }
        private void Generate()
        {
            m_Board.Generate(CachedTransform.position.ToFloat3()
            , CachedTransform.rotation.ToQuaternion(), Entity.Id);
        }
        
        public bool GetMouseHoverLattice(out LatticeEntityLogic latticeLogic)
        {
            var mouseRay = MouseRay;
            latticeLogic = null;
            ImpactInfo impactInfo = ImpactInfo.Alloc();
            bool result = false;

            if(PhysicsUtils.SingleLineCheck(mouseRay.origin.ToFloat3(), mouseRay.direction.ToFloat3()
               , 100.0f, PhysicsLayerDefine.GetFlag(PhysicTraceType.Entity)
               , ref impactInfo, true, duration:3.0f))
            {
                var entity = GameEntry.Entity.GetEntity(impactInfo.HitEntityId);
                if (null == entity)
                {
                    Log.Info($"MouseHitEntity is null, entityId: {impactInfo.HitEntityId}");
                }
                else if (entity.Logic is LatticeEntityLogic lattice)
                {
                    lattice.PointerDown();
                    latticeLogic = lattice;
                    Log.Info($"GetLatticeEntity: {latticeLogic.gameObject.name}");
                    result = true;
                    
                    var resultNodes = ListPool<IGraphNode<LatticeGameplayEntity>>.Get();
                    
                    //m_Board.BFS(resultNodes, lattice.Lattice, 2);
                    FArrayGraphNodeHandle handle = (FArrayGraphNodeHandle)lattice.LatticeNode.Handle;
                    var goalHandle = m_Board.CreateHandle(handle.Row + 3, handle.Column + 2);
                    if (m_Board.BFS(lattice.LatticeNode, m_Board.FindNode(goalHandle), resultNodes))
                    {
                        Log.Info("找到path");
                        
                        foreach (var node in resultNodes)
                        {
                            Log.Info($"path: {node.Name}");
                        }
                    }
                    else
                    {
                        Log.Error("未找到path");
                        
                    }
                    
                    
            
                    resultNodes.Clear();
                    ListPool<IGraphNode<LatticeGameplayEntity>>.Release(resultNodes);

                }
            }
            else
            {
                Log.Error("未命中任何Entity");
            }

            
            ImpactInfo.Recycle(impactInfo);
            return result;
        }

        private void UpdateRayCast()
        {
            var mouseRay = MouseRay;
            ImpactInfo impactInfo = ImpactInfo.Alloc();
            bool result = false;

            if (PhysicsUtils.SingleLineCheck(mouseRay.origin.ToFloat3(), mouseRay.direction.ToFloat3()
                    , 100.0f, PhysicsLayerDefine.GetFlag(PhysicTraceType.Entity)
                    , ref impactInfo, true, duration:0.0f))
            {
                var entity = GameEntry.Entity.GetEntity(impactInfo.HitEntityId);
                if (null == entity)
                {
                    //Log.Info($"MouseHitEntity is null, entityId: {impactInfo.HitEntityId}");
                }
                else if (entity.Logic is LatticeEntityLogic lattice)
                {
                    if (m_LastEnterLattice != null)
                    {
                        m_LastEnterLattice.PointerExit();
                    }
                    m_LastEnterLattice = lattice;
                    lattice.PointerEnter();
                }
            }
            else
            {
                if (m_LastEnterLattice != null)
                {
                    m_LastEnterLattice.PointerExit();
                }
            }
        }
        
        public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            UpdateRayCast();
            
            //@TODO: 临时
            if (Input.GetMouseButtonDown(0))
            {
                GetMouseHoverLattice(out var latticeLogic);
            }

            if (Input.GetMouseButtonUp(1))
            {
                var mouseRay = MouseRay;
                ImpactInfo impactInfo = ImpactInfo.Alloc();
                bool result = false;

                if (PhysicsUtils.SingleLineCheck(mouseRay.origin.ToFloat3(), mouseRay.direction.ToFloat3()
                        , 100.0f, PhysicsLayerDefine.GetFlag(PhysicTraceType.Entity)
                        , ref impactInfo, true, duration: 3.0f))
                {
                    var entity = GameEntry.Entity.GetEntity(impactInfo.HitEntityId);
                    if (null == entity)
                    {
                        //Log.Info($"MouseHitEntity is null, entityId: {impactInfo.HitEntityId}");
                    }
                    else if (entity.Logic is LatticeEntityLogic lattice)
                    {
                        lattice.PointerUp();
                    }
                }
            }

        }

        public override void OnHide(bool isShutdown, object userData)
        {
            m_LastEnterLattice = null;
            base.OnHide(isShutdown, userData);
        }
    }
}