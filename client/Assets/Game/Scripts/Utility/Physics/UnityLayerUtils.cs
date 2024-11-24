using Game.Gameplay;
using UnityEngine;

namespace Game.Client
{
    public class UnityLayersUtil
    {
        private const string LayerName_UI = "UI";                                   //UI
        private const string LayerName_World = "Gameplay/World";              //空气墙
        private const string LayerName_StaticActor = "Gameplay/StaticActor";  //场景静态
        private const string LayerName_StaticActor_ProjectileBreak = "Gameplay/StaticActor_Breakable"; //投掷物及子弹穿透，服务器端直接击碎

        private const string LayerName_Pawn = "Gameplay/Pawn";                    //友军角色玩家模型
        private const string LayerName_FirstPerson = "Gameplay/FirstPerson";      //1p玩家
        private const string LayerName_AimAssistance = "Gameplay/AimAssistance"; //辅助瞄准
        private const string LayerName_Monster = "Gameplay/Monster";              //怪，npc
        private const string LayerName_Projectile = "Gameplay/Projectile";        //投掷物，武器，
        private const string LayerName_Fire = "Gameplay/Fire";  //细分开火框NotPhy
        private const string LayerName_Flying = "Gameplay/Flying";  //飞行层
        private const string LayerName_AutoFire = "Gameplay/AutoFire";  //自动开火层

        private const string LayerName_Vehicle = "Gameplay/Vechicle";          //载具
        private const string LayerName_Entity = "Gameplay/Entity";

        public static int LayerMask_UI { get; } = LayerMask.NameToLayer(LayerName_UI);
        public static int LayerMask_World { get; } = LayerMask.NameToLayer(LayerName_World);
        public static int LayerMask_Pawn { get; } = LayerMask.NameToLayer(LayerName_Pawn);
        public static int LayerMask_Entity { get; } = LayerMask.NameToLayer(LayerName_Entity);

        public static int LayerMask_FirstPerson { get; } = LayerMask.NameToLayer(LayerName_FirstPerson);
        public static int LayerMask_Monster { get; } = LayerMask.NameToLayer(LayerName_Monster);
        public static int LayerMask_Projectile { get; } = LayerMask.NameToLayer(LayerName_Projectile);
        public static int LayerMask_StaticActor { get; } = LayerMask.NameToLayer(LayerName_StaticActor);
        public static int LayerMask_StaticActor_ProjectileBreak { get; } = LayerMask.NameToLayer(LayerName_StaticActor_ProjectileBreak);
        
        public static int LayerMask_Fire{ get; } = LayerMask.NameToLayer(LayerName_Fire);
        public static int LayerMask_AimAssistance { get; } = LayerMask.NameToLayer(LayerName_AimAssistance);
        
        public static int LayerMask_Vehicle { get; } = LayerMask.NameToLayer(LayerName_Vehicle);
        
        public static int LayerMask_Flying { get; } = LayerMask.NameToLayer(LayerName_Flying);
        public static int LayerMask_AutoFire { get; } = LayerMask.NameToLayer(LayerName_AutoFire);


        public static int GetLayerMask(params int[] layerIndices)
        {
            int layerMask = 0;
            foreach (var layerIndex in layerIndices)
            {
                layerMask |= 1 << layerIndex;
            }

            return layerMask;
        }
        
        public static bool IsStaticActor(int layer)
        {
            return LayerMask_StaticActor == layer;
        }
        
        public static bool IsFireActor(int layer)
        {
            return LayerMask_Fire == layer;
        }
        public static LayerType LayerNameToLayerType(int unitylayer)
        {
            // @todo Vehicle重复了2次
            if (unitylayer == LayerMask_Pawn || unitylayer == LayerMask_FirstPerson || unitylayer == LayerMask_Vehicle)
            {
                return LayerType.kTypePawn;
            }
            else if (unitylayer == LayerMask_Monster)
            {
                return LayerType.kTypeMonster;
            }
            else if (unitylayer == LayerMask_World || unitylayer == LayerMask_StaticActor )
            {
                return LayerType.kTypeStaticActor;
            }
            else if (unitylayer == LayerMask_Projectile)
            {
                return LayerType.kTypeProjectile;
            }
            else if (unitylayer == LayerMask_Vehicle)
            {
                return LayerType.kTypeVehicle;
            }
            else if (unitylayer == LayerMask_AimAssistance)
            {
                return LayerType.kTypeAimAssist;
            }
            else if (unitylayer == LayerMask_AutoFire)
            {
                return LayerType.kTypeAutoFire;
            }
            else if (unitylayer == LayerMask_Flying) //@todo 和Monster合并?
            {
                return LayerType.kTypeAutoFire;
            }
            else if (unitylayer == LayerMask_Entity)
            {
                return LayerType.kTypeEntity;
            }
            else if (unitylayer == LayerMask_UI)
            {
                return LayerType.kTypeUI;
            }

            return LayerType.kDefault;
        }
    }
}