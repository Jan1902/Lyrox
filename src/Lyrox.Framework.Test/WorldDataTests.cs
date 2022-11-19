using Lyrox.Framework.Core.Models.World;
using Lyrox.Framework.WorldData.Mojang.Data.Palette;
using Lyrox.Framework.WorldData.Mojang.Data.Palette.Abstraction;

namespace Lyrox.Framework.Test
{
    internal class WorldDataTests
    {
        private IGlobalPaletteProvider _stubGlobalPaletteProvider;

        [SetUp]
        public void Setup()
        {
            var mockGlobalPaletteProvider = new Mock<IGlobalPaletteProvider>();
            mockGlobalPaletteProvider
                .Setup(p => p.GetStateFromId(It.IsAny<int>()))
                .Returns(() => new BlockState(0, "minecraft:air"));
            _stubGlobalPaletteProvider = mockGlobalPaletteProvider.Object;
        }

        [Test]
        [TestCase(0, false, typeof(SingleValuedPalette))]
        [TestCase(3, false, typeof(IndirectPalette))]
        [TestCase(5, false, typeof(IndirectPalette))]
        [TestCase(3, true, typeof(IndirectPalette))]
        [TestCase(15, false, typeof(DirectPalette))]
        public void Test_PaletteFactory(int bitsPerEntry, bool biome, Type expectedPaletteType)
        {
            IPaletteFactory sut = new PaletteFactory(_stubGlobalPaletteProvider);
            var palette = sut.CreatePalette(bitsPerEntry, biome);

            palette.Should().BeOfType(expectedPaletteType);
        }
    }
}
