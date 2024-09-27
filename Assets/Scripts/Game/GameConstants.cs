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
            Selectable = 1 << LayerMask.NameToLayer("Selectable");
        }
        public static readonly string InvalidBuildLocationMessage = "Invalid build location! Please choose a valid spot on the map";
        public static readonly string NoBarracksForUnitProductionMessage = "No suitable barracks available for unit production!";
        public static readonly string BuildingPlacementInProgressMessage = "You cannot place a new building while another is being placed!";
        public static readonly string NoValidPathForUnitMessage = "No valid path found for the unit!";
    }
}