// Aleksey 061WR Pustohaylov [stilluswr@gmail.com]
// Last edit: 21-02-2014
using Assets.Classes;
using Assets.Interfaces;
using UnityEngine;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnassignedField.Compiler

namespace Assets.Scripts
{
    internal class CraftingDragDropContainerComponent : MonoBehaviour, IDragDropContainer
    {
        #region Data
        //===============================================================================================[]
        private ICraftingItemPlace _currentCraftingItemPlace;
        private bool _wasItemAdded;
        private Vector2 _newItemFinalPos;
        //===============================================================================================[]
        #endregion




        #region Public data
        //===============================================================================================[]
        public int Index;
        //===============================================================================================[]
        #endregion




        #region MonoBehaviour
        //===============================================================================================[]
        public void Start()
        {
            _currentCraftingItemPlace = EssenceManagerHelper.CraftingItemPlaces[ Index ];
        }

        //===============================================================================================[]
        #endregion




        #region IDragDropContainer
        //===============================================================================================[]
        public bool CanAddItemToContainer()
        {
            return !_currentCraftingItemPlace.ContainsItem();
        }

        //-------------------------------------------------------------------------------------[]
        public bool WasItemAdded()
        {
            return _wasItemAdded;
        }

        //-------------------------------------------------------------------------------------[]
        public void RemoveItem( string id )
        {
            _currentCraftingItemPlace.RemoveItem( id );
        }

        //-------------------------------------------------------------------------------------[]
        public Vector2 NewItemFinalPos { get { return _newItemFinalPos; } }

        //===============================================================================================[]
        #endregion




        #region Public methods
        //===============================================================================================[]
        public void OnNewChild( object value )
        {
            if( transform.childCount > 0 ) {
                if( CanAddItemToContainer() ) {
                    var info = ( ( string )value ).Split( '#' );
                    var objectName = info[ 0 ];
                    var objectId = info[ 1 ];
                    var child = transform.FindChild( objectName );
                    SpringPosition.Begin( child.gameObject, Vector3.zero, Constants.DifferentValues.SpringPositionStrength );
                    _currentCraftingItemPlace.AddItem( objectId );
                    _wasItemAdded = true;
                    _newItemFinalPos = Vector3.zero;
                }
                else
                    _wasItemAdded = false;
            }
        }

        //===============================================================================================[]
        #endregion
    }
}