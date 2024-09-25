using System.Collections.Generic;
using System.Linq;
using Game.Components.GridSystem;
using Game.Components.SoldierSystem;
using Game.Components.SoldierSystem.Units;
using UnityEngine;

namespace Game.Components.BuildingSystem.Buildings
{
    public class Barracks : BuildingBase
    {
        private SoldierBase _currentSoldier;
        private Vector3 _soldierSpawnPosition;
        private Vector2Int _soldierSpawnPoint;
        
        public bool CanProduceSoldier => _currentSoldier == null;
        public Vector3 SoldierSpawnPosition => _soldierSpawnPosition;
        
        public override void Initialize(int health)
        {
            base.Initialize(health);
            SoldierPlacer.Instance.AddBarracks(this);
            _soldierSpawnPoint = PlacedGridPositions.Last();
            _soldierSpawnPosition = GridManager.Instance.GetWorldPosition(_soldierSpawnPoint.x, _soldierSpawnPoint.y);
        }

        public override List<Vector2Int> GetBuildingGridPointList(Vector2Int center)
        {
            List<Vector2Int> buildingGridPointList = base.GetBuildingGridPointList(center);
            buildingGridPointList.Add(new Vector2Int(center.x-1, center.y-1));
            return buildingGridPointList;
        }

        public override void OnDestroyed()
        {
            ClearSoldier();
            SoldierPlacer.Instance.RemoveBarracks(this);
            base.OnDestroyed();
        }

        public void SetSoldier(SoldierBase soldier)
        {
            _currentSoldier = soldier;
        }

        public void ClearSoldier()
        {
            _currentSoldier = null;
        }
    }
}