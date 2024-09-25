using UnityEngine;

namespace Game.Components.Interface
{
    public interface IHittable
    {
        int Health { get; } 
        void OnHit(int damage);
        void OnDestroyed();
    }
}