using System.Reflection;
using FluentAssertions;
using MermaidClassDiagramGenerator;

namespace Test;

public class TestInheritance
{
    private const string FilePath = "../../../Outputs/testInheritance.md";
    
    private abstract class Vehicle
    {
        
    }
    private class Car : Vehicle
    {
    }

    [Fact]
    public void Generate_ClassAndBaseClass_InheritanceDocumented()
    {
        var generator = new DiagramGenerator(FilePath, [Assembly.GetExecutingAssembly()],
            [typeof(Vehicle)], true);
        generator.Generate();
        var content = File.ReadAllText(FilePath);
        content.Should().Contain("class Car");
        content.Should().Contain("class Vehicle");
        content.Should().Contain("Vehicle <|-- Car");
    }
    
    // generic inheritance
    
    // also document base class
}