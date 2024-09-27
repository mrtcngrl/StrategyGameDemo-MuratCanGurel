using System;
using Game.Components.Interface;
using Game.UI.ProductionMenu.Interface;
using UnityEngine;

namespace Game
{
    public static class GameConstants
    {
        public static readonly float AttackInterval = .5f;
        public static readonly string SurfaceSpriteName = "Surface";
        public static LayerMask Selectable;
        public static Action<IProduct> OnProductSelected;
        public static Action<IHittable> OnUnitDestroyed;
        public static void Initialize()
        {
            Debug.LogError("Test");
            Selectable = 1 << LayerMask.NameToLayer("Selectable");
        }
    }
}