using System;
using System.Collections.Generic;
using System.Linq;
using Game.Components.GridSystem;
using Scripts.Helpers;
using UnityEngine;
using Grid = Game.Components.GridSystem.Grid;

namespace Game.Components.PathFindingSystem
{
    public class PathFinding : MonoBehaviour
    {
        public static PathFinding Instance;
        private Grid _grid;
        private void Awake()
        {
            if (!object.ReferenceEquals(Instance, null) && !object.ReferenceEquals(Instance, this)) this.Destroy();
            else
            {
                Instance = this;
            }
        }

        public void Initialize(Grid grid)
        {
            _grid = grid;
        }

        public List<Node> FindPath(Vector3 startPosition, Vector3 targetPosition)
        {
            Node startNode = _grid.GetNodeByWorldPos(startPosition);
            Node targetNode = _grid.GetNodeByWorldPos(targetPosition);
            if (startNode == null || targetNode == null) return null;
            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);
            while (openSet.Count > 0)
            {
                Node node = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < node.FCost ||
                        openSet[i].FCost == node.FCost && openSet[i].HCost < node.HCost)
                    {
                        if(openSet[i].HCost < node.HCost)
                            node = openSet[i];
                    }
                }
                openSet.Remove(node);
                closedSet.Add(node);
                if (node == targetNode)
                {
                    return  RetracePath(startNode, targetNode);
                }
                
                foreach (var neighbour in _grid.GetNeighbours(node))
                {
                    if(!neighbour.Walkable || closedSet.Contains(neighbour))  continue;
                    int newCostToNeighbour = node.GCost + GetDistance(node, neighbour);
                    if (newCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                    {
                        neighbour.GCost = newCostToNeighbour;
                        neighbour.HCost = GetDistance(neighbour, targetNode);
                        neighbour.Parent = node;
                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }

            return null;
        }

        private List<Node> RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;
            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            path.Reverse();
            return path;
        }
        private int GetDistance(Node nodeA, Node nodeB)
        {
            int distX = Mathf.Abs(nodeA.GridXY.x - nodeB.GridXY.x);
            int distY = Mathf.Abs(nodeA.GridXY.y - nodeB.GridXY.y);
            if (distX > distY)
                return 14 * distY + 10 * (distX - distY);
            return 14 * distX + 10 * (distY - distX);
        }
    }
}