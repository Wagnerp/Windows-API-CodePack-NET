using Microsoft.WindowsAPICodePack.Sensors;

namespace WindowsAPICodePack.Tests;

public class SensorDescriptionAttributeTests
{
    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    [InlineData("11111111-2222-3333-4444-555555555555")]
    public void ConstructionStoresSensorType(string sensorTypeGuid)
    {
        SensorDescriptionAttribute attribute = new(sensorTypeGuid);
        Assert.Equal(sensorTypeGuid, attribute.SensorType);
    }
}
