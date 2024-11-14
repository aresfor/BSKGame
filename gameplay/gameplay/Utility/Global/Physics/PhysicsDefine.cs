namespace Game.Core;



public enum LayerType
{
    kDefault,
    kTypePawn,
    kTypeFirstPerson,
    kTypeStaticActor,
    kTypeProjectile,
    kTypeMonster,
    kTypeVehicle,
    kTypeAutoFire,
    kTypeAimAssist,
}

public enum EHitGroup : byte
{
    Default = 0,
    Head,
    Hand,
    Body,
    Belly,
    Foot,
    Shield,
    Left_Hand,
    Right_Hand,
    Left_Foot,
    Right_Foot,

    //DunPai,
    WheelFL,
    WheelFR,
    WheelRL,
    WheelRR,
};

public enum PhysicTraceType
{
    World = 0,
    StaticActor,
    StaticActor_ProjectileBreak,
    Pawn,
    FirstPerson,
    AimAssistance,
    Monster,
    Projectile,
    Vechicle,
    Fire,
    Ragdoll,
    AutoFire,
    Max,
}

public class PhysicsLayerDefine
{
    private static int GetTraceFlag(PhysicTraceType type)
    {
        return 1 << (int)type;
    }

    public static int GetFlag(PhysicTraceType type)
    {
        return GetTraceFlag(type);
    }

    public static int GetMultiFlag(PhysicTraceType type1, PhysicTraceType type2)
    {
        return GetTraceFlag(type1) | GetTraceFlag(type2);
    }

    public static void AddTraceFlagTypeValue(ref int result, PhysicTraceType type)
    {
        result |= GetTraceFlag(type);
    }

    public static int GetVisionAllFlag()
    {
        int result = 0;
        AddTraceFlagTypeValue(ref result, PhysicTraceType.World);
        AddTraceFlagTypeValue(ref result, PhysicTraceType.StaticActor);
        AddTraceFlagTypeValue(ref result, PhysicTraceType.StaticActor_ProjectileBreak);

        AddTraceFlagTypeValue(ref result, PhysicTraceType.Projectile);
        AddTraceFlagTypeValue(ref result, PhysicTraceType.Monster);
        AddTraceFlagTypeValue(ref result, PhysicTraceType.FirstPerson);

        return result;
    }

    public static int GetMonsterVisionAllFlag()
    {
        int result = 0;
        AddTraceFlagTypeValue(ref result, PhysicTraceType.StaticActor);
        AddTraceFlagTypeValue(ref result, PhysicTraceType.StaticActor_ProjectileBreak);
        AddTraceFlagTypeValue(ref result, PhysicTraceType.Pawn);
        //AddTraceFlagTypeValue(ref result, PhysicTraceType.Monster);

        return result;
    }

    public static int GetBulletCheckAllFlag()
    {
        int result = 0;
        AddTraceFlagTypeValue(ref result, PhysicTraceType.StaticActor);
        AddTraceFlagTypeValue(ref result, PhysicTraceType.StaticActor_ProjectileBreak);
        AddTraceFlagTypeValue(ref result, PhysicTraceType.Pawn);
        AddTraceFlagTypeValue(ref result, PhysicTraceType.Monster);

        return result;
    }

    public static int GetEnvironmentCheckAllFlag()
    {
        int result = 0;
        AddTraceFlagTypeValue(ref result, PhysicTraceType.StaticActor);
        AddTraceFlagTypeValue(ref result, PhysicTraceType.StaticActor_ProjectileBreak);

        return result;
    }

    public static int GetAoeCheckAllFlag()
    {
        int result = 0;
        AddTraceFlagTypeValue(ref result, PhysicTraceType.Pawn);
        AddTraceFlagTypeValue(ref result, PhysicTraceType.Monster);

        return result;
    }

    public static int GetAoeGroundCheckFlag()
    {
        int result = 0;
        AddTraceFlagTypeValue(ref result, PhysicTraceType.StaticActor);
        AddTraceFlagTypeValue(ref result, PhysicTraceType.StaticActor_ProjectileBreak);

        return result;
    }

    public static int GetLaserCheckAllFlag()
    {
        int result = 0;
        AddTraceFlagTypeValue(ref result, PhysicTraceType.StaticActor);
        AddTraceFlagTypeValue(ref result, PhysicTraceType.Pawn);
        AddTraceFlagTypeValue(ref result, PhysicTraceType.Monster);

        return result;
    }
    
    public static int GetInteractCheckAllFlag()
    {
        int result = 0;
        AddTraceFlagTypeValue(ref result, PhysicTraceType.Pawn);
        AddTraceFlagTypeValue(ref result, PhysicTraceType.Monster);

        return result;
    }
    
    public static int GetInteractCheckPawnFlag()
    {
        int result = 0;
        AddTraceFlagTypeValue(ref result, PhysicTraceType.Pawn);
        return result;
    }

    //具体功能性类型
    /// <summary>
    /// Same as GetBulletCheckAllFlag()
    /// </summary>
    public static int GetAttackCheckFlag()
    {
        //所有需要检测被击中的类型
        int result = 0;
        AddTraceFlagTypeValue(ref result, PhysicTraceType.StaticActor);
        AddTraceFlagTypeValue(ref result, PhysicTraceType.StaticActor_ProjectileBreak);

        AddTraceFlagTypeValue(ref result, PhysicTraceType.Pawn);
        AddTraceFlagTypeValue(ref result, PhysicTraceType.Monster);

        return result;
    }

    public static int GetAimAssistCheckFlag()
    {
        int result = 0;
        AddTraceFlagTypeValue(ref result, PhysicTraceType.StaticActor);
        AddTraceFlagTypeValue(ref result, PhysicTraceType.StaticActor_ProjectileBreak);
        AddTraceFlagTypeValue(ref result, PhysicTraceType.Pawn);
        AddTraceFlagTypeValue(ref result, PhysicTraceType.Monster);
        AddTraceFlagTypeValue(ref result, PhysicTraceType.AimAssistance);

        return result;
    }

    public static int GetAutoFireCheckFlag()
    {
        int result = 0;
        AddTraceFlagTypeValue(ref result, PhysicTraceType.StaticActor);
        AddTraceFlagTypeValue(ref result, PhysicTraceType.StaticActor_ProjectileBreak);
        AddTraceFlagTypeValue(ref result, PhysicTraceType.Pawn);
        AddTraceFlagTypeValue(ref result, PhysicTraceType.Monster);
        AddTraceFlagTypeValue(ref result, PhysicTraceType.AutoFire);

        return result;
    }
}

/// <summary>
/// 命中部位映射
/// 角色预制上命名不同且碰撞盒更多，要做多对一映射
/// </summary>
public static class HitGroupName
{
    public const string BodyName = "Spine_M_Collider";
    public const string ChestName = "Chest_M_Collider";
    public const string Body_M_Name = "Waist_M_Collider";
    public const string Body_B_Name = "Spine1_M_Collider";


    public const string HeadName = "Head_M_Collider";

    //脚未适配，下面有具体的左右脚。猜测可能不是人身上的碰撞盒。
    public const string FootName = "Foot";

    //手未适配，下面有具体的左右手。猜测可能不是人身上的碰撞盒。
    public const string HandName = "Hand";

    //盾牌也没有。
    public const string ShieldName = "Shield";
    public const string LFootName = "Toes_L_Collider";
    public const string RFootName = "Toes_R_Collider";
    public const string LHandName = "Wrist_L_Collider";

    public const string RHandName = "Wrist_R_Collider";

    //盾牌出现两个
    //public const string DunPai = "DunPai";
    //轮胎的这几个先放着不管。
    public const string WheelFL = "WheelFL";
    public const string WheelFR = "WheelFR";
    public const string WheelRL = "WheelRL";
    public const string WheelRR = "WheelRR";
    public const string SteelGun01 = "Gun_M_Collider";

    //额外的碰撞盒，要做映射。
    //肘部(下臂)和手映射
    public const string LElbow = "Elbow_L_Collider";

    public const string RElbow = "Elbow_R_Collider";

    //肩膀(上臂)和手映射
    public const string LShoulder = "Shoulder_L_Collider";

    public const string RShoulder = "Shoulder_R_Collider";

    //膝部(小腿)和脚映射
    public const string LKnee = "Knee_L_Collider";

    public const string RKnee = "Knee_R_Collider";

    //臀部(大腿)和脚映射
    public const string LHip = "Hip_L_Collider";
    public const string RHip = "Hip_R_Collider";

    //映射已实现多对一，例如左手，左小臂，左大臂都对应EHitGroup.Left_Hand
    //因为EHitGroup固定不新增，而TPS项目中的角色预制体碰撞盒分的更精细，所以才映射。
    public static EHitGroup NameToEnum(string inName)
    {
        switch (inName)
        {
            case BodyName:
            case SteelGun01:
            case Body_M_Name:
            case Body_B_Name:
            case ChestName:
                return EHitGroup.Body;
            case HeadName:
                return EHitGroup.Head;
            case FootName:
                return EHitGroup.Foot;
            case HandName:
                return EHitGroup.Hand;
            case ShieldName:
                return EHitGroup.Shield;
            case LFootName:
            case LKnee:
            case LHip:
                return EHitGroup.Left_Foot;
            case RFootName:
            case RKnee:
            case RHip:
                return EHitGroup.Right_Foot;
            case LHandName:
            case LElbow:
            case LShoulder:
                return EHitGroup.Left_Hand;
            case RHandName:
            case RElbow:
            case RShoulder:
                return EHitGroup.Right_Hand;
            //case DunPai:
            //return EHitGroup.DunPai;
            case WheelFL:
                return EHitGroup.WheelFL;
            case WheelFR:
                return EHitGroup.WheelFR;
            case WheelRL:
                return EHitGroup.WheelRL;
            case WheelRR:
                return EHitGroup.WheelRR;
        }

        return EHitGroup.Default;
    }

    public static string EnumToName(EHitGroup inEnum)
    {
        switch (inEnum)
        {
            case EHitGroup.Body:
                return BodyName;
            //case EHitGroup.Belly:
            //return BellyName;
            case EHitGroup.Head:
                return HeadName;
            case EHitGroup.Foot:
                return FootName;
            case EHitGroup.Hand:
                return HandName;
            case EHitGroup.Shield:
                return ShieldName;
            case EHitGroup.Left_Foot:
                return LFootName;
            case EHitGroup.Right_Foot:
                return RFootName;
            case EHitGroup.Left_Hand:
                return LHandName;
            case EHitGroup.Right_Hand:
                return RHandName;
            //case EHitGroup.DunPai:
            //return DunPai;
        }

        return BodyName;
    }
}