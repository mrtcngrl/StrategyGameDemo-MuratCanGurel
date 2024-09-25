using System.Collections.Generic;
using UnityEngine;

namespace Game.Components.GridSystem
{
public class Grid
{
    private int width;
    private int height;
    private float cellSize;
    private int[,] gridArray;
    private Transform[,] IsOccupied;

    public Grid(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        gridArray = new int[width, height];
        IsOccupied = new Transform[width, height];
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }
    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPosition.x / cellSize);
        y = Mathf.FloorToInt(worldPosition.y / cellSize);
    }

    public void SetValue(int x, int y, Transform value)
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
            IsOccupied[x, y] = value;
        Debug.LogError($"{x},{y} => {value.name}");
    }
    public bool IsValidGridPosition(Vector3 worldPosition, out int x, out int y)
    {
        GetXY(worldPosition, out x, out y);
        return IsValidGridXY(x,y);
    }
    public bool IsValidGridXY(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    public bool IsGridPositionEmpty(int x , int y)
    {
        return IsOccupied[x, y] == null;
    }
}

}

