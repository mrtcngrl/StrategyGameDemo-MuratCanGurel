using UnityEngine;

namespace Game.UI.ProductionMenu.Interface
{
    public interface IProduct
    {
        string ProductName { get; }
        int Health { get; }
        Sprite Icon { get; }
        Vector2Int Size { get; }
        bool IsMilitaryUnit { get; }
        void TryProduce();
    }
}