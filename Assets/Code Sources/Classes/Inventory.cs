// Aleksey 061WR Pustohaylov [stilluswr@gmail.com]
// Last edit: 21-02-2014
using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Interfaces;

namespace Assets.Classes
{
    internal sealed class Inventory : IInventory
    {
        #region Data
        //===============================================================================================[]
        private readonly int _width;
        private readonly int _height;
        private InventoryCell[,] _inventoryCells;
        private readonly List<IItem> _inventoryItemsList = new List<IItem>();
        //-------------------------------------------------------------------------------------[]
        private InventoryCell[,] _inventoryCellsAfterSort;
        //===============================================================================================[]
        #endregion




        #region Constructor
        //===============================================================================================[]
        public Inventory(
            int width,
            int height )
        {
            _width = width;
            _height = height;

            FillInventoryCells( _width, _height );
        }

        //===============================================================================================[]
        #endregion




        #region IInventory
        //===============================================================================================[]
        public bool CanAddItemToInventory(
            string itemId,
            IEnumerable<InventoryIndex> cellIndexes )
        {
            foreach( var cellIndex in cellIndexes ) {
                if( CellIndexExists( cellIndex ) ) {
                    var cell = _inventoryCells[ cellIndex.RowCoord, cellIndex.ColCoord ];
                    if( cell.ContainsItem ) {
                        if( cell.ItemId != itemId )
                            return false;
                    }
                }
            }
            return true;
        }

        //-------------------------------------------------------------------------------------[]
        public void AddItem(
            string itemId,
            IEnumerable<InventoryIndex> cellIndexes )
        {
            RemoveItem( itemId );
            AddItemToInventoryList( itemId );
            AddItemToInventoryCells( itemId, cellIndexes );
        }

        //-------------------------------------------------------------------------------------[]
        public void RemoveItem( string itemId )
        {
            RemoveItemFromInventoryList( itemId );
            RemoveItemFromInventoryCells( itemId );
        }

        //-------------------------------------------------------------------------------------[]
        public List<IItem> Items { get { return _inventoryItemsList; } }

        //-------------------------------------------------------------------------------------[]
        public List<InventoryCell> GetItemCells( string itemId )
        {
            return GetItemCells( _inventoryCells, itemId );
        }

        //-------------------------------------------------------------------------------------[]
        public List<InventoryCell> ItemCellsAfterSort( string itemId )
        {
            return GetItemCells( _inventoryCellsAfterSort, itemId );
        }

        //-------------------------------------------------------------------------------------[]
        public void SortItems()
        {
            var inventoryCells = new InventoryCell[_width, _height];
            for( int i = 0; i < _width; i++ ) {
                for( int j = 0; j < _height; j++ )
                    inventoryCells[ i, j ] = new InventoryCell( i, j );
            }

            var items = new List<IItem>();
            items.AddRange( _inventoryItemsList.Where( t => t.Size == 4 ) );
            items.AddRange( _inventoryItemsList.Where( t => t.Width == 2 && t.Height == 1 ) );
            items.AddRange( _inventoryItemsList.Where( t => t.Width == 1 && t.Height == 2 ) );
            items.AddRange( _inventoryItemsList.Where( t => t.Size == 1 ) );

            SortItemsSimpleAlgorithm( items, inventoryCells );

            _inventoryCellsAfterSort = inventoryCells;

            if( OnSortItems != null )
                OnSortItems();

            _inventoryCells = _inventoryCellsAfterSort;
        }

        //-------------------------------------------------------------------------------------[]
        public event Action OnSortItems;

        //===============================================================================================[]
        #endregion




        #region Routines
        //===============================================================================================[]
        private void FillInventoryCells(
            int width,
            int height )
        {
            _inventoryCells = new InventoryCell[width, height];

            for( int i = 0; i < width; i++ ) {
                for( int j = 0; j < height; j++ )
                    _inventoryCells[ i, j ] = new InventoryCell( i, j );
            }
        }

        //-------------------------------------------------------------------------------------[]
        private bool CellIndexExists( InventoryIndex cellIndex )
        {
            return cellIndex.RowCoord >= 0 && cellIndex.RowCoord < _width && cellIndex.ColCoord >= 0 &&
                   cellIndex.ColCoord < _height;
        }

        //-------------------------------------------------------------------------------------[]
        private void AddItemToInventoryList( string itemId )
        {
            _inventoryItemsList.Add( EssenceManagerHelper.FindItemById( itemId ) );
        }

        //-------------------------------------------------------------------------------------[]
        private void AddItemToInventoryCells(
            string itemId,
            IEnumerable<InventoryIndex> cellIndexes )
        {
            foreach( var cellIndex in cellIndexes )
                _inventoryCells[ cellIndex.RowCoord, cellIndex.ColCoord ].SetItem( itemId );
        }

        //-------------------------------------------------------------------------------------[]
        private void RemoveItemFromInventoryList( string itemId )
        {
            if( _inventoryItemsList.Any( t => t.Id == itemId ) )
                _inventoryItemsList.Remove( _inventoryItemsList.Find( t => t.Id == itemId ) );
        }

        //-------------------------------------------------------------------------------------[]
        private void RemoveItemFromInventoryCells( string itemId )
        {
            foreach( var inventoryCell in _inventoryCells ) {
                if( inventoryCell.ContainsItem &&
                    inventoryCell.ItemId == itemId )
                    inventoryCell.RemoveItem( itemId );
            }
        }

        //-------------------------------------------------------------------------------------[]
        private List<InventoryCell> GetItemCells(
            InventoryCell[,] inventoryCells,
            string itemId )
        {
            var cells = new List<InventoryCell>();
            foreach( var inventoryCell in inventoryCells ) {
                if( inventoryCell.ContainsItem ) {
                    if( inventoryCell.ItemId == itemId )
                        cells.Add( inventoryCell );
                }
            }
            return cells;
        }

        //-------------------------------------------------------------------------------------[]
        private void SortItemsSimpleAlgorithm(
            List<IItem> items,
            InventoryCell[,] inventoryCells )
        {
            for( int rowIndex = 0; rowIndex < _width; rowIndex++ ) {
                if( items.Count == 0 )
                    break;
                for( int colIndex = 0; colIndex < _height; colIndex++ ) {
                    if( items.Count == 0 )
                        break;
                    if( !inventoryCells[ rowIndex, colIndex ].ContainsItem ) {
                        for( int itemsIndex = 0; itemsIndex < items.Count; itemsIndex++ ) {
                            var item = items[ itemsIndex ];
                            if( CanAddItemToCells( inventoryCells, item, rowIndex, colIndex ) ) {
                                for( int i = 0; i < item.Width; i++ ) {
                                    for( int j = 0; j < item.Height; j++ )
                                        inventoryCells[ rowIndex + i, colIndex + j ].SetItem( item.Id );
                                }
                                items.Remove( item );
                                break;
                            }
                        }
                    }
                }
            }
        }

        //-------------------------------------------------------------------------------------[]
        private bool CanAddItemToCells(
            InventoryCell[,] inventoryCells,
            IItem item,
            int rowIndex,
            int colIndex )
        {
            for( int i = 0; i < item.Width; i++ ) {
                for( int j = 0; j < item.Height; j++ ) {
                    if( rowIndex + i < _width &&
                        colIndex + j < _height ) {
                        if( inventoryCells[ rowIndex + i, colIndex + j ].ContainsItem )
                            return false;
                    }
                    else
                        return false;
                }
            }
            return true;
        }

        //===============================================================================================[]
        #endregion
    }
}