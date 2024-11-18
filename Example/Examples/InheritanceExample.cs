using System.Reflection;
using MermaidClassDiagramGenerator;

namespace Example.Examples;

public static class InheritanceExample
{
    public static void Run()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var entityTypes = assembly.GetTypes().Where(type => type.InheritsFrom(typeof(Entity)))
            .ToList();
        
        var generator = new DiagramGenerator(
            outputFilePath: "../../../Outputs/inheritanceExample.md",
            assembliesToScan: new List<Assembly> { assembly },
            domainTypes: entityTypes,
            generateWithoutProperties: false
        );
            
        generator.Generate();

        Console.WriteLine("Mermaid.js class diagram generated successfully at inheritanceExample.md");
    }

    public abstract class Entity
    {
    }

    public class Car : Entity
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public Wheels Wheels { get; set; }
    }

    public class Wheels : Entity
    {
        public int Count { get; set; }
        public string Type { get; set; }
    }
}