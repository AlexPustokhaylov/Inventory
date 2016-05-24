// Aleksey 061WR Pustohaylov [stilluswr@gmail.com]
// Last edit: 21-02-2014
using Assets.Interfaces;

namespace Assets.Classes
{
    internal sealed class Item : IItem
    {
        #region Public data
        //===============================================================================================[]
        public string Id { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        //-------------------------------------------------------------------------------------[]
        public int Size { get { return Width*Height; } }
        //===============================================================================================[]
        #endregion




        #region Constructor
        //===============================================================================================[]
        public Item(
            string id,
            int width,
            int height )
        {
            Id = id;
            Width = width;
            Height = height;
        }

        //===============================================================================================[]
        #endregion
    }
}