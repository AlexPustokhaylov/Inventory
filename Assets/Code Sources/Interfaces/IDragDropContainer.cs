// Aleksey 061WR Pustohaylov [stilluswr@gmail.com]
// Last edit: 21-02-2014
using UnityEngine;

namespace Assets.Interfaces
{
    internal interface IDragDropContainer
    {
        bool CanAddItemToContainer();
        bool WasItemAdded();
        void RemoveItem( string id );

        Vector2 NewItemFinalPos { get; }
    }
}