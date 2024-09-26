using UnityEngine;

namespace Game
{
    public static class GameConstants
    {
        public static LayerMask Selectable;
        public static void Initialize()
        {
            Selectable = 1 << LayerMask.NameToLayer("Selectable");
        }
    }
}