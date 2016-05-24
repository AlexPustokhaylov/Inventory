// Aleksey 061WR Pustohaylov [stilluswr@gmail.com]
// Last edit: 21-02-2014
namespace Assets.Interfaces
{
    internal interface IItem
    {
        string Id { get; }
        int Width { get; }
        int Height { get; }
        int Size { get; }
    }
}