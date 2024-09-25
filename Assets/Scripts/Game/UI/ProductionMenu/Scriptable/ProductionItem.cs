using Game.Components.BuildingSystem;
using Game.Components.SoldierSystem;
using Game.UI.ProductionMenu.Interface;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.UI.ProductionMenu.Scriptable
{
    [CreateAssetMenu(fileName = "New Product", menuName = "Production Item", order = 0)]
    public class ProductionItem : ScriptableObject, IProduct
    {
        [SerializeField] private string _productName;
        [SerializeField] private Sprite icon;
        [SerializeField] private int _health;
        [SerializeField] private Vector2Int _size;
        [SerializeField] private bool isMilitaryUnit;

        // IProduct Arayüzü Uygulamaları
        public string ProductName => _productName;
        public Sprite Icon => icon;
        public int Health => _health;
        public Vector2Int Size => _size;
        public bool IsMilitaryUnit => isMilitaryUnit;
        public void TryProduce()
        {
            if (isMilitaryUnit)
            {
                SoldierPlacer.Instance.TryToProduce(this);
            }
            else
            {
                BuildingPlacer.Instance.TryToSpawnNewBuilding(this);
            }
        }
    }
}