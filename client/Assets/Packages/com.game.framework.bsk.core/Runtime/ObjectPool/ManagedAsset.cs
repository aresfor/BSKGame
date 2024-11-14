//using CustomGamePlay;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
namespace Ars.Client
{
    /// <summary>
    /// 当出现一个父节点prefab销毁回pool的时候，他的子节点中存在后来挂载的子prefab也要对应的拆开回pool
    /// </summary>
    public class ManagedAsset : MonoBehaviour
    {
        private GameObjectPool m_Pool = null;

        public GameObjectPool Pool
        {
            get { return m_Pool; }
        }
        
        public virtual void Init()
        {
        }

        public virtual void ShutDown()
        {
        }

        private Action _onDespawnCallback;

        public Action OnDespawnCallback
        {
            get { return _onDespawnCallback; }
            set { _onDespawnCallback = value; }
        }

    }


}