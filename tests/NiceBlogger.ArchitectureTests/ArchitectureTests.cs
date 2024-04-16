using NetArchTest.Rules;

namespace NiceBlogger.ArchitectureTests;

public class ArchitectureTests
{
    private const string InfrastructureProjectNameSpace = "NiceBlogger.Infrastructure";
    private const string ApiProjectNameSpace = "NiceBlogger.Api";

    [Fact]
    public void UseCases_Should_Not_HaveDependency_On_AnyOtherProjects()
    {
        // Arrange
        var assembly = typeof(UseCases.ApplicationServiceExtensions).Assembly;

        var otherProjects = new[]
        {
            InfrastructureProjectNameSpace,
            ApiProjectNameSpace
        };

        // Act
        var result = Types.InAssembly(assembly)
            .Should()
            .NotHaveDependencyOnAny(otherProjects)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Infrastructure_Should_Not_HaveDependency_On_AnyOtherProjects()
    {
        // Arrange
        var assembly = typeof(Infrastructure.InfrastructureServiceExtensions).Assembly;

        var otherProjects = new[]
        {
            ApiProjectNameSpace
        };

        // Act
        var result = Types.InAssembly(assembly)
            .Should()
            .NotHaveDependencyOnAny(otherProjects)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful);
    }
}