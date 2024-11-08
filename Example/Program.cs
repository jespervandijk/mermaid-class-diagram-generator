// Example domain classes

using System.Reflection;
using Generator;

var generator = new MermaidClassDiagramGenerator(
    outputFilePath: "../../../diagram.md",
    assembliesToScan: new List<Assembly> { Assembly.GetExecutingAssembly() },
    domainTypes: new List<Type> { typeof(Auto), typeof(Wheels) },
    generateWithoutProperties: false
);
            
generator.Generate();

Console.WriteLine("Mermaid.js class diagram generated successfully at diagram.md");

public class Auto
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