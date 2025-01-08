// using System;
// using System.Collections.Generic;
// using System.Threading;
// using System.Threading.Tasks;
// using Game.Client.Game.Client;
// using GameFramework;
// using GameFramework.ObjectPool;
// using GameFramework.Resource;
// using UnityEngine;
// using UnityEngine.Serialization;
// using UnityGameFramework.Runtime;
// using YooAsset;
// using Object = UnityEngine.Object;
//
// namespace Game.Client
// {
//     /// <summary>
//     /// 资源组件。
//     /// </summary>
//     [DisallowMultipleComponent]
//     [AddComponentMenu("Game Framework/YooResource")]
//     public partial class YooResourceComponent : GameFrameworkComponent, IYooResourceManager
//     {
//         #region Propreties
//
//         private IObjectPool<YooAssetObject> m_AssetPool;
//         
//         private const int DefaultPriority = 0;
//
//         //private IYooResourceManager m_YooResourceManager;
//
//         private bool m_ForceUnloadUnusedAssets = false;
//
//         private bool m_PreorderUnloadUnusedAssets = false;
//
//         private bool m_PerformGCCollect = false;
//
//         private AsyncOperation m_AsyncOperation = null;
//
//         private float m_LastUnloadUnusedAssetsOperationElapseSeconds = 0f;
//
//         [SerializeField] private float m_MinUnloadUnusedAssetsInterval = 60f;
//
//         [SerializeField] private float m_MaxUnloadUnusedAssetsInterval = 300f;
//
//         [SerializeField] private bool m_UseSystemUnloadUnusedAssets = true;
//         
//         public string FallbackHostServerURL { get; set; }
//
//         /// <summary>
//         /// 资源包列表。
//         /// </summary>
//         private Dictionary<string, ResourcePackage> PackageMap { get; } = new Dictionary<string, ResourcePackage>();
//         
//         /// <summary>
//         /// 正在加载的资源列表。
//         /// </summary>
//         private readonly HashSet<string> _assetLoadingList = new HashSet<string>();
//         
//         /// <summary>
//         /// 默认资源包。
//         /// </summary>
//         internal ResourcePackage DefaultPackage { private set; get; }
//         
//         /// <summary>
//         /// 资源服务器地址。
//         /// </summary>
//         public string HostServerURL { get; set; }
//         
//         /// <summary>
//         /// 实例化的根节点。
//         /// </summary>
//         public Transform InstanceRoot { get; set; }
//
//         /// <summary>
//         /// 当前最新的包裹版本。
//         /// </summary>
//         public string PackageVersion { set; get; }
//
//         /// <summary>
//         /// 资源包名称。
//         /// </summary>
//         public string DefaultPackageName { get; set; } = "DefaultPackage";
//
//         /// <summary>
//         /// 资源系统运行模式。
//         /// </summary>
//         [SerializeField] private EPlayMode playMode = EPlayMode.EditorSimulateMode;
//
//         /// <summary>
//         /// 资源信息列表。
//         /// </summary>
//         private readonly Dictionary<string, AssetInfo> _assetInfoMap = new Dictionary<string, AssetInfo>();
//         
//         /// <summary>
//         /// 资源系统运行模式。
//         /// <remarks>编辑器内优先使用。</remarks>
//         /// </summary>
//         public EPlayMode PlayMode
//         {
//             get
//             {
// #if UNITY_EDITOR
//                 //编辑器模式使用。
//                 return (EPlayMode)UnityEditor.EditorPrefs.GetInt("EditorPlayMode");
// #else
//                 if (playMode == EPlayMode.EditorSimulateMode)
//                 {
//                     playMode = EPlayMode.OfflinePlayMode;
//                 }
//                 //运行时使用。
//                 return playMode;
// #endif
//             }
//             set
//             {
// #if UNITY_EDITOR
//                 playMode = value;
// #endif
//             }
//         }
//         
//         /// <summary>
//         /// 是否支持边玩边下载。
//         /// </summary>
//         [SerializeField] private bool m_UpdatableWhilePlaying = false;
//
//         /// <summary>
//         /// 是否支持边玩边下载。
//         /// </summary>
//         public bool UpdatableWhilePlaying => m_UpdatableWhilePlaying;
//             
//         /// <summary>
//         /// 下载文件校验等级。
//         /// </summary>
//         public EVerifyLevel VerifyLevel { get; set; } = EVerifyLevel.Middle;
//
//         [SerializeField] private ReadWritePathType m_ReadWritePathType = ReadWritePathType.Unspecified;
//
//         /// <summary>
//         /// 设置异步系统参数，每帧执行消耗的最大时间切片（单位：毫秒）
//         /// </summary>
//         [SerializeField] public long Milliseconds { get; set; } = 30;
//
//         public int m_DownloadingMaxNum = 10;
//
//         /// <summary>
//         /// 获取或设置同时最大下载数目。
//         /// </summary>
//         public int DownloadingMaxNum
//         {
//             get => m_DownloadingMaxNum;
//             set => m_DownloadingMaxNum = value;
//         }
//
//         public int m_FailedTryAgain = 3;
//
//         public int FailedTryAgain
//         {
//             get => m_FailedTryAgain;
//             set => m_FailedTryAgain = value;
//         }
//
//         /// <summary>
//         /// 获取当前资源适用的游戏版本号。
//         /// </summary>
//         public string ApplicableGameVersion{ get; set; }
//
//         /// <summary>
//         /// 获取当前内部资源版本号。
//         /// </summary>
//         public int InternalResourceVersion{ get; set; }
//
//         /// <summary>
//         /// 获取资源读写路径类型。
//         /// </summary>
//         public ReadWritePathType ReadWritePathType => m_ReadWritePathType;
//
//         /// <summary>
//         /// 获取或设置无用资源释放的最小间隔时间，以秒为单位。
//         /// </summary>
//         public float MinUnloadUnusedAssetsInterval
//         {
//             get => m_MinUnloadUnusedAssetsInterval;
//             set => m_MinUnloadUnusedAssetsInterval = value;
//         }
//
//         /// <summary>
//         /// 获取或设置无用资源释放的最大间隔时间，以秒为单位。
//         /// </summary>
//         public float MaxUnloadUnusedAssetsInterval
//         {
//             get => m_MaxUnloadUnusedAssetsInterval;
//             set => m_MaxUnloadUnusedAssetsInterval = value;
//         }
//         
//         /// <summary>
//         /// 使用系统释放无用资源策略。
//         /// </summary>
//         public bool UseSystemUnloadUnusedAssets
//         {
//             get => m_UseSystemUnloadUnusedAssets;
//             set => m_UseSystemUnloadUnusedAssets = value;
//         }
//
//         /// <summary>
//         /// 获取无用资源释放的等待时长，以秒为单位。
//         /// </summary>
//         public float LastUnloadUnusedAssetsOperationElapseSeconds => m_LastUnloadUnusedAssetsOperationElapseSeconds;
//
//         /// <summary>
//         /// 获取资源只读路径。
//         /// </summary>
//         public string ReadOnlyPath{ get; set; }
//
//         /// <summary>
//         /// 获取资源读写路径。
//         /// </summary>
//         public string ReadWritePath{ get; set; }
//
//         [SerializeField]
//         private float m_AssetAutoReleaseInterval = 60f;
//
//         [SerializeField]
//         private int m_AssetCapacity = 64;
//
//         [SerializeField]
//         private float m_AssetExpireTime = 60f;
//
//         [SerializeField]
//         private int m_AssetPriority = 0;
//         
//         /// <summary>
//         /// 获取或设置资源对象池自动释放可释放对象的间隔秒数。
//         /// </summary>
//         public float AssetAutoReleaseInterval
//         {
//             get
//             {
//                 return m_AssetAutoReleaseInterval;
//             }
//             set
//             {
//                 m_AssetAutoReleaseInterval = value;
//             }
//         }
//         
//         /// <summary>
//         /// 获取或设置资源对象池的容量。
//         /// </summary>
//         public int AssetCapacity
//         {
//             get
//             {
//                 return m_AssetCapacity;
//             }
//             set
//             { 
//                 m_AssetCapacity = value;
//             }
//         }
//
//         /// <summary>
//         /// 获取或设置资源对象池对象过期秒数。
//         /// </summary>
//         public float AssetExpireTime
//         {
//             get
//             {
//                 return m_AssetExpireTime;
//             }
//             set
//             {
//                 m_AssetExpireTime = value;
//             }
//         }
//
//         /// <summary>
//         /// 获取或设置资源对象池的优先级。
//         /// </summary>
//         public int AssetPriority
//         {
//             get
//             {
//                 return m_AssetPriority;
//             }
//             set
//             {
//                  m_AssetPriority = value;
//             }
//         }
//
//         //@TODO:
//         private IResourceHelper m_ResourceHelper;
//         
//         #endregion
//         #region 设置接口
//
//         /// <summary>
//         /// 设置资源只读区路径。
//         /// </summary>
//         /// <param name="readOnlyPath">资源只读区路径。</param>
//         public void SetReadOnlyPath(string readOnlyPath)
//         {
//             if (string.IsNullOrEmpty(readOnlyPath))
//             {
//                 throw new GameFrameworkException("Read-only path is invalid.");
//             }
//
//             ReadOnlyPath = readOnlyPath;
//         }
//
//         /// <summary>
//         /// 设置资源读写区路径。
//         /// </summary>
//         /// <param name="readWritePath">资源读写区路径。</param>
//         public void SetReadWritePath(string readWritePath)
//         {
//             if (string.IsNullOrEmpty(readWritePath))
//             {
//                 throw new GameFrameworkException("Read-write path is invalid.");
//             }
//
//             ReadWritePath = readWritePath;
//         }
//
//         #endregion
//         private void Start()
//         {
//             BaseComponent baseComponent = UnityGameFramework.Runtime.GameEntry.GetComponent<BaseComponent>();
//             if (baseComponent == null)
//             {
//                 Log.Fatal("Base component is invalid.");
//                 return;
//             }
//             
//             // m_YooResourceManager = GameFramework.GameFrameworkEntry.GetModule<IYooResourceManager>();
//             // if (m_YooResourceManager == null)
//             // {
//             //     Log.Fatal("Resource component is invalid.");
//             //     return;
//             // }
//
//             if (PlayMode == EPlayMode.EditorSimulateMode)
//             {
//                 Log.Info("During this run, Game Framework will use editor resource files, which you should validate first.");
// #if !UNITY_EDITOR
//                 PlayMode = EPlayMode.OfflinePlayMode;
// #endif
//             }
//
//             SetReadOnlyPath(Application.streamingAssetsPath);
//             if (m_ReadWritePathType == ReadWritePathType.TemporaryCache)
//             {
//                 SetReadWritePath(Application.temporaryCachePath);
//             }
//             else
//             {
//                 if (m_ReadWritePathType == ReadWritePathType.Unspecified)
//                 {
//                     m_ReadWritePathType = ReadWritePathType.PersistentData;
//                 }
//
//                 SetReadWritePath(Application.persistentDataPath);
//             }
//             
//             PlayMode = PlayMode;
//             VerifyLevel = VerifyLevel;
//             Milliseconds = Milliseconds;
//             InstanceRoot = transform;
//             Initialize();
//             HostServerURL = SettingsUtils.GetResDownLoadPath();
//             AssetAutoReleaseInterval = m_AssetAutoReleaseInterval;
//             AssetCapacity = m_AssetCapacity;
//             AssetExpireTime = m_AssetExpireTime;
//             AssetPriority = m_AssetPriority;
//             Log.Info($"ResourceComponent Run Mode：{PlayMode}");
//         }
//         public void Initialize()
//         {
//             // 初始化资源系统
//             if (!YooAssets.Initialized)
//             {
//                 YooAssets.Initialize(new ResourceLogger());
//             }
//             YooAssets.SetOperationSystemMaxTimeSlice(Milliseconds);
//
//             // 创建默认的资源包
//             string packageName = DefaultPackageName;
//             var defaultPackage = YooAssets.TryGetPackage(packageName);
//             if (defaultPackage == null)
//             {
//                 defaultPackage = YooAssets.CreatePackage(packageName);
//                 YooAssets.SetDefaultPackage(defaultPackage);
//                 DefaultPackage = defaultPackage;
//             }
//
//             //CancellationToken = InstanceRoot.gameObject.GetCancellationTokenOnDestroy();
//
//             IObjectPoolManager objectPoolManager = GameFrameworkEntry.GetModule<IObjectPoolManager>();
//             SetObjectPoolManager(objectPoolManager);
//         }
//         /// <summary>
//         /// 设置对象池管理器。
//         /// </summary>
//         /// <param name="objectPoolManager">对象池管理器。</param>
//         public void SetObjectPoolManager(IObjectPoolManager objectPoolManager)
//         {
//             if (objectPoolManager == null)
//             {
//                 throw new GameFrameworkException("Object pool manager is invalid.");
//             }
//             m_AssetPool = objectPoolManager.CreateMultiSpawnObjectPool<YooAssetObject>("Asset Pool");
//         }
//         
//         /// <summary>
//         /// 初始化操作。
//         /// </summary>
//         /// <returns></returns>
//         public async Task<InitializationOperation> InitPackage(string packageName = "")
//         {
//             // if (m_YooResourceManager == null)
//             // {
//             //     Log.Fatal("Resource component is invalid.");
//             //     return null;
//             // }
//
//             var pacName = string.IsNullOrEmpty(packageName) ? DefaultPackageName : packageName;
//             #if UNITY_EDITOR
//             //编辑器模式使用。
//             EPlayMode playMode = (EPlayMode)UnityEditor.EditorPrefs.GetInt("EditorPlayMode");
//             Log.Warning($"Editor Module Used :{playMode}");
// #else
//             //运行时使用。
//             EPlayMode playMode = (EPlayMode)PlayMode;
// #endif
//
//             if (PackageMap.ContainsKey(pacName))
//             {
//                 Log.Error($"ResourceSystem has already init package : {pacName}");
//                 return null;
//             }
//
//             // 创建资源包裹类
//             var package = YooAssets.TryGetPackage(pacName);
//             if (package == null)
//             {
//                 package = YooAssets.CreatePackage(pacName);
//             }
//             PackageMap[pacName] = package;
//
//             // 编辑器下的模拟模式
//             InitializationOperation initializationOperation = null;
//             if (playMode == EPlayMode.EditorSimulateMode)
//             {
//                 var createParameters = new EditorSimulateModeParameters();
//                 createParameters.CacheBootVerifyLevel = VerifyLevel;
//                 createParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(EDefaultBuildPipeline.BuiltinBuildPipeline, pacName);
//                 initializationOperation = package.InitializeAsync(createParameters);
//             }
//
//             // 单机运行模式
//             if (playMode == EPlayMode.OfflinePlayMode)
//             {
//                 var createParameters = new OfflinePlayModeParameters();
//                 createParameters.CacheBootVerifyLevel = VerifyLevel;
//                 createParameters.DecryptionServices = new FileStreamDecryption();
//                 initializationOperation = package.InitializeAsync(createParameters);
//             }
//
//             // 联机运行模式
//             if (playMode == EPlayMode.HostPlayMode)
//             {
//                 string defaultHostServer = HostServerURL;
//                 string fallbackHostServer = FallbackHostServerURL;
//                 var createParameters = new HostPlayModeParameters();
//                 createParameters.CacheBootVerifyLevel = VerifyLevel;
//                 createParameters.DecryptionServices = new FileStreamDecryption();
//                 createParameters.BuildinQueryServices = new GameQueryServices();
//                 createParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
//                 initializationOperation = package.InitializeAsync(createParameters);
//             }
//
//             // WebGL运行模式
//             if (playMode == EPlayMode.WebPlayMode)
//             {
//                 string defaultHostServer = HostServerURL;
//                 string fallbackHostServer = FallbackHostServerURL;
//                 var createParameters = new WebPlayModeParameters();
//                 createParameters.CacheBootVerifyLevel = VerifyLevel;
//                 createParameters.DecryptionServices = new FileStreamDecryption();
//                 createParameters.BuildinQueryServices = new GameQueryServices();
//                 createParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
//                 initializationOperation = package.InitializeAsync(createParameters);
//             }
//
//             await initializationOperation.Task;
//
//             Log.Info($"Init resource package version : {initializationOperation?.PackageVersion}");
//
//             return initializationOperation;
//         }
//
//         #region 版本更新
//         /// <summary>
//         /// 获取当前资源包版本。
//         /// </summary>
//         /// <param name="customPackageName">指定资源包的名称。不传使用默认资源包</param>
//         /// <returns>资源包版本。</returns>
//         public string GetPackageVersion(string customPackageName = "")
//         {
//             var package = string.IsNullOrEmpty(customPackageName)
//                 ? YooAssets.GetPackage(DefaultPackageName)
//                 : YooAssets.GetPackage(customPackageName);
//             if (package == null)
//             {
//                 return string.Empty;
//             }
//
//             return package.GetPackageVersion();
//         }
//
//         /// <summary>
//         /// 异步更新最新包的版本。
//         /// </summary>
//         /// <param name="appendTimeTicks">请求URL是否需要带时间戳。</param>
//         /// <param name="timeout">超时时间。</param>
//         /// <param name="customPackageName">指定资源包的名称。不传使用默认资源包</param>
//         /// <returns>请求远端包裹的最新版本操作句柄。</returns>
//         public UpdatePackageVersionOperation UpdatePackageVersionAsync(bool appendTimeTicks = false, int timeout = 60,
//             string customPackageName = "")
//         {
//             var package = string.IsNullOrEmpty(customPackageName)
//                 ? YooAssets.GetPackage(DefaultPackageName)
//                 : YooAssets.GetPackage(customPackageName);
//             return package.UpdatePackageVersionAsync(appendTimeTicks, timeout);
//         }
//
//         /// <summary>
//         /// 向网络端请求并更新清单
//         /// </summary>
//         /// <param name="packageVersion">更新的包裹版本</param>
//         /// <param name="autoSaveVersion">更新成功后自动保存版本号，作为下次初始化的版本。</param>
//         /// <param name="timeout">超时时间（默认值：60秒）</param>
//         /// <param name="customPackageName">指定资源包的名称。不传使用默认资源包</param>
//         public UpdatePackageManifestOperation UpdatePackageManifestAsync(string packageVersion,
//             bool autoSaveVersion = true, int timeout = 60, string customPackageName = "")
//         {
//             var package = string.IsNullOrEmpty(customPackageName)
//                 ? YooAssets.GetPackage(DefaultPackageName)
//                 : YooAssets.GetPackage(customPackageName);
//             return package.UpdatePackageManifestAsync(packageVersion,autoSaveVersion , timeout);
//         }
//         
//         /// <summary>
//         /// 资源下载器，用于下载当前资源版本所有的资源包文件。
//         /// </summary>
//         public ResourceDownloaderOperation Downloader { get; set; }
//         
//         /// <summary>
//         /// 创建资源下载器，用于下载当前资源版本所有的资源包文件。
//         /// </summary>
//         /// <param name="customPackageName">指定资源包的名称。不传使用默认资源包</param>
//         public ResourceDownloaderOperation CreateResourceDownloader(string customPackageName = "")
//         {
//             if (string.IsNullOrEmpty(customPackageName))
//             {
//                 var package = YooAssets.GetPackage(DefaultPackageName);
//                 Downloader = package.CreateResourceDownloader(DownloadingMaxNum, FailedTryAgain);
//                 return Downloader;
//             }
//             else
//             {
//                 var package = YooAssets.GetPackage(customPackageName);
//                 Downloader = package.CreateResourceDownloader(DownloadingMaxNum, FailedTryAgain);
//                 return Downloader;
//             }
//         }
//         
//         /// <summary>
//         /// 清理包裹未使用的缓存文件。
//         /// </summary>
//         /// <param name="customPackageName">指定资源包的名称。不传使用默认资源包</param>
//         public ClearUnusedCacheFilesOperation ClearUnusedCacheFilesAsync(string customPackageName = "")
//         {
//             var package = string.IsNullOrEmpty(customPackageName)
//                 ? YooAssets.GetPackage(DefaultPackageName)
//                 : YooAssets.GetPackage(customPackageName);
//             return package.ClearUnusedCacheFilesAsync();
//         }
//
//         /// <summary>
//         /// 清理沙盒路径。
//         /// </summary>
//         /// <param name="customPackageName">指定资源包的名称。不传使用默认资源包</param>
//         public void ClearSandbox(string customPackageName = "")
//         {
//             var package = string.IsNullOrEmpty(customPackageName)
//                 ? YooAssets.GetPackage(DefaultPackageName)
//                 : YooAssets.GetPackage(customPackageName);
//             package.ClearPackageSandbox();
//         }
//         #endregion
//
//         #region 获取资源
//         /// <summary>
//         /// 检查资源是否存在。
//         /// </summary>
//         /// <param name="location">要检查资源的名称。</param>
//         /// <param name="customPackageName">指定资源包的名称。不传使用默认资源包</param>
//         /// <returns>检查资源是否存在的结果。</returns>
//         public HasAssetResult HasAsset(string location, string customPackageName = "")
//         {
//             if (string.IsNullOrEmpty(location))
//             {
//                 throw new GameFrameworkException("Asset name is invalid.");
//             }
//
//             AssetInfo assetInfo = GetAssetInfo(location, customPackageName);
//
//             if (!CheckLocationValid(location))
//             {
//                 return HasAssetResult.InValid;
//             }
//
//             if (assetInfo == null)
//             {
//                 return HasAssetResult.NotExist;
//             }
//
//             if (IsNeedDownloadFromRemote(assetInfo))
//             {
//                 return HasAssetResult.AssetOnline;
//             }
//
//             return HasAssetResult.AssetOnDisk;
//             //return m_YooResourceManager.HasAsset(location, packageName: customPackageName);
//         }
//         
//         /// <summary>
//         /// 是否需要从远端更新下载。
//         /// </summary>
//         /// <param name="assetInfo">资源信息。</param>
//         /// <param name="packageName">资源包名称。</param>
//         public bool IsNeedDownloadFromRemote(AssetInfo assetInfo, string packageName = "")
//         {
//             if (string.IsNullOrEmpty(packageName))
//             {
//                 return YooAssets.IsNeedDownloadFromRemote(assetInfo);
//             }
//             else
//             {
//                 var package = YooAssets.GetPackage(packageName);
//                 return package.IsNeedDownloadFromRemote(assetInfo);
//             }
//         }
//         
//         /// <summary>
//         /// 检查资源定位地址是否有效。
//         /// </summary>
//         /// <param name="location">资源的定位地址</param>
//         /// <param name="customPackageName">指定资源包的名称。不传使用默认资源包</param>
//         public bool CheckLocationValid(string location, string customPackageName = "")
//         {
//             if (string.IsNullOrEmpty(customPackageName))
//             {
//                 return YooAssets.CheckLocationValid(location);
//             }
//             else
//             {
//                 var package = YooAssets.GetPackage(customPackageName);
//                 return package.CheckLocationValid(location);
//             }
//             //return m_YooResourceManager.CheckLocationValid(location, packageName: customPackageName);
//         }
//         
//         /// <summary>
//         /// 获取资源信息列表。
//         /// </summary>
//         /// <param name="resTag">资源标签。</param>
//         /// <param name="customPackageName">指定资源包的名称。不传使用默认资源包</param>
//         /// <returns>资源信息列表。</returns>
//         public AssetInfo[] GetAssetInfos(string resTag, string customPackageName = "")
//         {
//             if (string.IsNullOrEmpty(customPackageName))
//             {
//                 return YooAssets.GetAssetInfos(tag);
//             }
//             else
//             {
//                 var package = YooAssets.GetPackage(customPackageName);
//                 return package.GetAssetInfos(tag);
//             }
//             //return m_YooResourceManager.GetAssetInfos(resTag, packageName: customPackageName);
//         }
//
//         /// <summary>
//         /// 获取资源信息列表。
//         /// </summary>
//         /// <param name="tags">资源标签列表。</param>
//         /// <param name="customPackageName">指定资源包的名称。不传使用默认资源包</param>
//         /// <returns>资源信息列表。</returns>
//         public AssetInfo[] GetAssetInfos(string[] tags, string customPackageName = "")
//         {
//             if (string.IsNullOrEmpty(customPackageName))
//             {
//                 return YooAssets.GetAssetInfos(tags);
//             }
//             else
//             {
//                 var package = YooAssets.GetPackage(customPackageName);
//                 return package.GetAssetInfos(tags);
//             }
//             //return m_YooResourceManager.GetAssetInfos(tags, packageName: customPackageName);
//         }
//
//         /// <summary>
//         /// 获取资源信息。
//         /// </summary>
//         /// <param name="location">资源的定位地址。</param>
//         /// <param name="customPackageName">指定资源包的名称。不传使用默认资源包</param>
//         /// <returns>资源信息。</returns>
//         public AssetInfo GetAssetInfo(string location, string customPackageName = "")
//         {
//             if (string.IsNullOrEmpty(location))
//             {
//                 throw new GameFrameworkException("Asset name is invalid.");
//             }
//
//             if (string.IsNullOrEmpty(customPackageName))
//             {
//                 if (_assetInfoMap.TryGetValue(location, out AssetInfo assetInfo))
//                 {
//                     return assetInfo;
//                 }
//
//                 assetInfo = YooAssets.GetAssetInfo(location);
//                 _assetInfoMap[location] = assetInfo;
//                 return assetInfo;
//             }
//             else
//             {
//                 string key = $"{customPackageName}/{location}";
//                 if (_assetInfoMap.TryGetValue(key, out AssetInfo assetInfo))
//                 {
//                     return assetInfo;
//                 }
//
//                 var package = YooAssets.GetPackage(customPackageName);
//                 if (package == null)
//                 {
//                     throw new GameFrameworkException($"The package does not exist. Package Name :{customPackageName}");
//                 }
//
//                 assetInfo = package.GetAssetInfo(location);
//                 _assetInfoMap[key] = assetInfo;
//                 return assetInfo;
//             }
//             //return m_YooResourceManager.GetAssetInfo(location, packageName: customPackageName);
//         }
//
//         public async void LoadAssetAsync(string location, int priority, LoadAssetCallbacks loadAssetCallbacks, object userData,
//             string packageName = "")
//         {
//             if (string.IsNullOrEmpty(location))
//             {
//                 throw new GameFrameworkException("Asset name is invalid.");
//             }
//
//             if (loadAssetCallbacks == null)
//             {
//                 throw new GameFrameworkException("Load asset callbacks is invalid.");
//             }
//             
//             string assetObjectKey = GetCacheKey(location, packageName);
//             
//             
//             float duration = Time.time;
//             
//             YooAssetObject assetObject = m_AssetPool.Spawn(assetObjectKey);
//             if (assetObject != null)
//             {
//                 await Task.Yield();
//                 loadAssetCallbacks.LoadAssetSuccessCallback(location, assetObject.Target, Time.time - duration, userData);
//                 return;
//             }
//             
//             _assetLoadingList.Add(assetObjectKey);
//
//             AssetInfo assetInfo = GetAssetInfo(location, packageName);
//
//             if (!string.IsNullOrEmpty(assetInfo.Error))
//             {
//                 _assetLoadingList.Remove(assetObjectKey);
//                 
//                 string errorMessage = Utility.Text.Format("Can not load asset '{0}' because :'{1}'.", location, assetInfo.Error);
//                 if (loadAssetCallbacks.LoadAssetFailureCallback != null)
//                 {
//                     loadAssetCallbacks.LoadAssetFailureCallback(location, LoadResourceStatus.NotExist, errorMessage, userData);
//                     return;
//                 }
//
//                 throw new GameFrameworkException(errorMessage);
//             }
//             
//             AssetHandle handle = GetHandleAsync(location, assetInfo.AssetType, packageName: packageName);
//
//             if (loadAssetCallbacks.LoadAssetUpdateCallback != null)
//             {
//                 InvokeProgress(location, handle, loadAssetCallbacks.LoadAssetUpdateCallback, userData);
//             }
//             
//             await handle.Task;
//
//             if (handle.AssetObject == null || handle.Status == EOperationStatus.Failed)
//             {
//                 _assetLoadingList.Remove(assetObjectKey);
//                 
//                 string errorMessage = Utility.Text.Format("Can not load asset '{0}'.", location);
//                 if (loadAssetCallbacks.LoadAssetFailureCallback != null)
//                 {
//                     loadAssetCallbacks.LoadAssetFailureCallback(location, LoadResourceStatus.NotReady, errorMessage, userData);
//                     return;
//                 }
//
//                 throw new GameFrameworkException(errorMessage);
//             }
//             else
//             {
//                 assetObject = YooAssetObject.Create(assetObjectKey, handle.AssetObject, handle,this);
//                 m_AssetPool.Register(assetObject, true);
//                 
//                 _assetLoadingList.Remove(assetObjectKey);
//
//                 if (loadAssetCallbacks.LoadAssetSuccessCallback != null)
//                 {
//                     duration = Time.time - duration;
//
//                     loadAssetCallbacks.LoadAssetSuccessCallback(location, handle.AssetObject, duration, userData);
//                 }
//             }
//         }
//         private async Task InvokeProgress(string location, AssetHandle assetHandle, LoadAssetUpdateCallback loadAssetUpdateCallback, object userData)
//         {
//             if (string.IsNullOrEmpty(location))
//             {
//                 throw new GameFrameworkException("Asset name is invalid.");
//             }
//             
//             if (loadAssetUpdateCallback != null)
//             {
//                 while (assetHandle is { IsValid: true, IsDone: false })
//                 {
//                     await Task.Yield();
//                 
//                     loadAssetUpdateCallback.Invoke(location, assetHandle.Progress, userData);
//                 }
//             }
//         }
//         #endregion
//
//         #region 加载资源
//
//         /// <summary>
//         /// 异步加载资源。
//         /// </summary>
//         /// <param name="location">资源的定位地址。</param>
//         /// <param name="assetType">要加载资源的类型。</param>
//         /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
//         /// <param name="userData">用户自定义数据。</param>
//         /// <param name="packageName">指定资源包的名称。不传使用默认资源包。</param>
//         public void LoadAssetAsync(string location, Type assetType, LoadAssetCallbacks loadAssetCallbacks, object userData = null, string packageName = "")
//         {
//             LoadAssetAsync(location, assetType, DefaultPriority, loadAssetCallbacks, userData, packageName);
//         }
//
//         /// <summary>
//         /// 异步加载资源。
//         /// </summary>
//         /// <param name="location">资源的定位地址。</param>
//         /// <param name="assetType">要加载资源的类型。</param>
//         /// <param name="priority">加载资源的优先级。</param>
//         /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
//         /// <param name="userData">用户自定义数据。</param>
//         /// <param name="packageName">指定资源包的名称。不传使用默认资源包。</param>
//         public async void LoadAssetAsync(string location, Type assetType, int priority, LoadAssetCallbacks loadAssetCallbacks, object userData, string packageName = "")
//         {
//             if (string.IsNullOrEmpty(location))
//             {
//                 Log.Error("Asset name is invalid.");
//                 return;
//             }
//
//             string assetObjectKey = GetCacheKey(location, packageName);
//             
//             //await TryWaitingLoading(assetObjectKey);
//             
//             float duration = Time.time;
//             
//             YooAssetObject assetObject = m_AssetPool.Spawn(assetObjectKey);
//             if (assetObject != null)
//             {
//                 await Task.Yield();
//                 loadAssetCallbacks.LoadAssetSuccessCallback(location, assetObject.Target, Time.time - duration, userData);
//                 return;
//             }
//             
//             _assetLoadingList.Add(assetObjectKey);
//             
//             AssetInfo assetInfo = GetAssetInfo(location, packageName);
//
//             if (!string.IsNullOrEmpty(assetInfo.Error))
//             {
//                 _assetLoadingList.Remove(assetObjectKey);
//                 
//                 string errorMessage = Utility.Text.Format("Can not load asset '{0}' because :'{1}'.", location, assetInfo.Error);
//                 if (loadAssetCallbacks.LoadAssetFailureCallback != null)
//                 {
//                     loadAssetCallbacks.LoadAssetFailureCallback(location, LoadResourceStatus.NotExist, errorMessage, userData);
//                     return;
//                 }
//
//                 throw new GameFrameworkException(errorMessage);
//             }
//
//             AssetHandle handle = GetHandleAsync(location, assetType, packageName: packageName);
//
//             if (loadAssetCallbacks.LoadAssetUpdateCallback != null)
//             {
//                 InvokeProgress(location, handle, loadAssetCallbacks.LoadAssetUpdateCallback, userData);
//             }
//             
//             await handle.Task;
//
//             if (handle.AssetObject == null || handle.Status == EOperationStatus.Failed)
//             {
//                 _assetLoadingList.Remove(assetObjectKey);
//                 
//                 string errorMessage = Utility.Text.Format("Can not load asset '{0}'.", location);
//                 if (loadAssetCallbacks.LoadAssetFailureCallback != null)
//                 {
//                     loadAssetCallbacks.LoadAssetFailureCallback(location, LoadResourceStatus.NotReady, errorMessage, userData);
//                     return;
//                 }
//
//                 throw new GameFrameworkException(errorMessage);
//             }
//             else
//             {
//                 assetObject = YooAssetObject.Create(assetObjectKey, handle.AssetObject, handle,this);
//                 m_AssetPool.Register(assetObject, true);
//                 
//                 _assetLoadingList.Remove(assetObjectKey);
//                 
//                 if (loadAssetCallbacks.LoadAssetSuccessCallback != null)
//                 {
//                     duration = Time.time - duration;
//                     
//                     loadAssetCallbacks.LoadAssetSuccessCallback(location, handle.AssetObject, duration, userData);
//                 }
//             }
//             //m_YooResourceManager.LoadAssetAsync(location, assetType, priority, loadAssetCallbacks, userData, packageName);
//         }
//
//         /// <summary>
//         /// 同步加载资源。
//         /// </summary>
//         /// <param name="location">资源的定位地址。</param>
//         /// <param name="packageName">指定资源包的名称。不传使用默认资源包。</param>
//         /// <typeparam name="T">要加载资源的类型。</typeparam>
//         /// <returns>资源实例。</returns>
//         public T LoadAsset<T>(string location, string packageName = "") where T : UnityEngine.Object
//         {
//             if (string.IsNullOrEmpty(location))
//             {
//                 Log.Error("Asset name is invalid.");
//                 return null;
//             }
//
//             if (string.IsNullOrEmpty(location))
//             {
//                 throw new GameFrameworkException("Asset name is invalid.");
//             }
//
//             string assetObjectKey = GetCacheKey(location, packageName);
//             YooAssetObject assetObject = m_AssetPool.Spawn(assetObjectKey);
//             if (assetObject != null)
//             {
//                 return assetObject.Target as T;
//             }
//             
//             AssetHandle handle = GetHandleSync<T>(location, packageName: packageName);
//
//             T ret = handle.AssetObject as T;
//                 
//             assetObject = YooAssetObject.Create(assetObjectKey, handle.AssetObject, handle,this);
//             m_AssetPool.Register(assetObject, true);
//
//             return ret;
//             
//             //return m_YooResourceManager.LoadAsset<T>(location, packageName);
//         }
//         /// <summary>
//         /// 获取资源定位地址的缓存Key。
//         /// </summary>
//         /// <param name="location">资源定位地址。</param>
//         /// <param name="packageName">资源包名称。</param>
//         /// <returns>资源定位地址的缓存Key。</returns>
//         private string GetCacheKey(string location, string packageName = "")
//         {
//             if (string.IsNullOrEmpty(packageName) || packageName.Equals(DefaultPackageName))
//             {
//                 return location;
//             }
//             return $"{packageName}/{location}";
//         }
//         
//
//         /// <summary>
//         /// 同步加载游戏物体并实例化。
//         /// </summary>
//         /// <param name="location">资源的定位地址。</param>
//         /// <param name="parent">资源实例父节点。</param>
//         /// <param name="packageName">指定资源包的名称。不传使用默认资源包。</param>
//         /// <returns>资源实例。</returns>
//         public GameObject LoadGameObject(string location, Transform parent = null, string packageName = "")
//         {
//             if (string.IsNullOrEmpty(location))
//             {
//                 Log.Error("Asset name is invalid.");
//                 return null;
//             }
//
//             if (string.IsNullOrEmpty(location))
//             {
//                 throw new GameFrameworkException("Asset name is invalid.");
//             }
//             
//             string assetObjectKey = GetCacheKey(location, packageName);
//             YooAssetObject assetObject = m_AssetPool.Spawn(assetObjectKey);
//             if (assetObject != null)
//             {
//                 return YooAssetsReference.Instantiate(assetObject.Target as GameObject, parent, this).gameObject;
//             }
//             
//             AssetHandle handle = GetHandleSync<GameObject>(location, packageName: packageName);
//
//             GameObject gameObject = YooAssetsReference.Instantiate(handle.AssetObject as GameObject, parent, this).gameObject;
//
//             assetObject = YooAssetObject.Create(assetObjectKey, handle.AssetObject, handle,this);
//             m_AssetPool.Register(assetObject, true);
//             
//             return gameObject;
//             
//             //return m_YooResourceManager.LoadGameObject(location, parent, packageName);
//         }
//         
//         /// <summary>
//         /// 获取同步资源句柄。
//         /// </summary>
//         /// <param name="location">资源定位地址。</param>
//         /// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
//         /// <typeparam name="T">资源类型。</typeparam>
//         /// <returns>资源句柄。</returns>
//         private AssetHandle GetHandleSync<T>(string location, string packageName = "") where T : UnityEngine.Object
//         {
//             return GetHandleSync(location,typeof(T), packageName);
//         }
//         
//         private AssetHandle GetHandleSync(string location, Type assetType, string packageName = "")
//         {
//             if (string.IsNullOrEmpty(packageName))
//             {
//                 return YooAssets.LoadAssetSync(location, assetType);
//             }
//
//             var package = YooAssets.GetPackage(packageName);
//             return package.LoadAssetSync(location, assetType);
//         }
//         
//         /// <summary>
//         /// 获取异步资源句柄。
//         /// </summary>
//         /// <param name="location">资源定位地址。</param>
//         /// <param name="packageName">指定资源包的名称。不传使用默认资源包</param>
//         /// <typeparam name="T">资源类型。</typeparam>
//         /// <returns>资源句柄。</returns>
//         private AssetHandle GetHandleAsync<T>(string location, string packageName = "") where T : UnityEngine.Object
//         {
//             return GetHandleAsync(location, typeof(T), packageName);
//         }
//         
//         private AssetHandle GetHandleAsync(string location, Type assetType, string packageName = "")
//         {
//             if (string.IsNullOrEmpty(packageName))
//             {
//                 return YooAssets.LoadAssetAsync(location, assetType);
//             }
//
//             var package = YooAssets.GetPackage(packageName);
//             return package.LoadAssetAsync(location, assetType);
//         }
//         
//         /// <summary>
//         /// 异步加载资源。
//         /// </summary>
//         /// <param name="location">资源的定位地址。</param>
//         /// <param name="callback">回调函数。</param>
//         /// <param name="customPackageName">指定资源包的名称。不传使用默认资源包</param>
//         /// <typeparam name="T">要加载资源的类型。</typeparam>
//         public void LoadAsset<T>(string location, Action<T> callback, string customPackageName = "") where T : UnityEngine.Object
//         {
//             if (string.IsNullOrEmpty(location))
//             {
//                 Log.Error("Asset name is invalid.");
//                 return;
//             }
//             
//             
//             if (string.IsNullOrEmpty(location))
//             {
//                 throw new GameFrameworkException("Asset name is invalid.");
//             }
//             
//             string assetObjectKey = GetCacheKey(location, customPackageName);
//             
//             //await TryWaitingLoading(assetObjectKey);
//
//             YooAssetObject assetObject = m_AssetPool.Spawn(assetObjectKey);
//             if (assetObject != null)
//             {
//                 callback?.Invoke(assetObject.Target as T);
//                 return;
//             }
//             
//             _assetLoadingList.Add(assetObjectKey);
//             
//             AssetHandle handle = GetHandleAsync<T>(location, customPackageName);
//
//             handle.Completed += assetHandle =>
//             {
//                 _assetLoadingList.Remove(assetObjectKey);
//                 
//                 if (assetHandle.AssetObject != null)
//                 {
//                     assetObject = YooAssetObject.Create(assetObjectKey, handle.AssetObject, handle,this);
//                     m_AssetPool.Register(assetObject, true);
//             
//                     callback?.Invoke(assetObject.Target as T);
//                 }
//                 else
//                 {
//                     callback?.Invoke(null);
//                 }
//             };
//             //m_YooResourceManager.LoadAsset<T>(location, callback, packageName: customPackageName);
//         }
//
//         public TObject[] LoadSubAssetsSync<TObject>(string location, string packageName = "") where TObject : Object
//         {
//             if (string.IsNullOrEmpty(location))
//             {
//                 throw new GameFrameworkException("Asset name is invalid.");
//             }
//             throw new NotImplementedException();
//         }
//
//         public Task<TObject[]> LoadSubAssetsAsync<TObject>(string location, string packageName = "") where TObject : Object
//         {
//             if (string.IsNullOrEmpty(location))
//             {
//                 throw new GameFrameworkException("Asset name is invalid.");
//             }
//             throw new NotImplementedException();
//         }
//
//         public async Task<T> LoadAssetAsync<T>(string location, string packageName = "") where T : Object
//         {
//             if (string.IsNullOrEmpty(location))
//             {
//                 throw new GameFrameworkException("Asset name is invalid.");
//             }
//             
//             string assetObjectKey = GetCacheKey(location, packageName);
//
//             //await TryWaitingLoading(assetObjectKey);
//             
//             YooAssetObject assetObject = m_AssetPool.Spawn(assetObjectKey);
//             if (assetObject != null)
//             {
//                 await Task.Yield();
//                 return assetObject.Target as T;
//             }
//             
//             _assetLoadingList.Add(assetObjectKey);
//  
//             AssetHandle handle = GetHandleAsync<T>(location, packageName: packageName);
//
//             // bool cancelOrFailed = await handle.ToUniTask().AttachExternalCancellation(cancellationToken).SuppressCancellationThrow();
//             //
//             // if (cancelOrFailed)
//             // {
//             //     _assetLoadingList.Remove(assetObjectKey);
//             //     return null;
//             // }
//             
//             assetObject = YooAssetObject.Create(assetObjectKey, handle.AssetObject, handle,this);
//             m_AssetPool.Register(assetObject, true);
//
//             _assetLoadingList.Remove(assetObjectKey);
//             
//             return handle.AssetObject as T;
//         }
//         
//
//         /// <summary>
//         /// 异步加载游戏物体并实例化。
//         /// </summary>
//         /// <param name="location">资源定位地址。</param>
//         /// <param name="parent">资源实例父节点。</param>
//         /// <param name="cancellationToken">取消操作Token。</param>
//         /// <param name="packageName">指定资源包的名称。不传使用默认资源包。</param>
//         /// <returns>异步游戏物体实例。</returns>
//         public void LoadGameObjectAsync(string location,Action<GameObject> onCompleted, Action onFail = null, Transform parent = null, 
//             string packageName = "")
//         {
//             if (string.IsNullOrEmpty(location))
//             {
//                 Log.Error("Asset name is invalid.");
//                 return;
//             }
//             
//             if (string.IsNullOrEmpty(location))
//             {
//                 throw new GameFrameworkException("Asset name is invalid.");
//             }
//             
//             string assetObjectKey = GetCacheKey(location, packageName);
//             
//             
//             YooAssetObject assetObject = m_AssetPool.Spawn(assetObjectKey);
//             if (assetObject != null)
//             {
//                 onCompleted?.Invoke(YooAssetsReference.Instantiate(assetObject.Target as GameObject, parent, this).gameObject);
//             }
//             
//             _assetLoadingList.Add(assetObjectKey);
//
//             AssetHandle handle = GetHandleAsync<GameObject>(location, packageName: packageName);
//
//             handle.Completed += assetHandle =>
//             {
//                 GameObject gameObject = YooAssetsReference.Instantiate(handle.AssetObject as GameObject, parent, this)
//                     .gameObject;
//
//                 assetObject = YooAssetObject.Create(assetObjectKey, handle.AssetObject, handle, this);
//                 m_AssetPool.Register(assetObject, true);
//
//                 _assetLoadingList.Remove(assetObjectKey);
//                 
//                 if(gameObject != null)
//                     onCompleted?.Invoke(gameObject);
//                 else
//                     onFail?.Invoke();
//             };
//             // bool cancelOrFailed = await handle.ToUniTask().AttachExternalCancellation(cancellationToken).SuppressCancellationThrow();
//             //
//             // if (cancelOrFailed)
//             // {
//             //     _assetLoadingList.Remove(assetObjectKey);
//             //     return null;
//             // }
//             
//             //return m_YooResourceManager.LoadGameObjectAsync(location, parent, cancellationToken, packageName);
//         }
//
//         #endregion
//
//         #region 卸载资源
//
//         /// <summary>
//         /// 卸载资源。
//         /// </summary>
//         /// <param name="asset">要卸载的资源。</param>
//         public void UnloadAsset(object asset)
//         {
//             if (asset == null)
//             {
//                 return;
//             }
//             
//             if (m_AssetPool != null)
//             {
//                 m_AssetPool.Unspawn(asset);
//             }
//             //m_YooResourceManager.UnloadAsset(asset);
//         }
//
//
//         #endregion
//
//         #region 释放资源
//
//         /// <summary>
//         /// 强制执行释放未被使用的资源。
//         /// </summary>
//         /// <param name="performGCCollect">是否使用垃圾回收。</param>
//         public void ForceUnloadUnusedAssets(bool performGCCollect)
//         {
//             m_ForceUnloadUnusedAssets = true;
//             if (performGCCollect)
//             {
//                 m_PerformGCCollect = true;
//             }
//         }
//
//         /// <summary>
//         /// 预订执行释放未被使用的资源。
//         /// </summary>
//         /// <param name="performGCCollect">是否使用垃圾回收。</param>
//         public void UnloadUnusedAssets(bool performGCCollect)
//         {
//             m_PreorderUnloadUnusedAssets = true;
//             if (performGCCollect)
//             {
//                 m_PerformGCCollect = true;
//             }
//         }
//         
//         #region 资源回收
//         public void UnloadUnusedAssets()
//         {
//             m_AssetPool.ReleaseAllUnused();
//             foreach (var package in PackageMap.Values)
//             {
//                 if (package is { InitializeStatus: EOperationStatus.Succeed })
//                 {
//                     package.UnloadUnusedAssets();
//                 }
//             }
//         }
//
//         public void ForceUnloadAllAssets()
//         {
// #if UNITY_WEBGL
//             Log.Warning($"WebGL not support invoke {nameof(ForceUnloadAllAssets)}");
// 			return;
// #else
//             
//             foreach (var package in PackageMap.Values)
//             {
//                 if (package is { InitializeStatus: EOperationStatus.Succeed })
//                 {
//                     package.ForceUnloadAllAssets();
//                 }
//             }
// #endif
//         }
//         #endregion
//         
//
//         private void Update()
//         {
//             m_LastUnloadUnusedAssetsOperationElapseSeconds += Time.unscaledDeltaTime;
//             if (m_AsyncOperation == null && (m_ForceUnloadUnusedAssets || m_LastUnloadUnusedAssetsOperationElapseSeconds >= m_MaxUnloadUnusedAssetsInterval ||
//                                              m_PreorderUnloadUnusedAssets && m_LastUnloadUnusedAssetsOperationElapseSeconds >= m_MinUnloadUnusedAssetsInterval))
//             {
//                 Log.Info("Unload unused assets...");
//                 m_ForceUnloadUnusedAssets = false;
//                 m_PreorderUnloadUnusedAssets = false;
//                 m_LastUnloadUnusedAssetsOperationElapseSeconds = 0f;
//                 m_AsyncOperation = Resources.UnloadUnusedAssets();
//                 if (m_UseSystemUnloadUnusedAssets)
//                 {
//                     UnloadUnusedAssets();
//                 }
//             }
//
//             if (m_AsyncOperation is { isDone: true })
//             {
//                 m_AsyncOperation = null;
//                 if (m_PerformGCCollect)
//                 {
//                     Log.Info("GC.Collect...");
//                     m_PerformGCCollect = false;
//                     GC.Collect();
//                 }
//             }
//         }
//
//         #endregion
//     }
// }