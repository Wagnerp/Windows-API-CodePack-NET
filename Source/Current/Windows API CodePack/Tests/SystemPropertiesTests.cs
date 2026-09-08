using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace WindowsAPICodePack.Tests;

public class SystemPropertiesTests
{
    [Fact]
    public void SystemAuthorKeyIsStable()
    {
        PropertyKey key = SystemProperties.System.Author;

        Assert.NotEqual(Guid.Empty, key.FormatId);
        Assert.True(key.PropertyId > 0);
        Assert.Equal(key, SystemProperties.System.Author);
    }
}
