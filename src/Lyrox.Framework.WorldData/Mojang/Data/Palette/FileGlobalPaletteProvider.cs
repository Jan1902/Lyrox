using System.Text;
using Lyrox.Framework.Core.Models.World;
using Lyrox.Framework.WorldData.Mojang.Data.Palette.Abstraction;
using Lyrox.Framework.WorldData.Properties;
using Newtonsoft.Json.Linq;

namespace Lyrox.Framework.WorldData.Mojang.Data.Palette
{
    internal class FileGlobalPaletteProvider : IGlobalPaletteProvider
    {
        private readonly Dictionary<int, BlockState> _idToState;
        private readonly Dictionary<BlockState, int> _stateToId;

        public FileGlobalPaletteProvider()
        {
            _idToState = new();
            _stateToId = new();

            Load();
        }

        private void Load()
        {
            var json = JObject.Parse(Encoding.UTF8.GetString(Resources.blocks));
            foreach (var block in json.Properties())
            {
                foreach (var state in ((JObject)block.Value)["states"])
                {
                    var id = (int)state["id"];
                    var blockState = new BlockState(id, block.Name);
                    _idToState[id] = blockState;
                    _stateToId[blockState] = id;
                }
            }
        }

        public int GetIdFromState(BlockState state)
            => _stateToId[state];

        public BlockState GetStateFromId(int id)
            => _idToState[id];
    }
}
