using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace WindowsAPICodePack.Tests;

public class PropertyKeyTests
{
    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000", 5)]
    public void ConstructorWithGuid(string formatIdString, int propertyId)
    {
        Guid formatId = new(formatIdString);
        PropertyKey pk = new(formatId, propertyId);

        Assert.Equal(formatId, pk.FormatId);
        Assert.Equal(propertyId, pk.PropertyId);
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000", 5)]
    public void ConstructorWithString(string formatId, int propertyId)
    {
        PropertyKey pk = new(formatId, propertyId);

        Assert.Equal(new Guid(formatId), pk.FormatId);
        Assert.Equal(propertyId, pk.PropertyId);
    }

    [Fact]
    public void ToStringReturnsExpectedString()
    {
        Guid guid = new("00000000-1111-2222-3333-000000000000");
        const int property = 1234;
        PropertyKey key = new(guid, property);

        Assert.Equal("{" + guid + "}, " + property, key.ToString());
    }

    [Fact]
    public void EqualityOperatorsMatchValue()
    {
        PropertyKey a = new(Guid.Empty, 1);
        PropertyKey b = new(Guid.Empty, 1);
        PropertyKey c = new(Guid.Empty, 2);

        Assert.True(a == b);
        Assert.False(a != b);
        Assert.True(a != c);
        Assert.Equal(a, b);
        Assert.NotEqual(a, c);
    }
}
