// Aleksey 061WR Pustohaylov [stilluswr@gmail.com]
// Last edit: 21-02-2014
using UnityEngine;

namespace Assets.Classes
{
    internal sealed class InventoryGuiCell
    {
        internal Vector2 Center { get; private set; }
        internal InventoryIndex IndexInInventory { get; private set; }

        internal InventoryGuiCell(
            Vector2 center,
            int rowCoord,
            int colCoord )
        {
            Center = center;
            IndexInInventory = new InventoryIndex( rowCoord, colCoord );
        }
    }
}