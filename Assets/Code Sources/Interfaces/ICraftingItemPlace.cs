// Aleksey 061WR Pustohaylov [stilluswr@gmail.com]
// Last edit: 21-02-2014
using System;

namespace Assets.Interfaces
{
    internal interface ICraftingItemPlace
    {
        bool ContainsItem();
        void AddItem( string id );
        void RemoveItem( string id );

        event Action OnAddNewItemToCraftingSlot;
    }
}