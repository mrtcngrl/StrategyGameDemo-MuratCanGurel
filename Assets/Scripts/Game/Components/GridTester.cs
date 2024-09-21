using System;
using UnityEngine;
using Grid = Game.Components.GridSystem.Grid;

namespace Game.Components
{
    public class GridTester : MonoBehaviour
    {
        private Grid _grid;
        [SerializeField] private GameObject _tile;
        private void Start()
        {
            _grid = new Grid(10, 10, 1);
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    Instantiate(_tile, _grid.GetWorldPosition(x, y), Quaternion.identity);
                }
            }
        }
    }
}

