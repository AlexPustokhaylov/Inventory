// Aleksey 061WR Pustohaylov [stilluswr@gmail.com]
// Last edit: 21-02-2014
using System;

namespace Assets.Interfaces
{
    internal interface IEssenceWorld
    {
        ICraftingItemPlace FirstCraftingItemPlace { get; }
        ICraftingItemPlace SecondCraftingItemPlace { get; }
        ICraftingItemPlace ThirdCraftingItemPlace { get; }
        ICraftingItemPlace FourthCraftingItemPlace { get; }
        //-------------------------------------------------------------------------------------[]
        ICraftingItemPlace ResultCraftingItemPlace { get; }
        //-------------------------------------------------------------------------------------[]
        IInventory Inventory { get; }
        //-------------------------------------------------------------------------------------[]
        IItem GetNewItem1X1();
        IItem GetNewItem2X1();
        IItem GetNewItem1X2();
        IItem GetNewItem2X2();
        //-------------------------------------------------------------------------------------[]
        IItem FindItemById( string id );
        //-------------------------------------------------------------------------------------[]
        void AddListenerForCraftNewItem( Action listener );
    }
}