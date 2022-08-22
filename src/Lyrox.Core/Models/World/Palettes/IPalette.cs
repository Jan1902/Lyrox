namespace Lyrox.Core.Models.World.Palettes
{
    internal interface IPalette
    {
        uint IdForState(BlockState state);
        BlockState StateForId(uint id);
        byte GetBitsPerBlock();
        void Read(Stream stream);
    }
}
