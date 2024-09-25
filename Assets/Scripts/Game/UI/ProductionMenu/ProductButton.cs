using Game.UI.ProductionMenu.Interface;
using Game.UI.ProductionMenu.Scriptable;
using UnityEngine;
using UnityEngine.UI;

namespace ProductionMenu
{
    public class ProductButton : MonoBehaviour
    {
        private IProduct _product;
        [SerializeField] private Image _image;
        [SerializeField] private Button _button;

        public void Initialize(IProduct product)
        {
            _product = product;
            _image.sprite = product.Icon;
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            _product.TryProduce();
        }
        
    }
}

