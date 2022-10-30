using Lyrox.Framework.Core.Models.NBT;
using Lyrox.Framework.Networking.Mojang;
using Lyrox.Framework.Networking.Mojang.Data;
using Lyrox.Framework.Test.Properties;

namespace Lyrox.Framework.Test.Mojang
{
    public class NbtReaderTests
    {
        [Test]
        public void TestSmall()
        {
            using var stream = new MemoryStream(Resources.smalltest);
            using var reader = new MojangBinaryReader(stream);
            var nbtRaw = MojangNBTReader.ParseNBT(reader);

            Assert.That(nbtRaw, Is.Not.Null);
            Assert.That(nbtRaw, Is.TypeOf<CompoundTag>());

            var nbt = nbtRaw as CompoundTag;

            Assert.Multiple(() =>
            {
                Assert.That(nbt.Name, Is.EqualTo("hello world"));
                Assert.That(nbt.Children.Count(), Is.EqualTo(1));
                Assert.That(nbt.Children.First(), Is.TypeOf<StringTag>());
            });

            Assert.Multiple(() =>
            {
                Assert.That(nbt.Children.First().Name, Is.EqualTo("name"));
                Assert.That(((StringTag)nbt.Children.First()).Value, Is.EqualTo("Bananrama"));
            });
        }
    }
}
