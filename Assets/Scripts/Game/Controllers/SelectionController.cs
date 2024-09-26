using Scripts.Helpers;
using Scripts.Helpers.InputSystem;
using UnityEngine;

namespace Game.Controllers
{
    public class SelectionController
    {
        private readonly InputHandler _inputHandler;
        private ISelectable _selectedItem;
        private readonly Camera _camera;

        public SelectionController()
        {
            Input.multiTouchEnabled = false;
            _inputHandler = InputHandler.Instance;
            _camera = Camera.main;
            Subscribe();
        }
        private void Subscribe()
        {
            ClearTouchEvents();
            _inputHandler.OnTouchBeginEvent += PressDown;
        }

        private void ClearTouchEvents()
        {
            _inputHandler.OnTouchBeginEvent = null;
            _inputHandler.OnTouchMovedEvent = null;
            _inputHandler.OnTouchEndedEvent = null;
        }
        
        ~SelectionController()
        {
            ClearTouchEvents();
        }

        private void PressDown(Touch touch)
        {
            if (touch.IsOverAnyUiElement() ) return;
            _selectedItem = GetSelectable();
           _selectedItem?.OnSelect();
        }
        
        
        private ISelectable GetSelectable()
        {
            _inputHandler.TryGetTouchPosition(0, out Vector2 touchPosition);
            Ray ray = _camera.ScreenPointToRay(touchPosition);
            if (!Physics.Raycast(ray, out var hit, 150f, GameConstants.Selectable))
                return null;
            var selectable = hit.transform.GetComponentInParent<ISelectable>();
            if (selectable != null)
                return selectable;
            return null;
        }
        
        private Vector3 MouseWorldPosition(Touch touch, Vector3 selectedItemPos)
        {
            Vector3 mouseScreenPos = touch.position;
            mouseScreenPos.z = _camera.WorldToScreenPoint(selectedItemPos).z;
            return _camera.ScreenToWorldPoint(mouseScreenPos);
        }

    }

    public interface ISelectable
    {
        bool Available { get;}
        void OnSelect();
    }
}
