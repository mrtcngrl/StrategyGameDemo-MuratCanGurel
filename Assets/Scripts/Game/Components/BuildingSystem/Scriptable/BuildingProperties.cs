using UnityEngine;

namespace Game.Components.BuildingSystem.Scriptable
{
    [CreateAssetMenu(fileName = "New Building", menuName = "Building", order = 0)]
    public class BuildingProperties : ScriptableObject
    {
        [SerializeField] private string _tag;
        [SerializeField] private Vector2Int _size;
        [SerializeField] private int _health;
        [SerializeField] private GameObject _prefab;

        public string Tag => _tag;
        public Vector2Int Size => _size;
        public int Health => _health;
        public GameObject Prefab => _prefab;
    }
}