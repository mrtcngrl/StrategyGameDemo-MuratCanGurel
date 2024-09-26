using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        
        public static string AddSpacesToSentence(this string text, bool preserveAcronyms)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                newText.Append(text[i]);
            }

            return newText.ToString();
        }
    }
}