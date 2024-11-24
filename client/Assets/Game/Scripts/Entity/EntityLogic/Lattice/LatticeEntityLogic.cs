using System;
using Game.Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;
using Random = UnityEngine.Random;
using Game.Core;
using GameFramework.Entity;
using Log = UnityGameFramework.Runtime.Log;

namespace Game.Client
{
    public partial class LatticeEntityLogic:GameEntityLogic
    {
        private LatticeEntityModel m_Model;
        private Sprite m_WhiteSprite;
        private BoxCollider m_BoxCollider;

        [SerializeField]
        private SpriteRenderer m_LatticeSpriteRenderer;
        [SerializeField]
        private SpriteRenderer m_HoverSpriteRenderer;

        public Color DefaultHoverColor = Color.white;
        public Color HoverColor = Color.red;
        public LatticeNode LatticeNode => m_Model.LatticeNode;
        public int Row => m_Model.X;
        public int Column => m_Model.Y;

        private IEntity m_CurrentAttachRoleEntity;
        

        
        public Sprite WhiteSprite
        {
            get
            {
                if (null == m_WhiteSprite)
                {
                    var texture = Texture2D.whiteTexture;
                    m_WhiteSprite = Sprite.Create(texture,
                        rect: new Rect(0, 0, texture.width, texture.width),
                        pivot: new Vector2(0.5f, 0.5f),
                        pixelsPerUnit: texture.width);
                }

                return m_WhiteSprite;
            }
        }
        
        
        public override void OnInit(object userData)
        {
            base.OnInit(userData);
            InitPointerHandler();
            m_Model = (LatticeEntityModel)userData;
            m_BoxCollider = gameObject.GetOrAddComponent<BoxCollider>();
            m_LatticeSpriteRenderer.sprite = WhiteSprite;
            float random = Random.Range(0.6f, 0.8f);
            m_LatticeSpriteRenderer.color = Color.HSVToRGB(0.5f, 0.3f, random);
            m_HoverSpriteRenderer.color = DefaultHoverColor;
        }

        protected override void CreateGameplayEntity()
        {
            base.CreateGameplayEntity();
            GameplayEntity = new LatticeGameplayEntity(Entity);
        }
        
        public override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_BoxCollider.size = new Vector3(m_Model.Width, m_Model.Height, 0.2f);
            // m_BoxCollider.isTrigger = true;
            // Physics.queriesHitTriggers = true;
            
            CachedTransform.position = m_Model.Position.ToVector3();
            CachedTransform.rotation = m_Model.Rotation.ToQuaternion();
            CachedTransform.localScale = Vector3.one;
            
            GameEntry.Entity.AttachEntity(Id, m_Model.BoardEntityId);
        }

        public override void OnHide(bool isShutdown, object userData)
        {
            m_CurrentAttachRoleEntity = null;
            base.OnHide(isShutdown, userData);
        }

        public override void OnAttached(IEntity childEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);
            //角色或者其他物体attach到这个棋子时
            m_CurrentAttachRoleEntity = childEntity;
        }


        public override void OnDetached(IEntity childEntity, object userData)
        {
            if(childEntity == m_CurrentAttachRoleEntity)
                m_CurrentAttachRoleEntity = null;
            base.OnDetached(childEntity, userData);
        }


        public override void OnRecycle()
        {
            RecyclePointerHandler();

            base.OnRecycle();
        }
    }
}