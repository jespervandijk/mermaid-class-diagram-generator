using System.Reflection;
using FluentAssertions;
using MermaidClassDiagramGenerator;

namespace Test;

public class TestClassesAndRelationships
{
    private const string FilePath = "../../../Outputs/testClassesAndRelationships.md";
    
    private class Car
    {
        public List<Wheel> Wheels { get; set; }
        public Engine Engine { get; set; }
    }

    private class Wheel
    {
    }
    
    private class Engine
    {
    }
    
    private class Driver
    {
    }

    [Fact]
    public void Generate_SingleClass_DocumentsClass()
    {
        var generator = new DiagramGenerator(FilePath, [Assembly.GetExecutingAssembly()],
            [typeof(Car)], true);
        generator.Generate();
        var content = File.ReadAllText(FilePath);
        content.Should().Contain("class Car");
    }

    [Fact]
    public void Generate_MultipleClasses_DocumentsAllClasses()
    {
        var generator = new DiagramGenerator(FilePath, [Assembly.GetExecutingAssembly()],
            [typeof(Car), typeof(Driver)], true);
        generator.Generate();
        var content = File.ReadAllText(FilePath);
        content.Should().Contain("class Car");
        content.Should().Contain("class Driver");
    }

    [Fact]
    public void Generate_ClassWithRelationship_DocumentsRelationship()
    {
        var generator = new DiagramGenerator(FilePath, [Assembly.GetExecutingAssembly()],
            [typeof(Car)], true);
        generator.Generate();
        var content = File.ReadAllText(FilePath);
        content.Should().Contain("class Car");
        content.Should().Contain("class Engine");
        content.Should().Contain("Car o-- Engine");
    }
    
    [Fact]
    public void Generate_ClassWithRelationshipToCollection_DocumentsCollectionRelationship()
    {
        var generator = new DiagramGenerator(FilePath, [Assembly.GetExecutingAssembly()],
            [typeof(Car)], true);
        generator.Generate();
        var content = File.ReadAllText(FilePath);
        content.Should().Contain("class Car");
        content.Should().Contain("class Wheel");
        content.Should().Contain("Car \"0\" o-- \"*\" Wheel");
    }
}