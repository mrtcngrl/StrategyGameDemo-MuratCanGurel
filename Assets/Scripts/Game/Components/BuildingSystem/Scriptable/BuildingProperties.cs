using Game.UI.ProductionMenu.Interface;
using UnityEngine;

namespace Game.Components.BuildingSystem.Scriptable
{
    [CreateAssetMenu(fileName = "New Building", menuName = "Building", order = 0)]
    public class BuildingProperties : ScriptableObject, IProduct
    {
        [SerializeField] private string _tag;
        [SerializeField] private Vector2Int _size;
        [SerializeField] private int _health;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Sprite _icon;
        public string ProductName  => _tag;
        public Sprite Icon { get; }
        public Vector2Int Size => _size;
        public bool IsMilitaryUnit => false;
        public void TryProduce()
        {
        }

        public int Health => _health;
        public GameObject Prefab => _prefab;
        public Sprite ProductionIcon => _icon;
    }
}