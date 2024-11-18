using System.Reflection;
using MermaidClassDiagramGenerator;

namespace Example.Examples;

public static class ExcludeFromDiagramExample
{
    public static void Run()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var aggregateTypes = assembly.GetTypes().Where(type => type.InheritsFromGenericType(typeof(Aggregate<>)))
            .ToList();
        
        var generator = new DiagramGenerator(
            outputFilePath: "../../../Outputs/excludeFromDiagram.md",
            assembliesToScan: new List<Assembly> { assembly },
            domainTypes: aggregateTypes,
            generateWithoutProperties: false
        );
            
        generator.Generate();

        Console.WriteLine("Mermaid.js class diagram generated successfully at excludeFromDiagram.md");
    }
    
    [ExcludeFromDiagram]
    public abstract class Aggregate<T>
    {
    }

    public class Car : Aggregate<Car>
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public Wheels Wheels { get; set; }
    }

    public class Wheels
    {
        public int Count { get; set; }
        public string Type { get; set; }
    }
}