// Aleksey 061WR Pustohaylov [stilluswr@gmail.com]
// Last edit: 21-02-2014
using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Interfaces;

namespace Assets.Classes
{
    internal sealed class EssenceWorld : IEssenceWorld
    {
        #region Data
        //===============================================================================================[]
        private readonly List<Action> _listenersForCraftNewItemList = new List<Action>();
        private readonly List<ICraftingItemPlace> _craftingItemPlacesList = new List<ICraftingItemPlace>();
        //-------------------------------------------------------------------------------------[]
        private static int _nextItemId;
        //-------------------------------------------------------------------------------------[]
        private readonly List<IItem> _itemsList = new List<IItem>();
        //===============================================================================================[]
        #endregion




        #region IEssenceWorld
        //===============================================================================================[]
        public ICraftingItemPlace FirstCraftingItemPlace { get; private set; }
        public ICraftingItemPlace SecondCraftingItemPlace { get; private set; }
        public ICraftingItemPlace ThirdCraftingItemPlace { get; private set; }
        public ICraftingItemPlace FourthCraftingItemPlace { get; private set; }
        //-------------------------------------------------------------------------------------[]
        public ICraftingItemPlace ResultCraftingItemPlace { get; private set; }
        //-------------------------------------------------------------------------------------[]
        public IInventory Inventory { get; private set; }
        //-------------------------------------------------------------------------------------[]
        public IItem GetNewItem1X1()
        {
            return CreateNewItem( 1, 1 );
        }

        //-------------------------------------------------------------------------------------[]
        public IItem GetNewItem2X1()
        {
            return CreateNewItem( 2, 1 );
        }

        //-------------------------------------------------------------------------------------[]
        public IItem GetNewItem1X2()
        {
            return CreateNewItem( 1, 2 );
        }

        //-------------------------------------------------------------------------------------[]
        public IItem GetNewItem2X2()
        {
            return CreateNewItem( 2, 2 );
        }

        //-------------------------------------------------------------------------------------[]
        public IItem FindItemById( string id )
        {
            return _itemsList.First( t => t.Id == id );
        }

        //-------------------------------------------------------------------------------------[]
        public void AddListenerForCraftNewItem( Action listener )
        {
            _listenersForCraftNewItemList.Add( listener );
        }

        //===============================================================================================[]
        #endregion




        #region Constructor
        //===============================================================================================[]
        public EssenceWorld()
        {
            FirstCraftingItemPlace = new CraftingItemPlace();
            FirstCraftingItemPlace.OnAddNewItemToCraftingSlot += OnAddNewItemToCraftingSlot;
            SecondCraftingItemPlace = new CraftingItemPlace();
            SecondCraftingItemPlace.OnAddNewItemToCraftingSlot += OnAddNewItemToCraftingSlot;
            ThirdCraftingItemPlace = new CraftingItemPlace();
            ThirdCraftingItemPlace.OnAddNewItemToCraftingSlot += OnAddNewItemToCraftingSlot;
            FourthCraftingItemPlace = new CraftingItemPlace();
            FourthCraftingItemPlace.OnAddNewItemToCraftingSlot += OnAddNewItemToCraftingSlot;

            ResultCraftingItemPlace = new CraftingItemPlace();

            _craftingItemPlacesList.Add( FirstCraftingItemPlace );
            _craftingItemPlacesList.Add( SecondCraftingItemPlace );
            _craftingItemPlacesList.Add( ThirdCraftingItemPlace );
            _craftingItemPlacesList.Add( FourthCraftingItemPlace );

            Inventory = new Inventory( Constants.InventoryValues.Width, Constants.InventoryValues.Height );
        }

        //===============================================================================================[]
        #endregion




        #region Routines
        //===============================================================================================[]
        private void OnAddNewItemToCraftingSlot()
        {
            foreach( var craftingItemPlace in _craftingItemPlacesList ) {
                if( !craftingItemPlace.ContainsItem() )
                    return;
            }

            foreach( var listenerForCraftNewItem in _listenersForCraftNewItemList )
                listenerForCraftNewItem();
        }

        //-------------------------------------------------------------------------------------[]
        private IItem CreateNewItem(
            int width,
            int height )
        {
            var item = new Item( GetNextItemId(), width, height );
            _itemsList.Add( item );
            return item;
        }

        //-------------------------------------------------------------------------------------[]
        private static string GetNextItemId()
        {
            _nextItemId += 1;
            return _nextItemId.ToString();
        }

        //===============================================================================================[]
        #endregion
    }
}