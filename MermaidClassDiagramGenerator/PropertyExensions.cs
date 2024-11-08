using System.Reflection;

namespace MermaidClassDiagramGenerator;

internal static class PropertyExensions
{
    internal static bool IsValueProperty(this PropertyInfo prop) =>
        prop.PropertyType.IsSimpleBuiltInType() || prop.PropertyType.IsValueType;
}
