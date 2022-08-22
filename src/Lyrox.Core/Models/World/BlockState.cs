namespace Lyrox.Core.Models.World
{
    public class BlockState
    {
        public int Id { get; set; }
        public string BlockName { get; set; }

        public BlockState(int id, string blockName)
        {
            Id = id;
            BlockName = blockName;
        }
    }
}
