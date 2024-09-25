using System;
using System.Collections.Generic;
using System.Linq;
using Game.Components.BuildingSystem.Scriptable;
using Game.Components.Interface;
using Game.Pool;
using Game.UI.ProductionMenu.Scriptable;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Components.BuildingSystem.Buildings
{
    public class BuildingBase : MonoBehaviour, IHittable
    {
        [SerializeField] protected ProductionItem Properties;
        [SerializeField] private SpriteRenderer SurfaceRenderer;
        private int _health;
        protected List<Vector2Int> PlacedGridPositions = new();
        public Vector2Int Size => Properties.Size;
        public int Health => _health;
        
        public virtual void Initialize(int health)
        {
            _health = health;
        }
        
        public void OnHit(int damage)
        {
            _health = damage;
            if (_health <= 0)
            {
                Demolish();
            }
        }

        public virtual void OnDestroyed()
        {
            MonoPool.Instance.ReturnToPool(Properties.ProductName, gameObject);
        }

 

        private void Demolish()
        {
            MonoPool.Instance.ReturnToPool(Properties.ProductName, gameObject);
        }

        public virtual void OnDrag(Vector2 candidatePosition)
        {
            transform.position = candidatePosition;
        }

        public void OnPlace(Vector3 placedPosition, List<Vector2Int> placedGridPositions, int health)
        {
            transform.position = placedPosition;
            PlacedGridPositions.Clear();
            PlacedGridPositions = placedGridPositions.ToList();
            Initialize(health);
        }

        public void IsPlaceable(bool key)
        {
            SurfaceRenderer.material.color = key ? Color.green : Color.red;
        }
        public virtual  List<Vector2Int> GetBuildingGridPointList(Vector2Int center)
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
    }
}