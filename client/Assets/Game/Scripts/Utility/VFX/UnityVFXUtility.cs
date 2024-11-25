using System.Collections.Generic;
using Game.Core;
using Game.Gameplay;

namespace Game.Client
{
    public class UnityVFXUtility: IVFXUtility, IUpdateable, IShutdown
    {
        private Dictionary<uint, IVFXFactory> VFXFactories = new();
        private BaseVFXFactory _VFXBaseFactory ;
        
        public void Initialize()
        {

            _VFXBaseFactory = new BaseVFXFactory(10);
            // 根据配置表，获取指定特效在当前设备上的最大数量
            VFXFactories.Add((uint)VFXType.Base, _VFXBaseFactory);                             //通用类型
            //VFXFactories.Add((uint)VFXType.WeaponDecal, new VFXFactory_WeaponDecal(10));     //贴花
            // Type2 等待填充
            //VFXFactories.Add((uint)VFXType.WeaponHit, new VFXFactory_WeaponHitEffect(10));   //击打特效
            //VFXFactories.Add((uint)VFXType.Bullet, new VFXFactory_BulletEffect(200));        //子弹弹体表现特效
            //VFXFactories.Add((uint)VFXType.Ballistic, new VFXFactory_Ballistic(200));        
            //VFXFactories.Add((uint)VFXType.Laser, new VFXFactory_Laser(200)); //（激光/射线）LineRenderer特效
        }

        public void Shutdown()
        {
            //clear and destroy all exist vfx
            foreach (var factory in VFXFactories)
            {
                factory.Value.ShutDown();
            }
        }

        public void Reset()
        {
            
        }

        public void Update(float deltaTime)
        {
            foreach (var factory in VFXFactories)
            {
                factory.Value.Update(deltaTime);
            }
        }

        

        public int SpawnVFX(VFXBaseSpawnParam vfxSpawnParam)
        {
            if (VFXFactories.TryGetValue(vfxSpawnParam.VFXTypeId, out IVFXFactory vfxFactory))
            {
                return vfxFactory.SpawnVFX(vfxSpawnParam);
            }

            return _VFXBaseFactory.SpawnVFX(vfxSpawnParam);
        }
        
        //@TODO: 根据不同Factory来DeSpawn
        public void DeSpawnVFX(int vfxSerialId)
        {
            _VFXBaseFactory.DeSpawnVFX(vfxSerialId);
        }

        public void PreloadVFX()
        {
            
        }
    }
}