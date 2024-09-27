using System.Collections.Generic;
using System.Linq;
using Game.Components.BuildingSystem.Buildings;
using Game.Components.SoldierSystem.Units;
using Game.Pool;
using Game.Signals.Helpers;
using Game.UI.ProductionMenu.Scriptable;
using Scripts.Helpers;
using UnityEngine;

namespace Game.Components.SoldierSystem
{
    public class SoldierPlacer : MonoBehaviour
    {
        public static SoldierPlacer Instance;
        private List<Barracks> _placedBarracks = new();
        private void Awake()
        {
            if (!object.ReferenceEquals(Instance, null) && !object.ReferenceEquals(Instance, this)) this.Destroy();
            else
            {
                Instance = this;
            }
        }
        public void AddBarracks(Barracks barracks)
        {
            _placedBarracks.Add(barracks);
        }

        public void RemoveBarracks(Barracks barracks)
        {
            _placedBarracks.Remove(barracks);
        }

        public void TryToProduce(ProductionItem product)
        {
            Barracks availableBarrack = _placedBarracks.FirstOrDefault(b => b.CanProduceSoldier);
            if (availableBarrack != null)
            {
                 SoldierBase soldier = MonoPool.Instance.SpawnObject<SoldierBase>(product.ProductName, availableBarrack.SoldierSpawnPosition,
                    Quaternion.identity);
                 soldier.Initialize(product, availableBarrack);
            }
            else
            {
              ShowAlertNotifyHelper.ShowAlert(GameConstants.NoBarracksForUnitProductionMessage);
            }
        }
    }
}