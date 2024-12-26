
using System.Collections.Generic;
using Game.Client.Build.Rutime;
using Game.Core;
using GameFramework;
using UnityEngine;

//@TEMP:

using UnityGameFramework.Runtime;
using Log = UnityGameFramework.Runtime.Log;

namespace Game.Client
{
    public class BuiltinDataComponent : GameFrameworkComponent
    {
        [SerializeField] private TextAsset m_GameplayTagTextAsset = null;
        public TextAsset GameplayTagTextAsset => m_GameplayTagTextAsset;
        
        [SerializeField] private TextAsset m_BuildInfoTextAsset = null;

        private BuildInfo m_BuildInfo = null;
        [SerializeField]
        private UIUpdateResourceForm m_UpdateResourceFormTemplate = null;
        public BuildInfo BuildInfo=> m_BuildInfo;
        public UIUpdateResourceForm UpdateResourceFormTemplate => m_UpdateResourceFormTemplate;
        
        protected override void Awake()
        {
            base.Awake();
        }

        public void InitBuildInfo()
        {
            if (m_BuildInfoTextAsset == null || string.IsNullOrEmpty(m_BuildInfoTextAsset.text))
            {
                Log.Info("Build info can not be found or empty.");
                return;
            }

            m_BuildInfo = Utility.Json.ToObject<BuildInfo>(m_BuildInfoTextAsset.text);
            if (m_BuildInfo == null)
            {
                Log.Warning("Parse build info failure.");
                return;
            }
        }
        
        private void Start()
        {
            
            
        }
        

        

    }
}
