
using UnityEngine;

namespace Game.Core
{
    public class BaseMonoBehaviour : MonoBehaviour
    {
        #region Unity native component and property accessors

        protected Transform m_Transform;

        public Transform CachedTransform
        {
            get
            {
                if (m_Transform == null)
                {
                    m_Transform = transform;
                }

                return m_Transform;
            }
        }

        public virtual Vector3 Position
        {
            get => CachedTransform.position;
            set => CachedTransform.position = value;
        }

        public virtual Quaternion Rotation
        {
            get => CachedTransform.rotation;
            set => CachedTransform.rotation = value;
        }

        public Vector3 LocalPosition
        {
            get => CachedTransform.localPosition;
            set => CachedTransform.localPosition = value;
        }

        public Quaternion LocalRotation
        {
            get => CachedTransform.localRotation;
            set => CachedTransform.localRotation = value;
        }

        public Vector3 Forward
        {
            get => CachedTransform.forward;
            set => CachedTransform.forward = value;
        }

        public Vector3 Right
        {
            get => CachedTransform.right;
            set => CachedTransform.right = value;
        }

        public Vector3 Up
        {
            get => CachedTransform.up;
            set => CachedTransform.up = value;
        }

        public Vector3 LocalScale
        {
            get => CachedTransform.localScale;
            set => CachedTransform.localScale = value;
        }

        #endregion
    }
}