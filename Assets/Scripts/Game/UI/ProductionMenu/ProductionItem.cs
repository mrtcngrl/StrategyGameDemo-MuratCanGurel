using Game.Components.BuildingSystem;
using Game.Components.BuildingSystem.Scriptable;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.ProductionMenu
{
    public class ProductionItem : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _buttonImage;
        [SerializeField] private BuildingProperties _buildingProperties;

        private void Awake()
        {
            //_button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            BuildingPlacer.Instance.TryToSpawnNewBuilding(_buildingProperties);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.A))
                OnButtonClicked();
        }
    }
}