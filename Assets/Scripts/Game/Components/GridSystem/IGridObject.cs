using System.Collections.Generic;
using UnityEngine;

namespace Game.Components.GridSystem
{
    public interface IGridObject
    {
        bool Available { get; }
        Vector2Int Center { get; }
        Vector2Int Size { get; }
        void GetNeighbours();
        void OnSelect();
        bool CanMove { get; }
    }

    public interface IMilitaryUnit : IGridObject
    {
        void Attack(IGridObject target);
    }
}