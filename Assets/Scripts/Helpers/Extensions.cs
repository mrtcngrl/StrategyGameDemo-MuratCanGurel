using UnityEngine;

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
    }
}