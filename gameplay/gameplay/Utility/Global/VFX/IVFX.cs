namespace Game.Gameplay;

public interface IVFXFactory
{
    public IVFX SpawnVFX(VFXBaseSpawnParam vfxSpawnParam);
    public void DeSpawnVFX(IVFX vfx);

    public void Update(float deltaime);
    public void ShutDown();
}

public interface IVFX
{
    /// <summary>
    /// 由哪个factory创建的，用于在手动Destroy时候，找到创建者，由创建者销毁
    /// </summary>
    IVFXFactory SpawnFactory
    {
        get;
    }
    /// <summary>
    /// 初始化特效参数
    /// </summary>
    /// <param name="tbvfxBean">配置表里，关于这个特效的基础通用配置，比如持续时间，销毁方式</param>
    /// <param name="factory">生成特效的factory，用于在销毁时候找到正确的创建工厂，谁创建谁销毁</param>
    /// <param name="param">初始化填充这个特效的参数</param>
    //void InitVFXParam(TBVFXBean tbvfxBean,IVFXFactory factory,VFXBaseSpawnParam param);
    
    void InitVFXParam(IVFXFactory factory,VFXBaseSpawnParam param);

    /// <summary>
    /// 重播这个特效
    /// </summary>
    void ReplayVFX();
    /// <summary>
    /// 暂停特效播放
    /// </summary>
    void PauseVFX();
}