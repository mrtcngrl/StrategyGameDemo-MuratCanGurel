using Game.Components.GridSystem;
using UnityEngine;

namespace Game.Components.PathFindingSystem
{
    public class Node
    {
        public bool Walkable;
        public Vector3 WorldPosition;
        public int GCost;
        public int HCost;
        public Vector2Int GridXY;
        public Node Parent;
        public int FCost => GCost + HCost;
        public Node(bool walkable, Vector3 worldPosition, Vector2Int gridXY)
        {
            Walkable = walkable;
            WorldPosition = worldPosition;
            GridXY = gridXY;
        }

       
    }
}