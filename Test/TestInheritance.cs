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

    private abstract class Person<T>
    {
    }

    private class Driver : Person<Driver>
    {
    }

    [Fact]
    public void Generate_BaseClassPassed_InheritanceDocumented()
    {
        var generator = new DiagramGenerator(FilePath, [Assembly.GetExecutingAssembly()],
            [typeof(Vehicle)], true);
        generator.Generate();
        var content = File.ReadAllText(FilePath);
        content.Should().Contain("class Car");
        content.Should().Contain("class Vehicle");
        content.Should().Contain("Vehicle <|-- Car");
    }
    
    [Fact]
    public void Generate_GenericBaseClassPassed_InheritanceDocumented()
    {
        var generator = new DiagramGenerator(FilePath, [Assembly.GetExecutingAssembly()],
            [typeof(Person<>)], true);
        generator.Generate();
        var content = File.ReadAllText(FilePath);
        content.Should().Contain("class `Person<T>`");
        content.Should().Contain("class Driver");
        content.Should().Contain("`Person<T>` <|-- Driver");
    }
    
    [Fact]
    public void Generate_InheritorPassed_InheritanceAndGenericBaseClassDocumented()
    {
        var generator = new DiagramGenerator(FilePath, [Assembly.GetExecutingAssembly()],
            [typeof(Driver)], true);
        generator.Generate();
        var content = File.ReadAllText(FilePath);
        content.Should().Contain("class Driver");
        content.Should().Contain("class `Person<T>`");
        content.Should().Contain("`Person<T>` <|-- Driver");
    }

    
    [Fact]
    public void Generate_InheritorPassed_InheritanceAndGBaseClassDocumented()
    {
        var generator = new DiagramGenerator(FilePath, [Assembly.GetExecutingAssembly()],
            [typeof(Car)], true);
        generator.Generate();
        var content = File.ReadAllText(FilePath);
        content.Should().Contain("class Car");
        content.Should().Contain("class Vehicle");
        content.Should().Contain("Vehicle <|-- Car");
    }
}