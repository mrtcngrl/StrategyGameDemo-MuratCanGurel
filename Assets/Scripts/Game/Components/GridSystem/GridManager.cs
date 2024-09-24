using Game.Components.BuildingSystem;
using UnityEngine;

namespace Game.Components.GridSystem
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private int _width, _height;
        [SerializeField] private float _cellSize;
        [SerializeField] private GameObject _tile;
        [SerializeField] private BuildingPlacer _buildingPlacer;
        private Grid _grid;
        private void Start()
        {
            CreateGrid();
            _buildingPlacer.Initialize(_grid);
        }

        private void CreateGrid()
        {
            _grid = new Grid(_width, _height, _cellSize);
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var tile = Instantiate(_tile, Vector3.zero, Quaternion.identity, transform);
                    tile.transform.localPosition = _grid.GetWorldPosition(x, y);
                }
            }
        }
        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * _cellSize;
        }

    }
}