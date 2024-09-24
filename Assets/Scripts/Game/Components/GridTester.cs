using System;
using UnityEngine;
using UnityEngine.UI;
using Grid = Game.Components.GridSystem.Grid;

namespace Game.Components
{
    public class GridTester : MonoBehaviour
    {
        private Grid _grid;
        [SerializeField] private GameObject _tile;
        private void Start()
        {
            _grid = new Grid(10, 10, 100);
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    var tile = Instantiate(_tile, Vector3.zero, Quaternion.identity, transform);
                    tile.transform.localPosition = _grid.GetWorldPosition(x, y);
                }
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //var screenPosition = Input.mousePosition;
                
                // int x = Mathf.FloorToInt(screenPosition.x / 100);
                // int y = Mathf.FloorToInt(screenPosition.y / 100);
                //Debug.Log($"X : {x} Y : {y}");
            }
        }
    }
}

