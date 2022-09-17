using Lyrox.Core.Models.NBT;
using Lyrox.Networking.Mojang;
using Lyrox.Networking.Mojang.Data;
using Lyrox.Test.Unit.Properties;

namespace Lyrox.Test.Unit
{
    public class NbtReaderTests
    {
        [Test]
        public void TestSmall()
        {
            using var stream = new MemoryStream(Resources.smalltest);
            using var reader = new MojangBinaryReader(stream);
            var nbt = MojangNBTReader.ParseNBT(reader);

            Assert.That(nbt, Is.Not.Null);
            Assert.That(nbt, Is.TypeOf<CompoundTag>());

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
