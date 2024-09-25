using System.Collections.Generic;
using Game.UI.ProductionMenu.Interface;
using Game.UI.ProductionMenu.Scriptable;
using ProductionMenu;
using UnityEngine;

namespace Game.UI.ProductionMenu.Controller
{
    public class ProductionMenuController : MonoBehaviour
    {
        [SerializeField] private List<ProductionItem> _products = new();
        [SerializeField] private ProductButton _productButtonPrefab;
        [SerializeField] private RectTransform _menuPanel;
        private void Start()
        {
            foreach (var product in _products)
            {
                Instantiate(_productButtonPrefab, _menuPanel).Initialize(product);
            }
        }
    }
}