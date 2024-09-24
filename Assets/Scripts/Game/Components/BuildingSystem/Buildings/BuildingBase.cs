using System;
using System.Collections.Generic;
using System.Linq;
using Game.Components.BuildingSystem.Scriptable;
using Game.Pool;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Components.BuildingSystem.Buildings
{
    public class BuildingBase : MonoBehaviour
    {
        [SerializeField] protected BuildingProperties Properties;
        [SerializeField] private SpriteRenderer SurfaceRenderer;
        private int _health;
        private List<Vector2Int> _placedGridPositions = new();
        public Vector2Int Size => Properties.Size;
        protected void Initialize()
        {
            _health = Properties.Health;
        }

        protected void OnHit(int damage)
        {
            _health = damage;
            if (_health <= 0)
            {
               Demolish();
            }
        }

        protected void Demolish()
        {
            MonoPool.Instance.ReturnToPool(Properties.Tag, gameObject);
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