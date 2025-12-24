using Mma.Cli.Shared.Builders;
using System.Text.Json;
using Xunit;

namespace Mma.Cli.Tests;

public class SerializationTests
{
    [Fact]
    public void EntityConfig_Serialization_Works()
    {
        // Arrange
        var config = new EntityConfig
        {
            ComponentName = "TestEntity",
            PkType = "Guid",
            Mapper = "AutoMapper",
            SolutionName = "TestSolution",
            ProjectsPath = "C:\\Test"
        };

        // Act
        var json = JsonSerializer.Serialize(config);
        var deserialized = JsonSerializer.Deserialize<EntityConfig>(json);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(config.ComponentName, deserialized.ComponentName);
        Assert.Equal(config.PkType, deserialized.PkType);
        Assert.Equal(config.Mapper, deserialized.Mapper);
    }

    [Fact]
    public void Replacements_AreCorrect()
    {
        // Arrange
        var config = new EntityConfig
        {
            ComponentName = "User",
            PkType = "int",
            SolutionName = "MyApp"
        };

        // Act
        var replacements = config.GetReplacements();

        // Assert
        Assert.Equal("User", replacements["$EntityName"]);
        Assert.Equal("int", replacements["$PK"]);
        Assert.Equal("MyApp", replacements["$SolutionName"]);
        Assert.Equal("Users", replacements["$EntitySetName"]);
        Assert.Equal("user", replacements["$EntityVarName"]);
    }
}
