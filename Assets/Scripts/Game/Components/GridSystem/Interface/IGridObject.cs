using UnityEngine;

namespace Game.Components.GridSystem.Interface
{
    public interface IGridObject
    {
        bool Available { get; }
        Vector2Int Size { get; }
        Vector3 WorldPosition { get; }
        void OnSelect();
    }

    public interface IMilitaryUnit : IGridObject
    {
        int Damage { get; }
        void MoveAndAttack(IGridObject gridObject);
    }
}