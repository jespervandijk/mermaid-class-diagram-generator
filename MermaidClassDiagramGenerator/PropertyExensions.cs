using System.Reflection;

namespace MermaidClassDiagramGenerator;

public static class PropertyExensions
{
    public static bool IsValueProperty(this PropertyInfo prop) =>
        prop.PropertyType.IsSimpleBuiltInType() || prop.PropertyType.IsValueType;
    
}
