
using System;
using Game.Gameplay;
using GameFramework;
using UnityEngine;

//@TEMP:
#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityGameFramework.Runtime;

namespace Game.Client
{
    public class BuiltinDataComponent : GameFrameworkComponent
    {
        [SerializeField] private TextAsset m_GameplayTagTextAsset = null;
        protected override void Awake()
        {
            base.Awake();

        }

        private void Start()
        {
            if (m_GameplayTagTextAsset == null)
            {
                Log.Error("GameplayTag Text Asset is null, check");
                return;
            }
            GameplayTagTree gameplayTagTree = GameplayTagExtension.InitializeGameplayTag(m_GameplayTagTextAsset.text);
            //@TEMP:
            #if UNITY_EDITOR
            AssetDatabase.Refresh();
            #endif
            if (gameplayTagTree == null)
            {
                Log.Warning("Parse GameplayTag failure.");
                return;
            }
        }
    }
}
