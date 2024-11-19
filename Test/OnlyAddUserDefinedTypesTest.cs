using System.Reflection;
using FluentAssertions;
using MermaidClassDiagramGenerator;

namespace Test;

public class OnlyAddUserDefinedTypesTest
{
    private const string FilePath = "../../../Outputs/onlyAddUserDefinedTypes.md";
    
    private class Car : Object
    {
    }

    [Fact]
    public void Generate_ClassInheritsFromDotnetType_DotnetTypeNotDocumented()
    {
        var generator = new DiagramGenerator(FilePath, [Assembly.GetExecutingAssembly()],
            [typeof(Car)], true);
        generator.Generate();
        var content = File.ReadAllText(FilePath);
        content.Should().Contain("class Car");
        content.Should().NotContain("class Object");
    }
    
    [Fact]
    public void Generate_ClassInheritsFromDotnetTypeAndDotnetTypeDirectlyPassed_DotnetTypeDocumented()
    {
        var generator = new DiagramGenerator(FilePath, [Assembly.GetExecutingAssembly()],
            [typeof(Car), typeof(Object)], true);
        generator.Generate();
        var content = File.ReadAllText(FilePath);
        content.Should().Contain("class Car");
        content.Should().Contain("class Object");
    }
}