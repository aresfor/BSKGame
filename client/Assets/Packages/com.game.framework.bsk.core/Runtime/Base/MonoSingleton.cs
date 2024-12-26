using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Game.Core
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T _mInstance = null;
        private static bool m_HasInit = false;
        public static T Instance
        {
            get
            {
                if (!m_HasInit)
                {
                    if (_mInstance == null)
                    {
                        m_HasInit = true;
                        GameObject go = new GameObject(typeof(T).Name);
                        //_mInstance = go.AddComponent<T>();
                        _mInstance = go.AddComponent<T>();
                        _mInstance = go.GetComponent<T>();
                        GameObject parent = GameObject.Find("Boot");
                        if (parent == null)
                        {
                            parent = new GameObject("Boot");
                            DontDestroyOnLoad(parent);
                        }

                        if (parent != null)
                        {
                            go.transform.parent = parent.transform;
                        }
                        _mInstance = GameObject.FindObjectOfType(typeof(T)) as T;
                    }
                }
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                if (_mInstance == null)
                {
                    Debug.LogWarning($"Could not call {typeof(T).Name}.Instance but the instance has destroy! pay attention MonoBehaviour destroy order");
                }
                #endif
                return _mInstance;
            }
        }

        private void Awake()
        {
            if (_mInstance == null)
            {
                _mInstance = this as T;
            }

            DontDestroyOnLoad(gameObject);
            Init();
        }

        public void Startup()
        {
        }

        protected virtual void Init()
        {
        }

        public void Destroy()
        {
            Dispose();
            _mInstance = null;
            Destroy(gameObject);
        }

        protected abstract void Dispose();

        /// <summary>
        /// do not forget call base.OnDestroy()
        /// </summary>
        public virtual void OnDestroy()
        {
            _mInstance = null;
            Debug.LogWarning($"Destroy monosignleton {GetType()}");
        }
    }
}