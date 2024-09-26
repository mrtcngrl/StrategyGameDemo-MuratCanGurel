using System;
using UnityEngine;

namespace Scripts.Helpers.InputSystem
{
    public class InputProvider : InputHandler
    {
        private static bool _isTouchActive;
        private static Touch _activeTouch;

        protected override void SetTouches()
        {
            var touch = GetTouches();
            bool isActive = touch.phase != TouchPhase.Canceled;
            Touches = isActive ? new[] {_activeTouch} : Array.Empty<Touch>();
            TouchCount = isActive ? 1 : 0;
        }

        private static Touch GetTouches()
        {
            if (!_isTouchActive)
                _activeTouch = new Touch
                {
                    fingerId = 0,
                    phase = TouchPhase.Canceled
                };
            Vector2 mousePosition = Input.mousePosition.GetXYVector();

            if (!_isTouchActive && Input.GetMouseButtonDown(0))
            {
                OnMouseClick(mousePosition);
                return _activeTouch;
            }

            if (!_isTouchActive) return _activeTouch;
            if (Input.GetMouseButtonUp(0)) OnMouseUp(mousePosition);
            else if (Input.GetMouseButton(0)) OnMouseDown(mousePosition);
            return _activeTouch;
        }

        private static void OnMouseClick(Vector2 mousePosition)
        {
            _activeTouch.phase = TouchPhase.Began;
            _activeTouch.position = mousePosition;
            _activeTouch.deltaPosition = Vector2.zero;
            _isTouchActive = true;
        }

        private static void OnMouseDown(Vector2 mousePosition)
        {
            _activeTouch.deltaPosition = mousePosition - _activeTouch.position;
            _activeTouch.position = mousePosition;
            _activeTouch.phase = Mathf.Approximately(_activeTouch.deltaPosition.sqrMagnitude, 0)
                ? TouchPhase.Stationary
                : TouchPhase.Moved;
        }

        private static void OnMouseUp(Vector2 mousePosition)
        {
            _activeTouch.phase = TouchPhase.Ended;
            _activeTouch.position = mousePosition;
            _activeTouch.deltaPosition = Vector2.zero;
            _isTouchActive = false;
        }

   
    }
}