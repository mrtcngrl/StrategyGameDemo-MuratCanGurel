using Game.Components.BuildingSystem.Buildings;
using Game.Components.Interface;
using Game.UI.ProductionMenu.Interface;
using UnityEngine;

namespace Game.Components.SoldierSystem.Units
{
    public class SoldierBase : MonoBehaviour, IHittable
    {
        private int _health;
        public int Health => _health;
        private Barracks _currentBarracks;
        public void Initialize(int health, Barracks barracks)
        {
            _health = health;
            barracks.SetSoldier(this);
            
        }
        public void OnHit(int damage)
        {
            throw new System.NotImplementedException();
        }

        public void OnDestroyed()
        {
            _currentBarracks = null;
            throw new System.NotImplementedException();
        }
    }
}