using Game.Core;
using Game.Gameplay;
using UnityEngine;

namespace Game.Client
{
    public class LatticeEntityLogic:GameEntityLogic
    {
        private LatticeEntityModel m_Model;
        private Sprite m_WhiteSprite;
        private BoxCollider m_BoxCollider;

        public Lattice Lattice => m_Model.Lattice;
        public int Row => m_Model.X;
        public int Column => m_Model.Y;
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

            m_Model = (LatticeEntityModel)userData;
            m_BoxCollider = gameObject.GetOrAddComponent<BoxCollider>();
            var spriteRenderer = gameObject.GetOrAddComponent<SpriteRenderer>();
            spriteRenderer.sprite = WhiteSprite;
            float random = Random.Range(0.6f, 0.8f);
            spriteRenderer.color = Color.HSVToRGB(0.5f, 0.3f, random);
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
            CachedTransform.position = m_Model.Position.ToVector3();
            CachedTransform.rotation = m_Model.Rotation.ToQuaternion();
            CachedTransform.localScale = Vector3.one;
            
            
            GameEntry.Entity.AttachEntity(Id, m_Model.BoardEntityId);
        }
        
        
    }
}