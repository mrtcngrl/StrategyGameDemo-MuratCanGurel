using System.Collections.Generic;
using Game.UI.ProductionMenu.Interface;
using Scripts.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.InformationScreen
{
    public class InformationScreenController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _productNameText;
        [SerializeField] private Image _productImage;
        [SerializeField] private GameObject _contentParent;
        [SerializeField] private GameObject _productionsParent;
        [SerializeField] private List<Image> _products = new();
        private void Awake()
        {
            _contentParent.SetActive(false);
            _productionsParent.SetActive(false);
            GameConstants.OnProductSelected += OnProductSelected;
        }

        private void OnProductSelected(IProduct product)
        {
            _contentParent.SetActive(true);
            _productNameText.text = product.ProductName.AddSpacesToSentence(true);
            _productImage.sprite = product.Icon;
            int count = product.Products.Count;
            if (count == 0)
            {
                _productionsParent.SetActive(false);
                return;
            }
            _productionsParent.SetActive(true);
            for (int i = 0; i < _products.Count; i++)
            {
                if (count > i)
                {
                    _products[i].enabled = true;
                    _products[i].sprite = product.Products[i].Icon;
                }
                    
                else
                {
                    _products[i].enabled = false;
                }
            }
        }
    }
}