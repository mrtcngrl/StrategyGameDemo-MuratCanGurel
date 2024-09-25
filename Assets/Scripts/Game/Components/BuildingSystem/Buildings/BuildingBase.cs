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
        private List<Vector2Int> _placedGridPositions = new();
        public Vector2Int Size => Properties.Size;
        public int Health => _health;
        protected void Initialize(int health)
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

        public void OnDestroyed()
        {
            throw new NotImplementedException();
        }

 

        private void Demolish()
        {
            MonoPool.Instance.ReturnToPool(Properties.ProductName, gameObject);
        }

        public virtual void OnDrag(Vector2 candidatePosition)
        {
            transform.position = candidatePosition;
        }

        public void OnPlace(Vector3 placedPosition, List<Vector2Int> placedGridPositions)
        {
            transform.position = placedPosition;
            _placedGridPositions.Clear();
            _placedGridPositions = placedGridPositions.ToList();
        }

        public void IsPlaceable(bool key)
        {
            SurfaceRenderer.material.color = key ? Color.green : Color.red;
        }
    }
}