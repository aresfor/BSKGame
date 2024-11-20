using Game.Core;
using Game.Math;
using GameFramework;
using GameFramework.DataTable;
using GameFramework.Entity;
using GameFramework.Runtime;

namespace Game.Gameplay
{
    public class Board : ArrayGraph<LatticeGameplayEntity>
    {
        //[Min(1)] public float BoardWidth = 1;
        //[Min(1)] public float BoardHeight = 1;
        public float LatticeWidth = 1;
        public float LatticeHeight = 1;
        

        public Board(int inRow, int inColumn) : base(inRow, inColumn)
        {
            
        }

        public void Generate(float3 boardPosition, quaternion boardRotation, int boardEntityId)
        {
            Clear();
            
            var perHeight = LatticeHeight * 1.1f;
            var perWidth = LatticeWidth * 1.1f;
            
            float rowPositionX = LatticeWidth; 
            for (int i = 0; i < Row; ++i)
            {
                float positionZ = i * perHeight + LatticeHeight;
                for (int j = 0; j < Column; ++j)
                {
                    float positionX = j * perWidth + rowPositionX;

                    var lattice = ReferencePool.Acquire<Lattice>();
                    Array[i, j] = lattice;
                    var latticeModel = new LatticeEntityModel(this,i, j
                        , new float3(positionX, boardPosition.y, positionZ)
                        , boardRotation
                        , lattice
                        , boardEntityId
                        , LatticeWidth, LatticeHeight)
                    {
                        Id = EntityId.GenerateSerialId(),
                        TypeId = 30000
                    };
                    
                    
                    IDataTable<DREntity> dtEntity = GameFrameworkEntry.GetModule<IDataTableManager>().GetDataTable<DREntity>();
                    DREntity drEntity = dtEntity.GetDataRow(latticeModel.TypeId);
                    if (drEntity == null)
                    {
                        Log.Warning("Can not load entity id '{0}' from data table.", latticeModel.TypeId.ToString());
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