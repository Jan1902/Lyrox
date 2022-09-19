using Lyrox.Framework.Core.Models.World;
using Lyrox.Framework.Networking.Mojang.Data;

namespace Lyrox.Framework.WorldData.Mojang.Data.Palette.Abstraction
{
    internal interface IPalette
    {
        BlockState? GetStateForId(int id);
        int GetBitsPerBlock();
        void Read(MojangBinaryReader reader);
    }
}
