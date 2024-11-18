using System.Reflection;
using MermaidClassDiagramGenerator;

namespace Example.Examples;

public class SimpleExample
{
    public static void Run()
    {
        var generator = new DiagramGenerator(
            outputFilePath: "../../../Outputs/simpleExample.md",
            assembliesToScan: new List<Assembly> { Assembly.GetExecutingAssembly() },
            domainTypes: new List<Type> { typeof(Car), typeof(Wheels) },
            generateWithoutProperties: false
        );

        generator.Generate();

        Console.WriteLine("Mermaid.js class diagram generated successfully at simpleExample.md");
    }

    public class Car
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