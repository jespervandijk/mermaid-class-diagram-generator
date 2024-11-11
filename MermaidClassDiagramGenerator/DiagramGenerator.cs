using System.Reflection;
using System.Text;

namespace MermaidClassDiagramGenerator;

public class DiagramGenerator
{
    private readonly StringBuilder _classBuilder = new();
    private readonly StringBuilder _relationBuilder = new();
    
    private readonly string _outputFilePath;
    private readonly List<Type> _domainTypes;
    private readonly List<Type> _allTypesAssemblies;
    private readonly List<Type> _passedTypes;
    private readonly bool _generateWithoutProperties;

    /// <summary>
    /// Initializes a new instance of the <see cref="DiagramGenerator"/> class for creating and configuring Mermaid.js class diagrams.
    /// </summary>
    /// <param name="outputFilePath">
    /// The file path where the generated Mermaid.js diagram will be saved. 
    /// Ensure the path is valid and the application has write permissions.
    /// The file must have a <c>.md</c> extension as the generator outputs the diagram in Markdown format, which supports Mermaid.js syntax.
    /// </param>
    /// <param name="assembliesToScan">
    /// A collection of assemblies that the generator will scan to discover domain classes.
    /// These assemblies should contain the classes you want to include in the diagram.
    /// </param>
    /// <param name="domainTypes">
    /// An array of domain class types that the generator should document. 
    /// The generator processes these types recursively, meaning that if a domain class 
    /// (e.g., <c>Auto</c>) has properties of other domain types (e.g., <c>Wheels</c>), 
    /// those related types will also be included in the generated class diagram automatically.
    /// </param>
    /// <param name="generateWithoutProperties">
    /// A boolean flag indicating whether to generate the class diagram without including property details.
    /// If set to <c>true</c>, the diagram will display class names without listing their properties.
    /// </param>
    public DiagramGenerator(string outputFilePath, List<Assembly> assembliesToScan, List<Type> domainTypes, bool generateWithoutProperties = false)
    {
        _outputFilePath = outputFilePath;
        _domainTypes = domainTypes;
        _generateWithoutProperties = generateWithoutProperties;
        _allTypesAssemblies = assembliesToScan.SelectMany(a => a.GetTypes()).ToList();
        _passedTypes = [];
    }
    
    /// <summary>
    /// Generates the Mermaid.js class diagram.
    /// </summary>
    public void Generate()
    {
        _classBuilder.Clear();
        CsToMermaid.StartClassDiagram(_classBuilder) ;
        
        foreach (var aggregateRoot in _domainTypes)
        {
            CreateClassRecursive(aggregateRoot);
        }

        var content = _classBuilder.ToString() + _relationBuilder.ToString();
        
        File.WriteAllText(_outputFilePath, content);
    }

    private void CreateClassRecursive(Type type)
    {
        if (type.ShouldExcludeFromDiagram())
        {
            return;
        }
        
        _passedTypes.Add(type);

        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        var notInheritedProperties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        
        var valueProperties =
            _generateWithoutProperties ? [] :
            properties
            .Where(prop => prop.IsValueProperty())
            .ToList(); 
        
        CsToMermaid.CreateClass(_classBuilder, type, valueProperties);

        CreateInheritance(type);
        
        foreach (var property in properties)
        {
            if (!property.IsValueProperty() && property.PropertyType.IsCollectionType(out var innerType))
            {
                if (innerType is not null && !_passedTypes.Contains(innerType) && _allTypesAssemblies.Contains(innerType))
                {
                    CreateClassRecursive(innerType.GetNonNullableType());
                }
            }
            else if(!property.IsValueProperty() && 
                    property.PropertyType.IsClass && 
                    !_passedTypes.Contains(property.PropertyType) && 
                    _allTypesAssemblies.Contains(property.PropertyType))
            {
                CreateClassRecursive(property.PropertyType);
            }
            
        }
        
        foreach (var property in notInheritedProperties)
        {
            if (!property.IsValueProperty() && property.PropertyType.IsCollectionType(out var innerType))
            {
                if (innerType is not null && _allTypesAssemblies.Contains(innerType))
                {
                    CsToMermaid.CreateCompositionCollection(_relationBuilder, type, innerType);
                }
            }
            else if(!property.IsValueProperty() && property.PropertyType.IsClass && _allTypesAssemblies.Contains(property.PropertyType))
            {
                CsToMermaid.CreateComposition(_relationBuilder, type, property.PropertyType);
            }
        }
        
        var inheritors = _allTypesAssemblies.Where(t => t.IsSubclassOf(type));

        foreach (var inheritor in inheritors)
        {
            CreateClassRecursive(inheritor);
        }
    }

    private void CreateInheritance(Type type)
    {
        var baseType = type.BaseType;
        if (baseType is null || baseType.ShouldExcludeFromDiagram()) return;
        
        if (baseType.IsGenericType && _allTypesAssemblies.Contains(baseType.GetGenericTypeDefinition()))
        {
            CsToMermaid.CreateInheritance(_relationBuilder, baseType.GetGenericTypeDefinition(), type);
        }
        else if (_allTypesAssemblies.Contains(baseType))
        {
            CsToMermaid.CreateInheritance(_relationBuilder, baseType, type);
        }
    }
}
