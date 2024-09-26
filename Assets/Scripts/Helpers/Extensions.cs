using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.Helpers
{
    public static class Extensions
    {
        public static void Destroy(this Object @object)
        {
#if UNITY_EDITOR
            Object.DestroyImmediate(@object is Component component ? component.gameObject : @object);
#else
            Object.Destroy(@object is Component component ? component.gameObject : @object);
#endif
        }
        
        public static Vector2 GetXYVector(this Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.y);
        }
        
        
        public static bool IsOverAnyUiElement(this Touch touch, int mask = -1) =>
            IsOverAnyUiElement(touch.position, mask);
        
        public static bool IsOverAnyUiElement(this Vector2 screenPosition, int mask = -1)
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = screenPosition;
            List<RaycastResult> hitElements = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, hitElements);
            if (mask == -1) return hitElements.Any(x => x.module is GraphicRaycaster);

            foreach (var hit in hitElements)
            {
                if (!(hit.module is GraphicRaycaster)) continue;
                if ((1 << hit.gameObject.layer & mask) != 0) continue;
                return true;
            }

            return false;
        }
    }
}