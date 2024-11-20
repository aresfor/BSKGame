using System.Collections;
using System.Collections.Generic;
using Game.Client;
using Game.Core;
using Game.Gameplay;
using Game.Math;
using GameFramework;
using UnityEngine;
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
        private Ray m_MouseRay => Camera.main.ScreenPointToRay(Input.mousePosition);
        
        public override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_Model = (BoardEntityModel)userData;
            m_Board = new BoardGraph(Row, Column);

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

            //让棋盘朝上
            CachedTransform.Rotate(new Vector3(90,0,0));
            Generate();

        }

        private void Generate()
        {
            m_Board.Generate(CachedTransform.position.ToFloat3()
            , CachedTransform.rotation.ToQuaternion(), Entity.Id);
        }
        
        public bool GetMouseHoverLattice(out LatticeEntityLogic latticeLogic)
        {
            var mouseRay = m_MouseRay;
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
        
        public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            //@TODO: 临时
            if (Input.GetMouseButtonDown(0))
            {
                GetMouseHoverLattice(out var latticeLogic);
            }
        }
        
    }
}