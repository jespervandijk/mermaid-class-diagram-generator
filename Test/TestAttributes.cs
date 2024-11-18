using System.Reflection;
using FluentAssertions;
using MermaidClassDiagramGenerator;

namespace Test;

public class TestAttributes
{
    private const string FilePath = "../../../Outputs/testAttribute.md";

    private class Car
    {
    }
    
    [ExcludeFromDiagram]
    private class Driver
    {
    }

    [Fact]
    public void Generate_ClassWithExcludeAttribute_DoesntDocumentsClass()
    {
        var generator = new DiagramGenerator(FilePath, [Assembly.GetExecutingAssembly()],
            [typeof(Car), typeof(Driver)], true);
        generator.Generate();
        var content = File.ReadAllText(FilePath);
        content.Should().Contain("class Car");
        content.Should().NotContain("class Driver");
    }
}