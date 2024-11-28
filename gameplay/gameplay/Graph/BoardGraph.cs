using Game.Core;
using Game.Math;
using GameFramework;
using GameFramework.DataTable;
using GameFramework.Entity;
using GameFramework.Runtime;

namespace Game.Gameplay
{
    public class BoardGraph : ArrayGraph<LatticeGameplayEntity>
    {
        //[Min(1)] public float BoardWidth = 1;
        //[Min(1)] public float BoardHeight = 1;
        
        public Action FinishGenerationCall;
        public int BoardEntityId { get; private set; }
        public void GenerateFinished()
        {
            FinishGenerationCall?.Invoke();
        }

        public BoardGraph(int boardEntityId, int inRow, int inColumn, float3 graphWorldPosition) : base(inRow, inColumn, graphWorldPosition)
        {
            BoardEntityId = boardEntityId;
        }

        public IGraphNode<LatticeGameplayEntity> GetNode(int row, int column)
        {
            return FindNode(CreateHandle(row, column));
        }

        public IGraphNode<LatticeGameplayEntity> GetNode(float3 position)
        {
            //@TODO: 
            return null;
        }
        public void Generate(float3 boardPosition, quaternion boardRotation, int boardEntityId)
        {
            Clear();
            
            
            
            for (int i = 0; i < Row; ++i)
            {
                float positionZ = i * (NodeMarginHeight +NodeHeight);
                for (int j = 0; j < Column; ++j)
                {
                    float positionX = j * (NodeMarginWidth + NodeWidth);

                    var lattice = ReferencePool.Acquire<LatticeNode>();
                    Array[i, j] = lattice;
                    lattice.OnInit(this, new FArrayGraphNodeHandle(i,j));
                    var latticeModel = new LatticeEntityModel(this
                        , new float3(positionX, 0, positionZ) + boardPosition
                        , boardRotation
                        , lattice)
                    {
                        Id = EntityId.GenerateSerialId(),
                        TypeId = 30000
                    };
                    
                    
                    IDataTable<DREntity> dtEntity = GameFrameworkEntry.GetModule<IDataTableManager>().GetDataTable<DREntity>();
                    DREntity drEntity = dtEntity.GetDataRow(latticeModel.TypeId);
                    if (drEntity == null)
                    {
                        Logs.Warning("Can not load entity id '{0}' from data table.", latticeModel.TypeId.ToString());
                        return;
                    }
                    GameFrameworkEntry.GetModule<IEntityManager>().ShowEntity(latticeModel.Id, AssetUtility.GetEntityAsset(drEntity.AssetName)
                        , "Lattice"
                        , Constant.AssetPriority.GameplayAsset, ShowEntityInfo.Create(null, latticeModel));
                    
                    //GameEntry.Entity.AttachEntity(latticeModel.Id, boardEntityId);
                    
                }
            }


        }

        
    }
}