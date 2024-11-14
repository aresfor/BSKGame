using Game.Core;
using UnityEngine;

namespace Game.Client
{
    public struct FLattice
    {
        public MonoLattice Lattice;
        public bool IsValid => Lattice != null && Lattice.gameObject.activeSelf;
    }
    public class MonoBoard: BaseMonoBehaviour
    {
        public GameObject Prefab;

        [Range(1, 10)]
        public int Row = 1;
        [Range(1, 10)]
        public int Column = 1;

        [Min(1)] public float BoardWidth = 1;
        [Min(1)] public float BoardHeight = 1;

        public float LatticeWidth = 1;
        public float LatticeHeight = 1;
        
        private FLattice[,] mBoard;
        private Sprite mWhiteSprite;

        public Sprite WhiteSprite
        {
            get
            {
                if (null == mWhiteSprite)
                {
                    var texture = Texture2D.whiteTexture;
                    mWhiteSprite = Sprite.Create(texture,
                        rect: new Rect(0, 0, texture.width, texture.width),
                        pivot: new Vector2(0.5f, 0.5f),
                        pixelsPerUnit: texture.width);
                }

                return mWhiteSprite;
            }
        }

        public void ClearBoard()
        {
            if (mBoard != null)
            {
                //@TODO: pool
                foreach (var fLattice in mBoard)
                {
                    if (fLattice.IsValid)
                    {
                        #if UNITY_EDITOR
                        DestroyImmediate(fLattice.Lattice.gameObject);
                        #else
                        Destroy(fLattice.Lattice.gameObject);
                        #endif
                    }
                }
            }
            else
            {
                for (int i = CachedTransform.childCount-1; i >= 0; --i)
                {
                    var t =CachedTransform.GetChild(i);
#if UNITY_EDITOR
                    DestroyImmediate(t.gameObject);
#else
                        Destroy(t.gameObject);
#endif
                }
            }
        }
        public void Generate()
        {
            ClearBoard();
            
            
            mBoard = new FLattice[Row,Column];
            var perHeight = LatticeHeight;
            var perWidth = LatticeWidth;

            float rowPositionX = LatticeWidth; 
            for (int i = 0; i < Row; ++i)
            {
                float positionZ = i * perHeight + LatticeHeight;
                for (int j = 0; j < Column; ++j)
                {
                    float positionX = j * perWidth + rowPositionX;
                    var lattice = GetLattice();
                    lattice.Initialize(new Vector3(positionX, CachedTransform.position.y, positionZ)
                        , CachedTransform.rotation, CachedTransform.localScale, CachedTransform);
                    
                    mBoard[i, j] = new FLattice()
                    {
                        Lattice = lattice
                    };
                }
            }
            
            
        }


        public MonoLattice GetLattice()
        {
            MonoLattice lattice = null;
            if (false /*pool*/)
            {
                
            }
            else
            {
                GameObject go;
                if (Prefab != null)
                {
                    go = Instantiate(Prefab);
                    lattice = go.GetOrAddComponent<MonoLattice>();
                }
                else
                {
                    go = new GameObject();
                    lattice = go.AddComponent<MonoLattice>();
                }

                var spriteRenderer = go.GetOrAddComponent<SpriteRenderer>();
                spriteRenderer.sprite = WhiteSprite;
                float random = Random.Range(0.6f, 0.8f);
                spriteRenderer.color = Color.HSVToRGB(0.5f , 0.3f, random);
                
            }
            lattice.Initialize(Vector3.zero,Quaternion.identity, Vector3.one);
            return lattice;
        }
    }
}