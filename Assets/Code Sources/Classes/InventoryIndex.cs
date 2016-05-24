// Aleksey 061WR Pustohaylov [stilluswr@gmail.com]
// Last edit: 21-02-2014
namespace Assets.Classes
{
    internal sealed class InventoryIndex
    {
        #region Public data
        //===============================================================================================[]
        internal int RowCoord { get; private set; }
        internal int ColCoord { get; private set; }
        //===============================================================================================[]
        #endregion




        #region Constructor
        //===============================================================================================[]
        internal InventoryIndex(
            int rowCoord,
            int colCoord )
        {
            RowCoord = rowCoord;
            ColCoord = colCoord;
        }

        //===============================================================================================[]
        #endregion
    }
}