using System;
using Game.Components.BuildingSystem.Scriptable;
using Game.Pool;
using UnityEngine;

namespace Game.Components.BuildingSystem.Buildings
{
    public class BuildingBase : MonoBehaviour
    {
        [SerializeField] private BuildingProperties _properties;
        public Vector2Int Size => _properties.Size;
        private int _health;

        protected void Initialize()
        {
            _health = _properties.Health;
        }

        protected void OnHit(int damage)
        {
            _health = damage;
            if (_health <= 0)
            {
                //todo make some particles
                Destroy(gameObject);
            }
        }

        public virtual void OnDrag(Vector2 candidatePosition)
        {
            transform.position = candidatePosition;
        }
        
    }
}