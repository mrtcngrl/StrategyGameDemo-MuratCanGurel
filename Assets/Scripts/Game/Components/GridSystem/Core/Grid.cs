using System.Collections.Generic;
using Game.Components.GridSystem.PathFindingSystem;
using UnityEngine;

namespace Game.Components.GridSystem.Core
{
public class Grid
{
    private int _width;
    private int _height;
    private float _cellSize;
    public Node[,] nodeArray;
    public Grid(int width, int height, float cellSize)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        CreateNodes();
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * _cellSize;
    }
    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPosition.x / _cellSize);
        y = Mathf.FloorToInt(worldPosition.y / _cellSize);
    }

    public void SetValue(int x, int y, bool walkable)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
        {
            nodeArray[x, y].Walkable = walkable;
        }
           
    }
    public bool IsValidGridPosition(Vector3 worldPosition, out int x, out int y)
    {
        GetXY(worldPosition, out x, out y);
        return IsValidGridXY(x,y);
    }
    public bool IsValidGridXY(int x, int y)
    {
        return x >= 0 && y >= 0 && x < _width && y < _height;
    }

    public bool IsGridPositionEmpty(int x , int y)
    {
        return nodeArray[x, y].Walkable;
    }

    public Node GetNodeByWorldPos(Vector3 worldPosition)
    {
        GetXY(worldPosition, out int x, out int y);
        return IsValidGridXY(x, y) ? nodeArray[x, y] : null;
    }

    public Node GetNodeByXY(int x, int y)
    {
        return IsValidGridXY(x, y) ? nodeArray[x, y] : null;
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1 ; y++)
            {
                if(x == 0 && y == 0)
                    continue;
                Vector2Int checkXY = new Vector2Int(node.GridXY.x + x, node.GridXY.y + y);
                if (IsValidGridXY(checkXY.x, checkXY.y))
                {
                    neighbours.Add(nodeArray[checkXY.x,checkXY.y]);
                }
            }
        }

        return neighbours;
    }
    
    public void CreateNodes()
    {
        nodeArray = new Node[_width, _height];
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Vector3 worldPosition = GetWorldPosition(x, y);
                nodeArray[x, y] = new Node(true, worldPosition, new Vector2Int(x,y));
            }
        }
    }
}

}

