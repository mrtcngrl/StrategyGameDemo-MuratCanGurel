using System.Collections.Generic;
using System.Linq;
using Scripts.Helpers;
using UnityEngine;

namespace Game.Components.GridSystem.PathFindingSystem
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

        public List<Node> FindPath(Vector3 startPosition,Vector3 targetPosition , IGridObject targetGridObject = null)
        {
            Node startNode = _grid.GetNodeByWorldPos(startPosition);
            Node targetNode = _grid.GetNodeByWorldPos(targetPosition);
            if (!targetNode.Walkable && targetGridObject != null)
            {
                targetNode = GetNearestWalkableNodeAroundBuilding(startPosition,targetNode, targetGridObject.Size.x, targetGridObject.Size.y);
                if (targetNode == null)
                {
                    Debug.LogError("Binanın çevresinde yürünebilir bir komşu bulunamadı.");
                    return null; 
                }
            }
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
        
        private Node GetNearestWalkableNodeAroundBuilding(Vector3 startPosition, Node targetNode, int buildingWidth, int buildingHeight)
        {
            List<Node> surroundingNodes = new List<Node>();

            int startX = targetNode.GridXY.x - 1;
            int endX = targetNode.GridXY.x + buildingWidth;
            int startY = targetNode.GridXY.y - 1;
            int endY = targetNode.GridXY.y + buildingHeight;

            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    // Kenarları kontrol et
                    if (x == startX || x == endX || y == startY || y == endY)
                    {
                        Node node = _grid.GetNodeByXY(x, y);
                        if (node != null)
                        {
                            if (node.Walkable)
                            {
                                surroundingNodes.Add(node);
                            }
                        }
                    }
                }
            }

            Node bestNode = null;
            int shortestPathCost = int.MaxValue;

            foreach (var node in surroundingNodes)
            {
                List<Node> pathToNode = FindPath(startPosition, node.WorldPosition);

                if (pathToNode != null)
                {
                    int currentCost = pathToNode.Sum(n => n.FCost);
                    if (currentCost < shortestPathCost)
                    {
                        shortestPathCost = currentCost;
                        bestNode = node;
                    }
                }
            }
            return bestNode;
        }
    }
}