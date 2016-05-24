// Aleksey 061WR Pustohaylov [stilluswr@gmail.com]
// Last edit: 21-02-2014
using System;
using Assets.Interfaces;

namespace Assets.Classes
{
    internal sealed class CraftingItemPlace : ICraftingItemPlace
    {
        #region Data
        //===============================================================================================[]
        private IItem _currentItem;
        //===============================================================================================[]
        #endregion




        #region ICraftingItemPlace
        //===============================================================================================[]
        public bool ContainsItem()
        {
            return _currentItem != null;
        }

        //-------------------------------------------------------------------------------------[]
        public void AddItem( string id )
        {
            if( !ContainsItem() ) {
                _currentItem = EssenceManagerHelper.FindItemById( id );
                if( OnAddNewItemToCraftingSlot != null )
                    OnAddNewItemToCraftingSlot();
            }
        }

        //-------------------------------------------------------------------------------------[]
        public void RemoveItem( string id )
        {
            if( _currentItem.Id == id )
                _currentItem = null;
        }

        //-------------------------------------------------------------------------------------[]
        public event Action OnAddNewItemToCraftingSlot;

        //===============================================================================================[]
        #endregion
    }
}