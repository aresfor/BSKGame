using Game.Math;
using GameFramework;

namespace Game.Gameplay
{

    public class ApplyDamageInfo : IReference
    {
        //接收伤害的目标ID， 不可为空
        public int TargetEntityId;

        //伤害来源的目标ID，可为空
        public int CasterEntityId;

        //伤害大小
        public float DamageNum;

        //HitTypeMask，用来保存一些命中标识，例如几种护盾，弱点等，只有PointDamage有
        //public int HitTypeMask;
        //外界不要随意操作，记录当次伤害是否击杀了目标
        public bool bKillTargetEntity;

        //伤害的属性标记
        //public EDamageAttributeMarkers DamageAttributeMark;
        //伤害的类型标记
        //public EDamageType DamageType;
        //伤害的Tag标记
        //public VersionList<int>? DamageTags;
        //造成伤害的方式
        //public EDamageWay DamageWay;
        //实际造成伤害的实体Id，可为空
        public int OweDamageEntityId;

        public virtual void Clear()
        {
            TargetEntityId = default;
            CasterEntityId = default;
            DamageNum = default;
            bKillTargetEntity = default;
            OweDamageEntityId = default;
        }

        public static ApplyDamageInfo Create(int targetEntityId, int damageNum, int casterEntityId = 0,
            int oweDamageEntityId = 0, bool bKillTarget = false)
        {
            ApplyDamageInfo info = ReferencePool.Acquire<ApplyDamageInfo>();
            info.TargetEntityId = targetEntityId;
            info.DamageNum = damageNum;
            info.CasterEntityId = casterEntityId;
            info.OweDamageEntityId = oweDamageEntityId;
            info.bKillTargetEntity = bKillTarget;
            return info;
        }
    }

    public class ApplyPointDamageInfo : ApplyDamageInfo
    {
        //点伤直接使用检测检测结果进行组装
        //部位
        public EHitGroup HitGroup;
        //
        //public float3 CasterEntityPos;
        //public float3 TargetEntityPos;

        public float3 HitNormal;
        public float3 HitPos;
        public float3 HitDir;

        public override void Clear()
        {
            base.Clear();
            HitGroup = default;
            //TargetEntityPos = default;
            HitNormal = default;
            HitPos = default;
            //CasterEntityPos = default;
            HitDir = default;
        }


        public static ApplyPointDamageInfo Create(int targetEntityId, int damageNum, EHitGroup hitGroup,
            float3 hitNormal, float3 hitPos, float3 hitDir, int casterEntityId = 0,
            int oweDamageEntityId = 0, bool bKillTarget = false)
        {
            ApplyPointDamageInfo info = ReferencePool.Acquire<ApplyPointDamageInfo>();
            info.TargetEntityId = targetEntityId;
            info.DamageNum = damageNum;
            info.CasterEntityId = casterEntityId;
            info.OweDamageEntityId = oweDamageEntityId;
            info.bKillTargetEntity = bKillTarget;
            info.HitGroup = hitGroup;
            info.HitNormal = hitNormal;
            info.HitPos = hitPos;
            info.HitDir = hitDir;
            return info;
        }
    }

    public class ApplyRadicalDamageInfo : ApplyDamageInfo
    {
        //范围伤害中心位置
        public float3 RadicalCenterPoint;

        //范围伤害命中实体位置
        public float3 HitPos;


        public override void Clear()
        {
            base.Clear();

            RadicalCenterPoint = default;
            HitPos = default;
        }

        public static ApplyRadicalDamageInfo Create(int targetEntityId, int damageNum, float3 radicalCenter,
            float3 hitPos, int casterEntityId = 0,
            int oweDamageEntityId = 0, bool bKillTarget = false)
        {
            ApplyRadicalDamageInfo info = ReferencePool.Acquire<ApplyRadicalDamageInfo>();
            info.TargetEntityId = targetEntityId;
            info.DamageNum = damageNum;
            info.CasterEntityId = casterEntityId;
            info.OweDamageEntityId = oweDamageEntityId;
            info.bKillTargetEntity = bKillTarget;
            info.RadicalCenterPoint = radicalCenter;
            info.HitPos = hitPos;
            return info;
        }
    }
}