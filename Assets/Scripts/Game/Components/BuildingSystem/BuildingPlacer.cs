using System;
using System.Collections.Generic;
using Game.Components.BuildingSystem.Buildings;
using Game.Components.BuildingSystem.Scriptable;
using Game.Pool;
using Scripts.Helpers;
using UnityEngine;
using UnityEngine.Serialization;
using Grid = Game.Components.GridSystem.Grid;

namespace Game.Components.BuildingSystem
{
    public class BuildingPlacer : MonoBehaviour
    {
        public static BuildingPlacer Instance;
        private Camera _camera;
        private Grid _grid;
        
        private BuildingBase _currentBuilding;
        private Vector2Int _currentBuildingSize;
        private Vector2Int _buildingCenter;
        private List<Vector2Int> _buildingGridPoints = new();
        

        public void Initialize(Grid grid)
        {
            _grid = grid;
        }

        private void Awake()
        {
            if (!object.ReferenceEquals(Instance, null) && !object.ReferenceEquals(Instance, this)) this.Destroy();
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        public void TryToSpawnNewBuilding(BuildingProperties buildingProperties)
        {
            if (_currentBuilding != null) return;
            _currentBuilding =
                MonoPool.Instance.SpawnObject<BuildingBase>(buildingProperties.Tag, Vector3.zero, Quaternion.identity);
            _currentBuildingSize = _currentBuilding.Size;
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (_currentBuilding == null) return;
                Vector3 mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPosition.z = 0;
                if (_grid.IsValidGridPosition(mouseWorldPosition, out int x, out int y))
                {
                    Vector2 candidatePosition = _grid.GetWorldPosition(x, y);
                    _currentBuilding.OnDrag(candidatePosition);
                    _currentBuilding.IsPlaceable(CanPlace(mouseWorldPosition));
                    //
                    // if (Input.GetMouseButtonDown(0))
                    // {
                    //     _grid.SetGridPosition(gridPosition, 1); // Mark the grid position as occupied
                    //     currentBuilding = null; // Reset currentBuilding so another can be placed
                    // }
                }
                else
                {
                    _currentBuilding.IsPlaceable(false);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (_currentBuilding == null) return;
                Vector3 mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPosition.z = 0;
                TryToPlace(mouseWorldPosition);
            }
        }

        private void SetGridCenterAndGridPoints(int x , int y)
        {
            _buildingCenter = new Vector2Int(x, y);
            _buildingGridPoints  = GetBuildingGridPointList(_buildingCenter);
        }
        private bool CanPlace(Vector3 mouseWorldPosition)
        {
            if (!_grid.IsValidGridPosition(mouseWorldPosition, out int x, out int y)) return false;
            SetGridCenterAndGridPoints(x,y);
            return IsPointsAvailable(_buildingGridPoints);
        }

        private void TryToPlace(Vector3 mouseWorldPosition)
        {
            if (!_grid.IsValidGridPosition(mouseWorldPosition, out int x, out int y)) return;
            Vector2Int center = new Vector2Int(x, y);
            List<Vector2Int> gridPointList = GetBuildingGridPointList(center);
            if (CanPlace(mouseWorldPosition))
            {
                PlaceBuilding(gridPointList, center);
            }
        }

        private bool IsPointsAvailable(List<Vector2Int> gridPointList)
        {
            bool canPlace = true;
            foreach (Vector2Int vector2Int in gridPointList)
            {
                if (_grid.IsValidGridXY(vector2Int.x, vector2Int.y) &&
                    _grid.IsGridPositionEmpty(vector2Int.x, vector2Int.y))
                {
                    continue;
                }
                canPlace = false;
                break;
            }

            return canPlace;
        }

        private List<Vector2Int> GetBuildingGridPointList(Vector2Int center)
        {
            List<Vector2Int> gridPointList = new List<Vector2Int>();
            for (int i = 0; i < _currentBuildingSize.x; i++)
            {
                for (int j = 0; j < _currentBuildingSize.y; j++)
                {
                    gridPointList.Add(center + new Vector2Int(i, j));
                }
            }

            return gridPointList;
        }

        private void PlaceBuilding(List<Vector2Int> gridPositionList, Vector2Int center)
        {
            _currentBuilding.OnPlace(_grid.GetWorldPosition(center.x, center.y), gridPositionList);
            foreach (var vector2Int in gridPositionList)
            {
                _grid.SetValue(vector2Int.x, vector2Int.y, _currentBuilding.transform);
            }

            _currentBuilding = null;
        }
    }
}