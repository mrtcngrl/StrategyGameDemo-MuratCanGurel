using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace Scripts.Helpers.InputSystem
{
    public class InputHandler
    {
        private static InputHandler _instance;
        public static InputHandler Instance => _instance ??= new InputProvider();
        public int TouchCount { get; protected set; }
        public Touch[] Touches { get; protected set; } = Array.Empty<Touch>();

        public bool TryGetTouch(int i, out Touch touch)
        {
            bool touchAvailable = TouchCount > i;
            touch = touchAvailable ? Touches[i] : default;
            return touchAvailable;
        }

        public bool TryGetTouchPosition(int i, out Vector2 touchPosition)
        {
            touchPosition = default;
            if (!TryGetTouch(i, out Touch touch)) return false;
            touchPosition = touch.position;
            return true;
        }
        
        private readonly IDisposable _update;


        #region Events
        public Action<Touch> OnTouchBeginEvent;
        public Action<Touch> OnTouchMovedEvent;
        public Action<Touch> OnTouchDownEvent;
        public Action<Touch> OnTouchStationaryEvent;
        public Action<Touch> OnTouchEndedEvent;
        public Action<Touch> OnTouchCanceled;
        #endregion

        protected InputHandler()
        {
            _update = Observable.EveryUpdate().Subscribe(_ => Update());
        }

        ~InputHandler()
        {
            _update.Dispose();
        }

        private void Update()
        {
            SetTouches();
            if (TouchCount == 0) return;
            TouchEvents();
        }

        protected virtual void SetTouches()
        {
            int previousCount = Touches.Length;
            TouchCount = Input.touchCount;
            if (TouchCount != previousCount) Touches = new Touch[TouchCount];
            for (int i = 0; i < TouchCount; i++) Touches[i] = Input.GetTouch(i);
            SortTouches();
        }

        private void SortTouches()
        {
            for (int i = 0; i < TouchCount - 1; i++)
            {
                for (int j = 0; j < TouchCount - i - 1; j++)
                {
                    if (Touches[j].phase > Touches[j + 1].phase)
                        (Touches[j], Touches[j + 1]) = (Touches[j + 1], Touches[j]);
                }
            }
        }

        private void TouchEvents()
        {
            for (int i = 0; i < TouchCount; i++)
            {
                Touch touch = Touches[i];

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        OnTouchBeginEvent?.Invoke(touch);
                        break;
                    case TouchPhase.Moved:
                        OnTouchDownEvent?.Invoke(touch);
                        OnTouchMovedEvent?.Invoke(touch);
                        break;
                    case TouchPhase.Stationary:
                        OnTouchDownEvent?.Invoke(touch);
                        OnTouchStationaryEvent?.Invoke(touch);
                        break;
                    case TouchPhase.Ended:
                        OnTouchEndedEvent?.Invoke(touch);
                        break;
                    case TouchPhase.Canceled:
                        OnTouchCanceled?.Invoke(touch);
                        break;
                }
            }
        }

       
    }
}