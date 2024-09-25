using Game.Components.Interface;
using Game.UI.ProductionMenu.Interface;
using UnityEngine;

namespace Game.Components.SoldierSystem.Units
{
    public class SoldierBase : MonoBehaviour, IHittable
    {
        private int _health;
        public int Health => _health;

        public void Initialize(int health)
        {
            _health = health;
        }
        public void OnHit(int damage)
        {
            throw new System.NotImplementedException();
        }

        public void OnDestroyed()
        {
            throw new System.NotImplementedException();
        }
    }
}