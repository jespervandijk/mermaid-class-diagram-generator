using System.Reflection;
using MermaidClassDiagramGenerator;

namespace Example.Examples;

public class InterfaceExample
{
    public static void Run()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var entityTypes = assembly.GetTypes().Where(type => type.Implements(typeof(IEntity)))
            .ToList();
        
        var generator = new DiagramGenerator(
            outputFilePath: "../../../Outputs/implementsExample.md",
            assembliesToScan: new List<Assembly> { assembly },
            domainTypes: entityTypes,
            generateWithoutProperties: false
        );
            
        generator.Generate();

        Console.WriteLine("Mermaid.js class diagram generated successfully at implementsExample.md");
    }

    public interface IEntity
    {
        
    }

    public class Auto : IEntity
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public Wheels Wheels { get; set; }
    }

    public class Wheels : IEntity
    {
        public int Count { get; set; }
        public string Type { get; set; }
    }
}