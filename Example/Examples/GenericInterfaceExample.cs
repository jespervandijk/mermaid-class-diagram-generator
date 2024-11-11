using System.Reflection;
using MermaidClassDiagramGenerator;

namespace Example.Examples;

public class GenericInterfaceExample
{
    public static void Run()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var aggregateTypes = assembly.GetTypes().Where(type => type.ImplementsGenericInterface(typeof(IAggregate<>)))
            .ToList();
        
        var generator = new DiagramGenerator(
            outputFilePath: "../../../Outputs/genericInterfaceExample.md",
            assembliesToScan: new List<Assembly> { assembly },
            domainTypes: aggregateTypes,
            generateWithoutProperties: false
        );
            
        generator.Generate();

        Console.WriteLine("Mermaid.js class diagram generated successfully at genericInterfaceExample.md");
    }

    public interface IAggregate<T>
    {
    }

    public class Auto : IAggregate<Auto>
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