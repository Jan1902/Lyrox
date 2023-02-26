namespace Lyrox.Framework.World.Mojang.Data;

internal class CompactedDataArray
{
    private ulong[] _dataArray;
    private int _bitsPerEntry;

    public CompactedDataArray(ulong[] dataArray, int bitsPerEntry)
    {
        _dataArray = dataArray;
        _bitsPerEntry = bitsPerEntry;
    }

    private int EntriesPerLong => 64 / _bitsPerEntry;
    private ulong IndividualValueMask => (ulong)((1 << _bitsPerEntry) - 1);

    public ulong this[int index] => Get(index);

    public ulong Get(int index)
    {
        var longIndex = index / EntriesPerLong;
        var individualOffset = (index % EntriesPerLong) * _bitsPerEntry;

        var value = (ulong)(_dataArray[longIndex] >> individualOffset);
        value &= IndividualValueMask;

        return value;
    }
}