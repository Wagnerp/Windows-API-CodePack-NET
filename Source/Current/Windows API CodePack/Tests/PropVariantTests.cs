using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace WindowsAPICodePack.Tests;

public class PropVariantTests
{
    [Theory]
    [MemberData(nameof(FromObjectTestValues))]
    public void StaticFromObjectTest(object inputObject, VarEnum expectedVarType)
    {
        using PropVariant pv = PropVariant.FromObject(inputObject);

        Assert.Equal(inputObject, pv.Value);
        Assert.Equal(expectedVarType, pv.VarType);
        Assert.False(pv.IsNullOrEmpty);
    }

    [Theory]
    [MemberData(nameof(InputObjectValues))]
    public void DisposeClearsValue(object inputObject)
    {
        using PropVariant pv = PropVariant.FromObject(inputObject);

        Assert.NotNull(pv.Value);
        Assert.NotEqual(VarEnum.VT_EMPTY, pv.VarType);
        Assert.False(pv.IsNullOrEmpty);

        pv.Dispose();

        Assert.Null(pv.Value);
        Assert.Equal(VarEnum.VT_EMPTY, pv.VarType);
        Assert.True(pv.IsNullOrEmpty);
    }

    [Fact]
    public void DefaultConstructorIsEmpty()
    {
        using PropVariant pv = new();

        Assert.True(pv.IsNullOrEmpty);
        Assert.Equal(VarEnum.VT_EMPTY, pv.VarType);
        Assert.Null(pv.Value);
    }

    [Fact]
    public void FromObjectNullIsEmpty()
    {
        using PropVariant pv = PropVariant.FromObject(null);

        Assert.True(pv.IsNullOrEmpty);
        Assert.Equal(VarEnum.VT_EMPTY, pv.VarType);
        Assert.Null(pv.Value);
    }

    public static IEnumerable<object[]> FromObjectTestValues
    {
        get
        {
            yield return new object[] { "hello", VarEnum.VT_LPWSTR };
            yield return new object[] { new[] { "hello", "world" }, VarEnum.VT_VECTOR | VarEnum.VT_LPWSTR };
            yield return new object[] { true, VarEnum.VT_BOOL };
            yield return new object[] { (byte)123, VarEnum.VT_UI1 };
            yield return new object[] { (sbyte)123, VarEnum.VT_I1 };
            yield return new object[] { (short)123, VarEnum.VT_I2 };
            yield return new object[] { (ushort)123, VarEnum.VT_UI2 };
            yield return new object[] { 123, VarEnum.VT_I4 };
            yield return new object[] { (uint)123, VarEnum.VT_UI4 };
            yield return new object[] { (long)123, VarEnum.VT_I8 };
            yield return new object[] { (ulong)123, VarEnum.VT_UI8 };
            yield return new object[] { 12.3, VarEnum.VT_R8 };
            yield return new object[] { new[] { 1, 2, 3 }, VarEnum.VT_VECTOR | VarEnum.VT_I4 };
        }
    }

    public static IEnumerable<object[]> InputObjectValues
    {
        get
        {
            yield return new object[] { true };
            yield return new object[] { (byte)123 };
            yield return new object[] { 123 };
            yield return new object[] { new[] { 1, 2, 3 } };
            yield return new object[] { "hello" };
            yield return new object[] { new[] { "hello", "world" } };
        }
    }
}
