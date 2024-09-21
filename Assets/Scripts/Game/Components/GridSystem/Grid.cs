using UnityEngine;

namespace Game.Components.GridSystem
{
public class Grid
{
    private int width;
    private int height;
    private float cellSize;
    public int[,] gridArray;

    public Grid(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        gridArray = new int[width, height];
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    public void SetValue(int x, int y, int value)
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
            gridArray[x, y] = value;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPosition.x / cellSize);
        y = Mathf.FloorToInt(worldPosition.y / cellSize);
    }
    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

}
}

