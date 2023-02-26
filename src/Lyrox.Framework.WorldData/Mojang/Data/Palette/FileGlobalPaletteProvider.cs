using System.Text;
using Lyrox.Framework.Core.Models.World;
using Lyrox.Framework.World.Mojang.Data.Palette.Abstraction;
using Lyrox.Framework.World.Properties;
using Newtonsoft.Json.Linq;

namespace Lyrox.Framework.World.Mojang.Data.Palette;

internal class FileGlobalPaletteProvider : IGlobalPaletteProvider
{
    private readonly Dictionary<int, BlockState> _idToState;

    public FileGlobalPaletteProvider()
    {
        _idToState = new();

        Load();
    }

    private void Load()
    {
        var json = JObject.Parse(Encoding.UTF8.GetString(Resources.blocks));
        foreach (var block in json.Properties())
            foreach (var state in ((JObject)block.Value)["states"])
            {
                var id = (int)state["id"];
                var blockState = new BlockState(id, block.Name);
                _idToState[id] = blockState;
            }
    }

    public BlockState GetStateFromId(int id)
        => _idToState[id];
}
