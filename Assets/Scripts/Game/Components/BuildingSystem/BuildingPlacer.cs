using System.Collections.Generic;
using Game.Components.BuildingSystem.Buildings;
using Game.Pool;
using Game.Signals.Helpers;
using Game.UI.ProductionMenu.Scriptable;
using Scripts.Helpers;
using UnityEngine;
using Grid = Game.Components.GridSystem.Core.Grid;

namespace Game.Components.BuildingSystem
{
    public class BuildingPlacer : MonoBehaviour
    {
        public static BuildingPlacer Instance;
        private Camera _camera;
        private Grid _grid;
        private BuildingBase _currentBuilding;
        private ProductionItem _currentBuildingProduct;
        private Vector2Int _buildingCenter;
        private List<Vector2Int> _buildingGridPoints = new();

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

        public void Initialize(Grid grid)
        {
            _grid = grid;
        }
        

        public void TryToSpawnNewBuilding(ProductionItem buildingProduct)
        {
            if (_currentBuilding != null)
            {
                ShowAlertNotifyHelper.ShowAlert(GameConstants.BuildingPlacementInProgressMessage);
                return;
            }

            _currentBuildingProduct = buildingProduct;
            _currentBuilding =
                MonoPool.Instance.SpawnObject<BuildingBase>(_currentBuildingProduct.ProductName, Vector3.zero,
                    Quaternion.identity);
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
                    _currentBuilding.SetSurfaceColor(CanPlace(mouseWorldPosition));
                }
                else
                {
                    _currentBuilding.SetSurfaceColor(false);
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                Vector2 mouseScreenPosition = Input.mousePosition;
                if (_currentBuilding == null || mouseScreenPosition.IsOverAnyUiElement()) return;
                Vector3 mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPosition.z = 0;
                TryToPlace(mouseWorldPosition);
            }
        }

        private void SetGridCenterAndGridPoints(int x, int y)
        {
            _buildingCenter = new Vector2Int(x, y);
            _buildingGridPoints = _currentBuilding.GetBuildingGridPointList(_buildingCenter);
        }

        private bool CanPlace(Vector3 mouseWorldPosition)
        {
            if (!_grid.IsValidGridPosition(mouseWorldPosition, out int x, out int y)) return false;
            SetGridCenterAndGridPoints(x, y);
            return IsPointsAvailable(_buildingGridPoints);
        }

        private void TryToPlace(Vector3 mouseWorldPosition)
        {
            if (!_grid.IsValidGridPosition(mouseWorldPosition, out int x, out int y))
            {
                ShowAlertNotifyHelper.ShowAlert(GameConstants.InvalidBuildLocationMessage);
                return;
            }

            List<Vector2Int> gridPointList = _currentBuilding.GetBuildingGridPointList(_buildingCenter);
            if (CanPlace(mouseWorldPosition))
            {
                PlaceBuilding(gridPointList, _buildingCenter);
            }
            else
            {
                ShowAlertNotifyHelper.ShowAlert(GameConstants.InvalidBuildLocationMessage);
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

        private void PlaceBuilding(List<Vector2Int> gridPositionList, Vector2Int center)
        {
            _currentBuilding.OnPlace(center, _grid.GetWorldPosition(center.x, center.y), gridPositionList,
                _currentBuildingProduct.Health);
            foreach (var vector2Int in gridPositionList)
            {
                _grid.SetValue(vector2Int.x, vector2Int.y, false);
            }

            _currentBuilding = null;
        }
    }
}