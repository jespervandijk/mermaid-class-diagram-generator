using System.Reflection;
using FluentAssertions;
using MermaidClassDiagramGenerator;

namespace Test;

public class TestGenericClassNames
{
    private const string FilePath = "../../../Outputs/testGenericClassNames.md";
    
    private class Vehicle<T>
    {
    }
    private class Car : Vehicle<Car>
    {
    }

    [Fact]
    public void Generate_GenericBaseClass_ShouldBeDocumentedBetweenBackticks()
    {
        var generator = new DiagramGenerator(FilePath, [Assembly.GetExecutingAssembly()],
            [typeof(Vehicle<>)], true);
        generator.Generate();
        var content = File.ReadAllText(FilePath);
        content.Should().Contain("class `Vehicle<T>`");
        content.Should().Contain("class Car");
        content.Should().Contain("`Vehicle<T>` <|-- Car");
    }
}