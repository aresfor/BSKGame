namespace Game.Gameplay
{

    public enum VFXType : uint
    {
        Base = 0, //基本
        WeaponDecal = 1, //贴花

        // 2 待填充
        WeaponHit = 3, //命中、受击
        Bullet = 4, //子弹
        Ballistic = 5, //子弹轨迹(射线检测型子弹用 特殊的LineRenderer)
        Laser = 6,
        Skill = 7, //技能特效
        Max
    }
}