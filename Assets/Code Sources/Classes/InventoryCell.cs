// Aleksey 061WR Pustohaylov [stilluswr@gmail.com]
// Last edit: 21-02-2014
namespace Assets.Classes
{
    internal sealed class InventoryCell
    {
        internal bool ContainsItem { get { return !string.IsNullOrEmpty( ItemId ); } }
        internal string ItemId { get; private set; }
        internal InventoryIndex IndexInInventory { get; private set; }

        internal InventoryCell(
            int rowCoord,
            int colCoord )
        {
            IndexInInventory = new InventoryIndex( rowCoord, colCoord );
            ItemId = "";
        }

        internal void SetItem( string id )
        {
            ItemId = id;
        }

        internal void RemoveItem( string id )
        {
            if( ItemId == id )
                ItemId = "";
        }
    }
}