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
            m_Board = new BoardGraph(Row, Column, CachedTransform.position.ToFloat3());
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

            GameUtils.Board = Board;
            //让棋盘朝上
            CachedTransform.Rotate(new Vector3(90,0,0));
            m_Board.WorldPosition = CachedTransform.position.ToFloat3();
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
            latticeLogic = null;

            //检测是否在UI上
            if (EventSystem.current.IsPointerOverGameObject())
                return false;
            
            var mouseRay = MouseRay;
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
                    lattice.PointerDown(new FPointerEventData()
                    {
                        pointerWorldPos = impactInfo.HitLocation
                    });
                    latticeLogic = lattice;
                    //Log.Info($"MouseDown hit LatticeEntity: {latticeLogic.gameObject.name}");
                    result = true;
                }
            }
            else
            {
                Log.Error("未命中任何Entity");
            }

            if(impactInfo!= null)
                ImpactInfo.Recycle(impactInfo);
            return result;
        }

        private void UpdateRayCast()
        {
            //检测是否在UI上
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            
            var mouseRay = MouseRay;
            ImpactInfo impactInfo = ImpactInfo.Alloc();
            bool result = false;

            if (PhysicsUtils.SingleLineCheck(mouseRay.origin.ToFloat3(), mouseRay.direction.ToFloat3()
                    , 100.0f, PhysicsLayerDefine.GetFlag(PhysicTraceType.Entity)|PhysicsLayerDefine.GetFlag(PhysicTraceType.UI)
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
                        m_LastEnterLattice.PointerExit(new FPointerEventData()
                        {
                            pointerWorldPos = impactInfo.HitLocation
                        });
                    }
                    m_LastEnterLattice = lattice;
                    lattice.PointerEnter(new FPointerEventData()
                    {
                        pointerWorldPos = impactInfo.HitLocation
                    });
                }
            }
            else
            {
                if (m_LastEnterLattice != null)
                {
                    m_LastEnterLattice.PointerExit(new FPointerEventData());
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
        }

        public override void OnHide(bool isShutdown, object userData)
        {
            m_LastEnterLattice = null;
            base.OnHide(isShutdown, userData);
        }
    }
}