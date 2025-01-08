// using GameFramework;
// using GameFramework.Resource;
//
// namespace Game.Client
// {
//     using System;
// using System.Collections.Generic;
// using UnityEngine;
// using Object = UnityEngine.Object;
//
// namespace Game.Client
// {
//     [Serializable]
//     public struct AssetsRefInfo
//     {
//         public int instanceId;
//
//         public Object refAsset;
//
//         public AssetsRefInfo(Object refAsset)
//         {
//             this.refAsset = refAsset;
//             instanceId = this.refAsset.GetInstanceID();
//         }
//     }
//
//     public sealed class YooAssetsReference : MonoBehaviour
//     {
//         [SerializeField] private GameObject _sourceGameObject;
//
//         [SerializeField] private List<AssetsRefInfo> _refAssetInfoList;
//
//         private YooResourceComponent resourceComponent;
//
//         private void OnDestroy()
//         {
//             if (resourceComponent == null)
//             {
//                 resourceComponent = UnityGameFramework.Runtime.GameEntry.GetComponent<YooResourceComponent>();
//             }
//
//             if (resourceComponent == null)
//             {
//                 throw new GameFrameworkException($"ResourceManager is null.");
//             }
//
//             if (_sourceGameObject != null)
//             {
//                 resourceComponent.UnloadAsset(_sourceGameObject);
//             }
//
//             if (_refAssetInfoList != null)
//             {
//                 foreach (var refInfo in _refAssetInfoList)
//                 {
//                     resourceComponent.UnloadAsset(refInfo.refAsset);
//                 }
//
//                 _refAssetInfoList.Clear();
//             }
//         }
//
//         public YooAssetsReference Ref(GameObject source, YooResourceComponent resourceManager = null)
//         {
//             if (source == null)
//             {
//                 throw new GameFrameworkException($"Source gameObject is null.");
//             }
//
//             if (source.scene.name != null)
//             {
//                 throw new GameFrameworkException($"Source gameObject is in scene.");
//             }
//
//             resourceComponent = resourceManager;
//             _sourceGameObject = source;
//             return this;
//         }
//
//         public YooAssetsReference Ref<T>(T source, YooResourceComponent resourceManager = null) where T : UnityEngine.Object
//         {
//             if (source == null)
//             {
//                 throw new GameFrameworkException($"Source gameObject is null.");
//             }
//
//             resourceComponent = resourceManager;
//             _refAssetInfoList = new List<AssetsRefInfo>();
//             _refAssetInfoList.Add(new AssetsRefInfo(source));
//             return this;
//         }
//
//         public static YooAssetsReference Instantiate(GameObject source, Transform parent = null, YooResourceComponent resourceManager = null)
//         {
//             if (source == null)
//             {
//                 throw new GameFrameworkException($"Source gameObject is null.");
//             }
//
//             if (source.scene.name != null)
//             {
//                 throw new GameFrameworkException($"Source gameObject is in scene.");
//             }
//
//             GameObject instance = Object.Instantiate(source, parent);
//             return instance.AddComponent<YooAssetsReference>().Ref(source, resourceManager);
//         }
//
//         public static YooAssetsReference Ref(GameObject source, GameObject instance, YooResourceComponent resourceManager = null)
//         {
//             if (source == null)
//             {
//                 throw new GameFrameworkException($"Source gameObject is null.");
//             }
//
//             if (source.scene.name != null)
//             {
//                 throw new GameFrameworkException($"Source gameObject is in scene.");
//             }
//
//             return instance.GetOrAddComponent<YooAssetsReference>().Ref(source, resourceManager);
//         }
//
//         public static YooAssetsReference Ref<T>(T source, GameObject instance, YooResourceComponent resourceManager = null) where T : UnityEngine.Object
//         {
//             if (source == null)
//             {
//                 throw new GameFrameworkException($"Source gameObject is null.");
//             }
//
//             return instance.GetOrAddComponent<YooAssetsReference>().Ref(source, resourceManager);
//         }
//     }
// }
// }