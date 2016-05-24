// Aleksey 061WR Pustohaylov [stilluswr@gmail.com]
// Last edit: 21-02-2014
using Assets.Classes;
using Assets.Interfaces;
using UnityEngine;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnassignedField.Compiler

namespace Assets.Scripts
{
    internal class DraggableItemComponent : MonoBehaviour
    {
        #region Data
        //===============================================================================================[]
        private Transform _mTrans;
        private bool _mIsDragging;
        private bool _mSticky;
        private Transform _mParent;
        private Vector3 _startPos;
        private bool _canDrag;
        private string _id;
        //-------------------------------------------------------------------------------------[]
        private IDragDropContainer _prevContainer;
        //-------------------------------------------------------------------------------------[]
        private IItem _currentItem;
        //===============================================================================================[]
        #endregion




        #region Public data
        //===============================================================================================[]
        public int ItemWidth;
        public int ItemHeight;
        //-------------------------------------------------------------------------------------[]
        public int ItemSize { get { return _currentItem.Size; } }
        //-------------------------------------------------------------------------------------[]
        public string ItemId
        {
            get
            {
                GetItemIfNeed();

                return _currentItem.Id;
            }
        }

        //===============================================================================================[]
        #endregion




        #region MonoBehaviour
        //===============================================================================================[]
        public void Awake()
        {
            _mTrans = transform;
        }

        //-------------------------------------------------------------------------------------[]
        public void Start()
        {
            _startPos = _mTrans.localPosition;

            GetItemIfNeed();
        }

        //-------------------------------------------------------------------------------------[]
        public void OnDrag( Vector2 delta )
        {
            if( enabled &&
                UICamera.currentTouchID > -2 &&
                _canDrag ) {
                if( !_mIsDragging ) {
                    _mIsDragging = true;

                    _mParent = _mTrans.parent;

                    _mTrans.parent = DragDropRoot.root;

                    Vector3 pos = _mTrans.localPosition;
                    _mTrans.localPosition = pos;

                    NGUITools.MarkParentAsChanged( gameObject );
                }
                else
                    _mTrans.localPosition += ( Vector3 )delta;
            }
        }

        //-------------------------------------------------------------------------------------[]
        public void OnPress( bool isPressed )
        {
            if( enabled ) {
                _canDrag = isPressed;
                if( isPressed ) {
                    if( !UICamera.current.stickyPress ) {
                        _mSticky = true;
                        UICamera.current.stickyPress = true;
                    }
                }
                else if( _mSticky ) {
                    _mSticky = false;
                    UICamera.current.stickyPress = false;
                }

                _mIsDragging = false;
                Collider col = GetComponent<Collider>();
                if( col != null )
                    col.enabled = !isPressed;
                if( !isPressed )
                    Drop();
            }
        }

        //===============================================================================================[]
        #endregion




        #region Public methods
        //===============================================================================================[]
        public void UpdatePos( Vector2 newItemFinalPos )
        {
            _startPos = new Vector3( newItemFinalPos.x, newItemFinalPos.y, _startPos.z );
        }

        //===============================================================================================[]
        #endregion




        #region Routines
        //===============================================================================================[]
        private void GetItemIfNeed()
        {
            if( _currentItem == null ) {
                if( ItemWidth == 1 &&
                    ItemHeight == 1 )
                    _currentItem = EssenceManagerHelper.GetNewItem1X1();
                if( ItemWidth == 2 &&
                    ItemHeight == 1 )
                    _currentItem = EssenceManagerHelper.GetNewItem2X1();
                if( ItemWidth == 1 &&
                    ItemHeight == 2 )
                    _currentItem = EssenceManagerHelper.GetNewItem1X2();
                if( ItemWidth == 2 &&
                    ItemHeight == 2 )
                    _currentItem = EssenceManagerHelper.GetNewItem2X2();
            }
        }

        //-------------------------------------------------------------------------------------[]
        private void Drop()
        {
            if( _mTrans.parent != DragDropRoot.root )
                return;
            Collider col = UICamera.lastHit.collider;
            IDragDropContainer container = ( col != null )
                                               ? col.gameObject.GetInterface<IDragDropContainer>()
                                               : null;
            if( CanDropToContainer( container ) )
                DropToContainer( container );
            else
                ReturnItemToPrevParent();

            _prevContainer = null;

            NGUITools.MarkParentAsChanged( gameObject );
        }

        //-------------------------------------------------------------------------------------[]
        private bool CanDropToContainer( IDragDropContainer container )
        {
            return container != null && container.CanAddItemToContainer();
        }

        //-------------------------------------------------------------------------------------[]
        private void DropToContainer( IDragDropContainer container )
        {
            _mTrans.parent = ( container as MonoBehaviour ).transform;

            ( container as MonoBehaviour ).gameObject.SendMessage(
                "OnNewChild",
                _mTrans.gameObject.name + Constants.DifferentValues.SpecialSeparatorSymbol + ItemId );

            if( !container.WasItemAdded() ) {
                _mTrans.parent = _mParent;
                _mTrans.localPosition = _startPos;
            }
            else {
                _startPos = new Vector3( container.NewItemFinalPos.x, container.NewItemFinalPos.y, _startPos.z );

                var prevContainer = _mParent.gameObject.GetInterface<IDragDropContainer>();
                if( prevContainer != null &&
                    prevContainer != container )
                    prevContainer.RemoveItem( ItemId );
            }
        }

        //-------------------------------------------------------------------------------------[]
        private void ReturnItemToPrevParent()
        {
            if( _prevContainer != null ) {
                _mTrans.parent = ( _prevContainer as MonoBehaviour ).transform;

                ( _prevContainer as MonoBehaviour ).gameObject.SendMessage(
                    "OnNewChild",
                    _mTrans.gameObject.name + Constants.DifferentValues.SpecialSeparatorSymbol + ItemId );
            }
            _mTrans.parent = _mParent;
            _mTrans.localPosition = _startPos;
        }

        //===============================================================================================[]
        #endregion
    }
}