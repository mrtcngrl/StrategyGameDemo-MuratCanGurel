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
        public GameObject buildingPrefab;
        [FormerlySerializedAs("currentBuilding")] [SerializeField] private BuildingBase _currentBuilding;
        private Camera _camera;
        private Grid _grid;
        
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
            if (_currentBuilding != null)
            {
                //todo show information message
                return;
            }
            _currentBuilding = MonoPool.Instance.SpawnObject<BuildingBase>(buildingProperties.Tag, Vector3.zero, Quaternion.identity);
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if(_currentBuilding == null) return;
                Vector3 mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPosition.z = 0;
                if (_grid.IsValidGridPosition(mouseWorldPosition, out int x , out int y))
                {
                    Vector2 placementPosition = _grid.GetWorldPosition(x, y);
                    _currentBuilding.OnDrag(placementPosition);
                    //
                    // if (Input.GetMouseButtonDown(0))
                    // {
                    //     _grid.SetGridPosition(gridPosition, 1); // Mark the grid position as occupied
                    //     currentBuilding = null; // Reset currentBuilding so another can be placed
                    // }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if(_currentBuilding == null) return;
                Vector3 mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPosition.z = 0;
                TryToPlace(mouseWorldPosition);
            }
        }

        private void TryToPlace(Vector3 mouseWorldPosition)
        {
            if(!_grid.IsValidGridPosition(mouseWorldPosition, out int x, out int y)) return;
            Vector2Int center = new Vector2Int(x,y);
            Vector2Int size = _currentBuilding.Size;
            List<Vector2Int> gridPositionList = new List<Vector2Int>();
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    gridPositionList.Add(center + new Vector2Int(i,j));
                }
            }

            bool canPlace = true;
            foreach (Vector2Int vector2Int in gridPositionList)
            {
                if (_grid.IsValidGridXY(vector2Int.x, vector2Int.y) &&
                    _grid.IsGridPositionEmpty(vector2Int.x, vector2Int.y))
                {
                    continue;
                }
                canPlace = false;
                break;
            }
            if (canPlace)
            {
                PlaceBuilding(gridPositionList,center);
            }
        }

        private void PlaceBuilding(List<Vector2Int> gridPositionList, Vector2Int center)
        {
            _currentBuilding.transform.position = _grid.GetWorldPosition(center.x, center.y);
            foreach (var vector2Int in gridPositionList)
            { 
                _grid.SetValue(vector2Int.x, vector2Int.y,_currentBuilding.transform);
            }
            _currentBuilding = null;
        }
    }
}