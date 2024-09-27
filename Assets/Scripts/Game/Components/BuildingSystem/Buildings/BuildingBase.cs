using System.Collections.Generic;
using System.Linq;
using Game.Components.GridSystem.Interface;
using Game.Components.GridSystem.Managers;
using Game.Components.Interface;
using Game.Pool;
using Game.UI.ProductionMenu.Scriptable;
using Scripts.Helpers;
using UnityEngine;

namespace Game.Components.BuildingSystem.Buildings
{
    public class BuildingBase : MonoBehaviour, IHittable, IGridObject
    {
        [SerializeField] protected ProductionItem Properties;
        [SerializeField] private SpriteRenderer _modelRenderer;
        [SerializeField] private SpriteRenderer _surfaceRenderer;
        [SerializeField] private ParticleSystem _hitParticle;
        private int _health;
        private bool _isPlaced;
        private Vector2Int _center;
        private MaterialPropertyBlock _propertyBlock;
        protected List<Vector2Int> PlacedGridPoints = new();
        public int Health => _health;
        
        #region IGridObject Properties

        public bool Available => _health >= 0 && _isPlaced;
        public Vector2Int Center => _center;
        public Vector2Int Size => Properties.Size;
        public Vector3 WorldPosition => transform.position;
        public List<Vector2Int> GridPoints => PlacedGridPoints;
        
        #endregion

        protected virtual void Start()
        {
            _modelRenderer.sprite = SpriteHelper.Instance.GetSprite(Properties.name);
            _surfaceRenderer.sprite = SpriteHelper.Instance.GetSprite(GameConstants.SurfaceSpriteName);
            _propertyBlock = new MaterialPropertyBlock();
            SetSurfaceColor(false);
        }

        protected virtual void Initialize(int health)
        {
            _isPlaced = true;
            _health = health;
        }
        
        public void OnHit(int damage)
        {
            _health -= damage;
            _hitParticle.Play();
            if (_health <= 0)
            {
                OnDestroyed();
            }
        }

        public virtual void OnDestroyed()
        {
            GridManager.Instance.ClearPoints(PlacedGridPoints);
            MonoPool.Instance.ReturnToPool(Properties.ProductName, gameObject);
        }
        

        public virtual void OnDrag(Vector2 candidatePosition)
        {
            transform.position = candidatePosition;
        }

        public void OnPlace(Vector2Int center, Vector3 placedPosition, List<Vector2Int> placedGridPoints, int health)
        {
            _center = center;
            transform.position = placedPosition;
            PlacedGridPoints.Clear();
            PlacedGridPoints = placedGridPoints.ToList();
            Initialize(health);
        }

        public void SetSurfaceColor(bool canPlace)
        {
            _surfaceRenderer.GetPropertyBlock(_propertyBlock);
            Color color = canPlace ? Color.green : Color.red;
            _propertyBlock.SetColor("_Color",color);
            _surfaceRenderer.SetPropertyBlock(_propertyBlock);
        }
        public virtual List<Vector2Int> GetBuildingGridPointList(Vector2Int center)
        {
            List<Vector2Int> gridPointList = new List<Vector2Int>();
            for (int i = 0; i < Size.x; i++)
            {
                for (int j = 0; j < Size.y; j++)
                {
                    gridPointList.Add(center + new Vector2Int(i, j));
                }
            }
            return gridPointList;
        }
        public void OnSelect()
        {
            if (Available)
            {
                GameConstants.OnProductSelected?.Invoke(Properties);
            }
        }
    }
}