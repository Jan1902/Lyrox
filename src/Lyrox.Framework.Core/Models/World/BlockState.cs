namespace Lyrox.Framework.Core.Models.World
{
    public class BlockState
    {
        public int ID { get; }
        public string BlockName { get; }

        public BlockState(int id, string blockName)
        {
            ID = id;
            BlockName = blockName;
        }
    }
}
