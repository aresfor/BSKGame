// ReSharper disable InconsistentNaming
namespace Game.Gameplay
{
    public struct FSpawnedVFXInfo
    {
        public IVFX VFX;
    }
    public static class VFXUtils
    {
        // @todo 更好的做法? 需要考虑多个world吗?
        //private static Dictionary<World, IVFXService> _vfxServices = new Dictionary<World, IVFXService>();

        private static IVFXUtility s_VfxUtility;

        public static void Initialize(IVFXUtility vfxUtility)
        {
            s_VfxUtility = vfxUtility;
            s_VfxUtility?.Initialize();
        }
        public static int SpawnVFX(VFXBaseSpawnParam spawnParam)
        {
            var vfxUtility = GetVFXUtility();
            
            return vfxUtility.SpawnVFX(spawnParam);
        }

        private static IVFXUtility GetVFXUtility()
        {
            if (null == s_VfxUtility)
                s_VfxUtility = GlobalUtility.Get<IVFXUtility>();

            return s_VfxUtility;
        }
        
        public static void DeSpawnVFX(int vfxSerialId)
        {
            GetVFXUtility().DeSpawnVFX(vfxSerialId);
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

