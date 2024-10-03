using System.Reflection;
using System.Text;
using Haas.Documentation.Utils;

namespace Generator.Utils;

public class DomainDiagramGenerator
{
    private readonly StringBuilder _classBuilder = new();
    private readonly StringBuilder _relationBuilder = new();
    
    private readonly string _filePath;
    private readonly List<Type> _aggregateRoots;
    private readonly List<Type> _allTypesAssemblies;
    private readonly List<Type> _passedTypes;
    private readonly bool _withoutProperties;

    public DomainDiagramGenerator(string filePath, List<Assembly> assemblies, List<Type> aggregateRoots, bool withoutProperties = false)
    {
        _filePath = filePath;
        _aggregateRoots = aggregateRoots;
        _withoutProperties = withoutProperties;
        _allTypesAssemblies = assemblies.SelectMany(a => a.GetTypes()).ToList();
        _passedTypes = new List<Type>();
    }
    
    public void Generate()
    {
        _classBuilder.Clear();
        CsToMermaid.StartClassDiagram(_classBuilder) ;
        
        foreach (var aggregateRoot in _aggregateRoots)
        {
            CreateClassRecursive(aggregateRoot);
        }

        var content = _classBuilder.ToString() + _relationBuilder.ToString();
        
        File.WriteAllText(_filePath, content);
    }
    
    public void CreateClassRecursive(Type type)
    {
        _passedTypes.Add(type);

        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        var notInheritedProperties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        
        var valueProperties =
            _withoutProperties ? [] :
            properties
            .Where(prop => prop.IsValueProperty())
            .ToList(); 
        
        CsToMermaid.CreateClass(_classBuilder, type, valueProperties);

        if (type.BaseType is not null && _allTypesAssemblies.Contains(type.BaseType))
        {
            CsToMermaid.CreateInheritance(_relationBuilder, type.BaseType, type);
        }
        
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
}
