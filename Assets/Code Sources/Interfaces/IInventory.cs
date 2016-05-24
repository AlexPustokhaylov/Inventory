// Aleksey 061WR Pustohaylov [stilluswr@gmail.com]
// Last edit: 21-02-2014
using System;
using System.Collections.Generic;
using Assets.Classes;

namespace Assets.Interfaces
{
    internal interface IInventory
    {
        bool CanAddItemToInventory(
            string id,
            IEnumerable<InventoryIndex> cellIndexes );

        void AddItem(
            string id,
            IEnumerable<InventoryIndex> cellIndexes );

        void RemoveItem( string id );

        List<IItem> Items { get; }

        List<InventoryCell> GetItemCells( string itemId );
        List<InventoryCell> ItemCellsAfterSort( string itemId );

        void SortItems();

        event Action OnSortItems;
    }
}