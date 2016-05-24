// Aleksey 061WR Pustohaylov [stilluswr@gmail.com]
// Last edit: 21-02-2014
using System;
using System.Collections.Generic;
using Assets.Interfaces;
using Assets.Scripts;

namespace Assets.Classes
{
    internal static class EssenceManagerHelper
    {
        #region Private data
        //===============================================================================================[]
        private static ComponentEssenceManagerComponent _essenceManager;
        private static ComponentEssenceManagerComponent EssenceManager { get { return GetEssenceManager(); } }
        //-------------------------------------------------------------------------------------[]
        private static IEssenceWorld EssenceWorldInstance { get { return EssenceManager.EssenceWorldInstance; } }
        //-------------------------------------------------------------------------------------[]
        private static int _nextItemId;
        //-------------------------------------------------------------------------------------[]
        private static List<ICraftingItemPlace> _craftingItemPlaces = new List<ICraftingItemPlace>();
        //===============================================================================================[]
        #endregion




        #region Constructor
        //===============================================================================================[]
        static EssenceManagerHelper()
        {
            EssenceWorldInstance.AddListenerForCraftNewItem( OnNeedCraftNewItem );

            _craftingItemPlaces.Add( FirstCraftingItemPlace );
            _craftingItemPlaces.Add( SecondCraftingItemPlace );
            _craftingItemPlaces.Add( ThirdCraftingItemPlace );
            _craftingItemPlaces.Add( FourthCraftingItemPlace );
            _craftingItemPlaces.Add( ResultCraftingItemPlace );
        }

        //===============================================================================================[]
        #endregion




        #region Public methods
        //===============================================================================================[]
        internal static ICraftingItemPlace FirstCraftingItemPlace { get { return EssenceWorldInstance.FirstCraftingItemPlace; } }
        //-------------------------------------------------------------------------------------[]
        internal static ICraftingItemPlace SecondCraftingItemPlace
        {
            get { return EssenceWorldInstance.SecondCraftingItemPlace; }
        }

        //-------------------------------------------------------------------------------------[]
        internal static ICraftingItemPlace ThirdCraftingItemPlace { get { return EssenceWorldInstance.ThirdCraftingItemPlace; } }

        //-------------------------------------------------------------------------------------[]
        internal static ICraftingItemPlace FourthCraftingItemPlace
        {
            get { return EssenceWorldInstance.FourthCraftingItemPlace; }
        }

        //-------------------------------------------------------------------------------------[]
        internal static ICraftingItemPlace ResultCraftingItemPlace
        {
            get { return EssenceWorldInstance.ResultCraftingItemPlace; }
        }

        //-------------------------------------------------------------------------------------[]
        internal static List<ICraftingItemPlace> CraftingItemPlaces { get { return _craftingItemPlaces; } }

        //-------------------------------------------------------------------------------------[]
        internal static IInventory Inventory { get { return EssenceWorldInstance.Inventory; } }
        //-------------------------------------------------------------------------------------[]
        internal static IItem GetNewItem1X1()
        {
            return EssenceWorldInstance.GetNewItem1X1();
        }

        //-------------------------------------------------------------------------------------[]
        internal static IItem GetNewItem2X1()
        {
            return EssenceWorldInstance.GetNewItem2X1();
        }

        //-------------------------------------------------------------------------------------[]
        internal static IItem GetNewItem1X2()
        {
            return EssenceWorldInstance.GetNewItem1X2();
        }

        //-------------------------------------------------------------------------------------[]
        internal static IItem GetNewItem2X2()
        {
            return EssenceWorldInstance.GetNewItem2X2();
        }

        //-------------------------------------------------------------------------------------[]
        internal static IItem FindItemById( string id )
        {
            return EssenceWorldInstance.FindItemById( id );
        }

        //-------------------------------------------------------------------------------------[]
        public static event Action OnCraftNewItem;

        //===============================================================================================[]
        #endregion




        #region Routines
        //===============================================================================================[]
        private static ComponentEssenceManagerComponent GetEssenceManager()
        {
            FindManagerIfNeed();
            return _essenceManager;
        }

        //-------------------------------------------------------------------------------------[]
        private static void FindManagerIfNeed()
        {
            if( _essenceManager == null )
                _essenceManager = UnityHelper.FindComponentWithoutCheck<ComponentEssenceManagerComponent>();
        }

        //-------------------------------------------------------------------------------------[]
        private static void OnNeedCraftNewItem()
        {
            if( OnCraftNewItem != null )
                OnCraftNewItem();
        }

        //===============================================================================================[]
        #endregion
    }
}