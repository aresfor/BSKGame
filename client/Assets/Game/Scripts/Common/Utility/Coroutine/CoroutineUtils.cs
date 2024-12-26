using System.Collections;
using Game.Core;
using UnityEngine;

namespace Game.Client
{
    public static class CoroutineUtils
    {
        #region 控制协程Coroutine
        private static GameObject _entity;
        private static MonoBehaviour _behaviour;

        private static void _MakeEntity()
        {
            if (_entity != null)
            {
                return;
            }

            _entity = new GameObject("[Unity.Utility]");
            _entity.SetActive(true);
#if UNITY_EDITOR
            if (Application.isPlaying)
#endif
            {
                Object.DontDestroyOnLoad(_entity);
            }

            UnityEngine.Assertions.Assert.IsFalse(_behaviour);
            _behaviour = _entity.AddComponent<MonoBehaviour>();
        }
        
            public static Coroutine StartCoroutine(string methodName)
            {
                if (string.IsNullOrEmpty(methodName))
                {
                    return null;
                }

                _MakeEntity();
                return _behaviour.StartCoroutine(methodName);
            }

            public static Coroutine StartCoroutine(IEnumerator routine)
            {
                if (routine == null)
                {
                    return null;
                }

                _MakeEntity();
                return _behaviour.StartCoroutine(routine);
            }

            public static Coroutine StartCoroutine(string methodName, object value)
            {
                if (string.IsNullOrEmpty(methodName))
                {
                    return null;
                }

                _MakeEntity();
                return _behaviour.StartCoroutine(methodName, value);
            }

            public static void StopCoroutine(string methodName)
            {
                if (string.IsNullOrEmpty(methodName))
                {
                    return;
                }

                if (_entity != null)
                {
                    _behaviour.StopCoroutine(methodName);
                }
            }

            public static void StopCoroutine(IEnumerator routine)
            {
                if (routine == null)
                {
                    return;
                }

                if (_entity != null)
                {
                    _behaviour.StopCoroutine(routine);
                }
            }

            public static void StopCoroutine(Coroutine routine)
            {
                if (routine == null)
                    return;

                if (_entity != null)
                {
                    _behaviour.StopCoroutine(routine);
                    routine = null;
                }
            }

            public static void StopAllCoroutines()
            {
                if (_entity != null)
                {
                    _behaviour.StopAllCoroutines();
                }
            }

            #endregion
    }
}