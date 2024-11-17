// ReSharper disable InconsistentNaming
namespace Game.Gameplay
{
    public struct FSpawnedVFXInfo
    {
        public IVFX VFX;
        public IVFXUtility Service;
    }
    public static class VFXUtils
    {
        // @todo 更好的做法? 需要考虑多个world吗?
        //private static Dictionary<World, IVFXService> _vfxServices = new Dictionary<World, IVFXService>();

        private static IVFXUtility mVfxUtility;
        
        public static IVFX SpawnVFX(VFXBaseSpawnParam spawnParam)
        {
            var vfxUtility = GetVFXUtility();
            
            return vfxUtility.SpawnVFX(spawnParam);
        }

        private static IVFXUtility GetVFXUtility()
        {
            if (null == mVfxUtility)
                mVfxUtility = GlobalUtility.Get<IVFXUtility>();

            return mVfxUtility;
        }
        public static FSpawnedVFXInfo SpawnVFXWithInfo(VFXBaseSpawnParam spawnParam)
        {
            var vfxUtility = GetVFXUtility();
            return new FSpawnedVFXInfo()
            {
                VFX = vfxUtility.SpawnVFX(spawnParam),
                Service = vfxUtility
            };
        }

        public static void DeSpawnVFX(FSpawnedVFXInfo info)
        {
            if(info.Service != null)
                info.Service.DeSpawnVFX(info.VFX);
        }
        
        // public static IVFX SpawnVFXForPreload(World world, TBVFXBean tbVfxBean)
        // {
        //     switch (tbVfxBean.Id)
        //     {
        //         
        //     }
        // }
    }
}

