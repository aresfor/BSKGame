using System;
using System.Threading;
using System.Threading.Tasks;
using GameFramework.Resource;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

namespace Game.Client
{
    /// <summary>
    /// 资源管理器接口。
    /// </summary>
    public interface IYooResourceManager
    {
        /// <summary>
        /// 获取当前资源适用的游戏版本号。
        /// </summary>
        string ApplicableGameVersion { get; }

        /// <summary>
        /// 获取当前内部资源版本号。
        /// </summary>
        int InternalResourceVersion { get; }

        /// <summary>
        /// 获取或设置运行模式。
        /// </summary>
        EPlayMode PlayMode { get; set; }

        /// <summary>
        /// 缓存系统启动时的验证级别。
        /// </summary>
        EVerifyLevel VerifyLevel { get; set; }

        /// <summary>
        /// 同时下载的最大数目。
        /// </summary>
        int DownloadingMaxNum { get; set; }

        /// <summary>
        /// 失败重试最大数目。
        /// </summary>
        int FailedTryAgain { get; set; }

        /// <summary>
        /// 获取资源只读区路径。
        /// </summary>
        string ReadOnlyPath { get; }

        /// <summary>
        /// 获取资源读写区路径。
        /// </summary>
        string ReadWritePath { get; }

        /// <summary>
        /// 设置资源只读区路径。
        /// </summary>
        /// <param name="readOnlyPath">资源只读区路径。</param>
        void SetReadOnlyPath(string readOnlyPath);

        /// <summary>
        /// 设置资源读写区路径。
        /// </summary>
        /// <param name="readWritePath">资源读写区路径。</param>
        void SetReadWritePath(string readWritePath);

        /// <summary>
        /// 初始化接口。
        /// </summary>
        void Initialize();

        /// <summary>
        /// 初始化操作。
        /// </summary>
        /// <param name="packageName">资源包名称。</param>
        Task<InitializationOperation> InitPackage(string packageName);

        /// <summary>
        /// 默认资源包名称。
        /// </summary>
        string DefaultPackageName { get; set; }

        /// <summary>
        /// 获取或设置异步系统参数，每帧执行消耗的最大时间切片（单位：毫秒）。
        /// </summary>
        long Milliseconds { get; set; }

        Transform InstanceRoot { get; set; }

        /// <summary>
        /// 热更链接URL。
        /// </summary>
        string HostServerURL { get; set; }
        
        /// <summary>
        /// 获取或设置资源对象池自动释放可释放对象的间隔秒数。
        /// </summary>
        float AssetAutoReleaseInterval
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置资源对象池的容量。
        /// </summary>
        int AssetCapacity
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置资源对象池对象过期秒数。
        /// </summary>
        float AssetExpireTime
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置资源对象池的优先级。
        /// </summary>
        int AssetPriority
        {
            get;
            set;
        }

        /// <summary>
        /// 卸载资源。
        /// </summary>
        /// <param name="asset">要卸载的资源。</param>
        void UnloadAsset(object asset);

        /// <summary>
        /// 资源回收（卸载引用计数为零的资源）
        /// </summary>
        void UnloadUnusedAssets();

        /// <summary>
        /// 强制回收所有资源
        /// </summary>
        void ForceUnloadAllAssets();

        

        /// <summary>
        /// 检查资源定位地址是否有效。
        /// </summary>
        /// <param name="location">资源的定位地址</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
        bool CheckLocationValid(string location, string packageName = "");

        /// <summary>
        /// 获取资源信息列表。
        /// </summary>
        /// <param name="resTag">资源标签。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
        /// <returns>资源信息列表。</returns>
        AssetInfo[] GetAssetInfos(string resTag, string packageName = "");

        /// <summary>
        /// 获取资源信息列表。
        /// </summary>
        /// <param name="tags">资源标签列表。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
        /// <returns>资源信息列表。</returns>
        AssetInfo[] GetAssetInfos(string[] tags, string packageName = "");

        /// <summary>
        /// 获取资源信息。
        /// </summary>
        /// <param name="location">资源的定位地址。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
        /// <returns>资源信息。</returns>
        AssetInfo GetAssetInfo(string location, string packageName = "");


        /// <summary>
        /// 同步加载游戏物体并实例化。
        /// </summary>
        /// <param name="location">资源的定位地址。</param>
        /// <param name="parent">资源实例父节点。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
        /// <returns>资源实例。</returns>
        /// <remarks>会实例化资源到场景，无需主动UnloadAsset，Destroy时自动UnloadAsset。</remarks>
        GameObject LoadGameObject(string location, Transform parent = null, string packageName = "");

        /// <summary>
        /// 异步加载游戏物体并实例化。
        /// </summary>
        /// <param name="location">资源定位地址。</param>
        /// <param name="parent">资源实例父节点。</param>
        /// <param name="cancellationToken">取消操作Token。</param>
        /// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
        /// <returns>异步游戏物体实例。</returns>
        /// <remarks>会实例化资源到场景，无需主动UnloadAsset，Destroy时自动UnloadAsset。</remarks>
        //Task<GameObject> LoadGameObjectAsync(string location, Transform parent = null, string packageName = "");
        void LoadGameObjectAsync(string location, Action<GameObject> onCompleted, Action onFail = null,
            Transform parent = null,
            string packageName = "");
    }
}