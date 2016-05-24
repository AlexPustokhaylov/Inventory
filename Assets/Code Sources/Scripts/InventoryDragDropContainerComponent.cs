// Aleksey 061WR Pustohaylov [stilluswr@gmail.com]
// Last edit: 21-02-2014
using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Classes;
using Assets.Interfaces;
using UnityEngine;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnassignedField.Compiler

namespace Assets.Scripts
{
    internal class InventoryDragDropContainerComponent : MonoBehaviour, IDragDropContainer
    {
        #region Data
        //===============================================================================================[]
        private IInventory _inventory;
        private InventoryGuiCell[,] _inventoryCells;
        //-------------------------------------------------------------------------------------[]
        private bool _wasElementAdded;
        private Vector2 _newItemFinalPos;
        //===============================================================================================[]
        #endregion




        #region Public data
        //===============================================================================================[]
        public int CellWidth;
        public int CellHeight;
        //===============================================================================================[]
        #endregion




        #region MonoBehaviour
        //===============================================================================================[]
        public void Start()
        {
            _inventory = EssenceManagerHelper.Inventory;
            _inventory.OnSortItems += OnSortItems;

            FillInventoryCells();
        }

        //===============================================================================================[]
        #endregion




        #region IDragDropContainer
        //===============================================================================================[]
        public bool CanAddItemToContainer()
        {
            return true;
        }

        //-------------------------------------------------------------------------------------[]
        public bool WasItemAdded()
        {
            return _wasElementAdded;
        }

        //-------------------------------------------------------------------------------------[]
        public void RemoveItem( string id )
        {
            _inventory.RemoveItem( id );
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
                var info = ( ( string )value ).Split( '#' );
                var objectName = info[ 0 ];
                var objectId = info[ 1 ];
                var child = transform.FindChild( objectName );
                var nearCells = new List<DistanceToCell>();

                var item = EssenceManagerHelper.FindItemById( objectId );

                var itemSize = item.Size;

                foreach( var inventoryCell in _inventoryCells ) {
                    var magnitude =
                        ( new Vector2( child.localPosition.x, child.localPosition.y ) - inventoryCell.Center ).magnitude;
                    if( magnitude < Math.Sqrt( CellWidth*CellWidth + CellWidth*CellWidth ) )
                        nearCells.Add( new DistanceToCell {Magnitude = magnitude, InventoryCell = inventoryCell} );
                }
                var nearCellsSortedByMagnitudeCells = nearCells.OrderBy( t => t.Magnitude ).ToList();

                var rightCells = new List<InventoryGuiCell>();

                var allCellsExists = GetNeededCellsToDropItemAndGetResultIfAllOfCellsExists(
                    nearCellsSortedByMagnitudeCells,
                    rightCells,
                    itemSize,
                    item.Width,
                    item.Height );

                AddExtraCellIfNeed( allCellsExists, rightCells );

                AddItemToInventoryIfCan( allCellsExists, item, rightCells, child, itemSize );
            }
        }

        //===============================================================================================[]
        #endregion




        #region Routines
        //===============================================================================================[]
        private void FillInventoryCells()
        {
            _inventoryCells = new InventoryGuiCell[Constants.InventoryValues.Width, Constants.InventoryValues.Height];

            var centerCellIndex = new Vector2(
                ( float )Math.Floor( Constants.InventoryValues.Width/2.0 ),
                ( float )Math.Floor( Constants.InventoryValues.Height/2.0 ) );

            for( int rowCoord = 0; rowCoord < Constants.InventoryValues.Width; rowCoord++ ) {
                for( int colCoord = 0; colCoord < Constants.InventoryValues.Height; colCoord++ ) {
                    _inventoryCells[ rowCoord, colCoord ] =
                        new InventoryGuiCell(
                            new Vector2(
                                CellWidth*( rowCoord - centerCellIndex.x ),
                                -CellHeight*( colCoord - centerCellIndex.y ) ),
                            rowCoord,
                            colCoord );
                }
            }
        }

        //-------------------------------------------------------------------------------------[]
        private void SetPositionToNewInventoryElement(
            Transform child,
            int itemSize,
            List<InventoryGuiCell> nearCells )
        {
            var firstCellInventoryCoords = nearCells[ 0 ].IndexInInventory;
            var rowCells = nearCells.Where( t => t.IndexInInventory.ColCoord == firstCellInventoryCoords.ColCoord );
            var colCells = nearCells.Where( t => t.IndexInInventory.RowCoord == firstCellInventoryCoords.RowCoord );
            var newPos = new Vector2(
                rowCells.Sum( t => t.Center.x )/rowCells.Count(),
                colCells.Sum( t => t.Center.y )/colCells.Count() );
            SpringPosition.Begin( child.gameObject, newPos, Constants.DifferentValues.SpringPositionStrength );
            _newItemFinalPos = newPos;
        }

        //-------------------------------------------------------------------------------------[]
        private bool CanAddItem(
            string itemId,
            List<InventoryGuiCell> rightCells )
        {
            return _inventory.CanAddItemToInventory(
                itemId,
                rightCells.Select( t => new InventoryIndex( t.IndexInInventory.RowCoord, t.IndexInInventory.ColCoord ) ).ToList() );
        }

        //-------------------------------------------------------------------------------------[]
        private void OnSortItems()
        {
            for( int i = 0; i < transform.childCount; i++ ) {
                var child = transform.GetChild( i );
                var itemComponent = child.GetComponent<DraggableItemComponent>();
                if( itemComponent ) {
                    SetPositionToNewInventoryElement(
                        child,
                        itemComponent.ItemSize,
                        _inventory.ItemCellsAfterSort( itemComponent.ItemId ).Select(
                            t => GetCellCoordByIndex( t.IndexInInventory.RowCoord, ( int )t.IndexInInventory.ColCoord ) ).ToList() );
                    itemComponent.UpdatePos( _newItemFinalPos );
                }
            }
        }

        //-------------------------------------------------------------------------------------[]
        private InventoryGuiCell GetCellCoordByIndex(
            int rowIndex,
            int colIndex )
        {
            return _inventoryCells[ rowIndex, colIndex ];
        }

        //-------------------------------------------------------------------------------------[]
        private bool GetNeededCellsToDropItemAndGetResultIfAllOfCellsExists(
            List<DistanceToCell> nearCellsSortedByMagnitudeCells,
            List<InventoryGuiCell> rightCells,
            int itemSize,
            int itemWidth,
            int itemHeight )
        {
            var allCellsExists = true;

            if( nearCellsSortedByMagnitudeCells.Count > 0 ) {
                if( itemSize == 1 )
                    rightCells.Add( nearCellsSortedByMagnitudeCells[ 0 ].InventoryCell );
                if( itemSize > 1 ) {
                    rightCells.Add( nearCellsSortedByMagnitudeCells[ 0 ].InventoryCell );
                    var firstCellInventoryCoords = nearCellsSortedByMagnitudeCells[ 0 ].InventoryCell.IndexInInventory;
                    nearCellsSortedByMagnitudeCells.Remove( nearCellsSortedByMagnitudeCells[ 0 ] );
                    if( itemWidth == 2 ) {
                        var anotherCellInRow =
                            nearCellsSortedByMagnitudeCells.FirstOrDefault(
                                t => t.InventoryCell.IndexInInventory.ColCoord == firstCellInventoryCoords.ColCoord );
                        if( anotherCellInRow != null )
                            rightCells.Add( anotherCellInRow.InventoryCell );
                        else
                            allCellsExists = false;
                    }
                    if( itemHeight == 2 ) {
                        var anotherCellInCol =
                            nearCellsSortedByMagnitudeCells.FirstOrDefault(
                                t => t.InventoryCell.IndexInInventory.RowCoord == firstCellInventoryCoords.RowCoord );
                        if( anotherCellInCol != null )
                            rightCells.Add( anotherCellInCol.InventoryCell );
                        else
                            allCellsExists = false;
                    }
                }
            }
            else
                allCellsExists = false;

            return allCellsExists;
        }

        //-------------------------------------------------------------------------------------[]
        private void AddExtraCellIfNeed(
            bool allCellsExists,
            List<InventoryGuiCell> rightCells )
        {
            if( allCellsExists ) {
                if( rightCells.Count == 3 ) {
                    var rowCoords = rightCells.Select( t => t.IndexInInventory.RowCoord ).ToList();
                    var rowCoord = rowCoords.First( t => rowCoords.Count( r => r == t ) == 1 );
                    var colCoords = rightCells.Select( t => t.IndexInInventory.ColCoord ).ToList();
                    var colCoord = colCoords.First( t => colCoords.Count( r => r == t ) == 1 );
                    rightCells.Add( _inventoryCells[ rowCoord, colCoord ] );
                }
            }
        }

        //-------------------------------------------------------------------------------------[]
        private void AddItemToInventoryIfCan(
            bool allCellsExists,
            IItem item,
            List<InventoryGuiCell> rightCells,
            Transform child,
            int itemSize )
        {
            if( !allCellsExists ||
                !CanAddItem( item.Id, rightCells ) )
                _wasElementAdded = false;
            else {
                SetPositionToNewInventoryElement( child, itemSize, rightCells );
                _wasElementAdded = true;
                _inventory.AddItem( item.Id, rightCells.Select( t => t.IndexInInventory ).ToList() );
            }
        }

        //===============================================================================================[]
        #endregion




        #region Helpers
        //===============================================================================================[]
        private sealed class DistanceToCell
        {
            public float Magnitude { get; set; }
            public InventoryGuiCell InventoryCell { get; set; }
        }

        //===============================================================================================[]
        #endregion
    }
}