using System.Collections.Generic;
using Game.Components.BuildingSystem;
using Game.Components.GridSystem.PathFindingSystem;
using Game.Components.GridSystem.Visual;
using Scripts.Helpers;
using UnityEngine;
using Grid = Game.Components.GridSystem.Core.Grid;

namespace Game.Components.GridSystem.Managers
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance;
        [SerializeField] private int _width, _height;
        [SerializeField] private float _cellSize;
        //[SerializeField] private GameObject _tile;
        [SerializeField] private BuildingPlacer _buildingPlacer;
        [SerializeField] private PathFinding _pathFinding;
        [SerializeField] private GridVisualizer _gridVisualizer;
        private Grid _grid;
        public Grid Grid => _grid;
        private void Awake()
        {
            if (!object.ReferenceEquals(Instance, null) && !object.ReferenceEquals(Instance, this)) this.Destroy();
            else
            {
                Instance = this;
            }
            CreateGrid();
        }
        
        private void Start()
        {
            _buildingPlacer.Initialize(_grid);
            _pathFinding.Initialize(_grid);
            _gridVisualizer.Initialize(_width,_height, _cellSize);
        }
        

        private void CreateGrid()
        {
            _grid = new Grid(_width, _height, _cellSize);
            // for (int x = 0; x < _width; x++)
            // {
            //     for (int y = 0; y < _height; y++)
            //     {
            //         var tile = Instantiate(_tile, Vector3.zero, Quaternion.identity, transform);
            //         tile.transform.localPosition = _grid.GetWorldPosition(x, y);
            //     }
            // }
        }
        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * _cellSize;
        }
        public List<Node> path;

        private void OnDrawGizmos()
        {
            if (_grid != null)
            {
                foreach (var node in _grid.nodeArray)
                {
                    Gizmos.color = node.Walkable ? Color.white : Color.black;
                    if (path != null)
                    {
                        if (path.Contains(node))
                        {
                            Gizmos.color = Color.cyan;
                            Gizmos.DrawCube(node.WorldPosition,Vector2.one);
                        }
                    }
                    Gizmos.DrawCube(node.WorldPosition,Vector2.one);
                }
            }
        }

        public void ClearPoints(List<Vector2Int> gridPointList)
        {
            foreach (var vector2Int in gridPointList)
            {
                _grid.nodeArray[vector2Int.x, vector2Int.y].Walkable = true;
            }
        }
    }
}