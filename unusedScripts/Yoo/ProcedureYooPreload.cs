﻿using System;
using System.Collections;
using System.Collections.Generic;
using Game.Core;
using GameFramework;
using GameFramework.Procedure;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;
using YooAsset;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Client
{
    /// <summary>
    /// 预加载流程
    /// </summary>
    public class ProcedureYooPreload : ProcedureBase
    {
        private float _progress = 0f;

        private readonly Dictionary<string, bool> _loadedFlag = new Dictionary<string, bool>();
        private readonly bool _needProLoad = true;

        /// <summary>
        /// 预加载回调。
        /// </summary>
        private LoadAssetCallbacks m_PreLoadAssetCallbacks;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
            m_PreLoadAssetCallbacks = new LoadAssetCallbacks(OnPreLoadAssetSuccess, OnPreLoadAssetFailure);
        }


        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            _loadedFlag.Clear();

            UILoadMgr.Show(UIDefine.UILoadUpdate, Utility.Text.Format(LoadText.Instance.Label_Load_Load_Progress, 0));

            TypeEventSystem.Global.Send(new FRefreshVersionEvent());
            //GameEvent.Send("UILoadUpdate.RefreshVersion");

            PreloadResources();
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            var totalCount = _loadedFlag.Count <= 0 ? 1 : _loadedFlag.Count;

            var loadCount = _loadedFlag.Count <= 0 ? 1 : 0;

            foreach (KeyValuePair<string, bool> loadedFlag in _loadedFlag)
            {
                if (!loadedFlag.Value)
                {
                    break;
                }
                else
                {
                    loadCount++;
                }
            }

            if (_loadedFlag.Count != 0)
            {
                UILoadMgr.Show(UIDefine.UILoadUpdate, Utility.Text.Format(LoadText.Instance.Label_Load_Load_Progress, (float)loadCount / totalCount * 100));
            }
            else
            {
                TypeEventSystem.Global.Send(_progress);
                //GameEvent.Send(StringId.StringToHash("DownProgress"), _progress);

                string progressStr = $"{_progress * 100:f1}";

                if (Mathf.Abs(_progress - 1f) < 0.001f)
                {
                    UILoadMgr.Show(UIDefine.UILoadUpdate, "加载完成");
                }
                else
                {
                    UILoadMgr.Show(UIDefine.UILoadUpdate, Utility.Text.Format(LoadText.Instance.Label_Load_Load_Progress, progressStr));
                }
            }

            if (loadCount < totalCount)
            {
                return;
            }

            ChangeState<ProcedureYooLoadAssembly>(procedureOwner);
        }


        public IEnumerator SmoothValue(float value, float duration, Action callback = null)
        {
            float time = 0f;
            while (time < duration)
            {
                time += Time.deltaTime;
                var result = Mathf.Lerp(0, value, time / duration);
                _progress = result;
                yield return new WaitForEndOfFrame();
            }

            _progress = value;
            callback?.Invoke();
        }

        private void PreloadResources()
        {
            // await SmoothValue(1f, 1.2f).ToUniTask(GameModule.Procedure);
            if (_needProLoad)
            {
                PreLoad();
            }
        }

        private void PreLoad()
        {
            if (GameEntry.YooResource.PlayMode == EPlayMode.EditorSimulateMode)
            {
                return;
            }

            string[] preLoadTags = SettingsUtils.GetPreLoadTags();
            AssetInfo[] assetInfos = GameEntry.YooResource.GetAssetInfos(preLoadTags);
            foreach (var assetInfo in assetInfos)
            {
                PreLoad(assetInfo.Address);
            }
        }

        private void PreLoad(string configName)
        {
            _loadedFlag.Add(configName, false);
            GameEntry.YooResource.LoadAssetAsync(configName, typeof(UnityEngine.Object), m_PreLoadAssetCallbacks, null);
        }

        private void OnPreLoadAssetFailure(string assetName, LoadResourceStatus status, string errormessage, object userdata)
        {
            Log.Warning("Can not preload asset from '{0}' with error message '{1}'.", assetName, errormessage);
            _loadedFlag[assetName] = true;
        }

        private void OnPreLoadAssetSuccess(string assetName, object asset, float duration, object userdata)
        {
            Log.Debug("Success preload asset from '{0}' duration '{1}'.", assetName, duration);
            _loadedFlag[assetName] = true;
        }
    }
}