using System.Collections.Generic;
using System.Linq;
using Game.Components.GridSystem;
using Game.Components.Interface;
using Game.Pool;
using Game.UI.ProductionMenu.Scriptable;
using UnityEngine;

namespace Game.Components.BuildingSystem.Buildings
{
    public class BuildingBase : MonoBehaviour, IHittable, IGridObject
    {
        [SerializeField] protected ProductionItem Properties;
        [SerializeField] private SpriteRenderer SurfaceRenderer;
        private int _health;
        private bool _isPlaced;
        private Vector2Int _center;
        protected List<Vector2Int> PlacedGridPoints = new();
        public int Health => _health;
        
        #region IGridObject Properties

        public bool Available => _health >= 0 && _isPlaced;
        public Vector2Int Center => _center;
        public Vector2Int Size => Properties.Size;
        public Vector3 WorldPosition => transform.position;
        public List<Vector2Int> GridPoints => PlacedGridPoints;
        
        #endregion
        
        protected virtual void Initialize(int health)
        {
            _isPlaced = true;
            _health = health;
        }
        
        public void OnHit(int damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                OnDestroyed();
            }
        }

        public virtual void OnDestroyed()
        {
            Debug.Log("Yıkıldım");
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

        public void SetSurfaceColor(bool key)
        {
            SurfaceRenderer.material.color = key ? Color.green : Color.red;
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