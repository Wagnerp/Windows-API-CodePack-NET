using System.Reflection;
using Microsoft.WindowsAPICodePack.Shell;

namespace WindowsAPICodePack.Tests;

public class KnownFoldersTests
{
    [Fact]
    public void DesktopKnownFolderIsAvailable()
    {
        IKnownFolder? desktop = KnownFolders.Desktop;

        Assert.NotNull(desktop);
        Assert.False(string.IsNullOrWhiteSpace(desktop.CanonicalName));
        Assert.NotEqual(Guid.Empty, desktop.FolderId);
    }

    [Fact]
    public void AllContainsDesktopWhenPresent()
    {
        IKnownFolder? desktop = KnownFolders.Desktop;
        Assert.NotNull(desktop);
        Assert.Contains(KnownFolders.All, folder => folder.FolderId == desktop.FolderId);
    }

    [Fact]
    public void StaticKnownFolderPropertiesAreIKnownFolder()
    {
        PropertyInfo[] properties = typeof(KnownFolders).GetProperties(BindingFlags.Static | BindingFlags.Public);
        Assert.Contains(properties, info => info.PropertyType == typeof(IKnownFolder) && info.Name == nameof(KnownFolders.Desktop));
    }
}
