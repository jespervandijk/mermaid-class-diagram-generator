using System.Reflection;

namespace Generator;

internal static class PropertyExensions
{
    internal static bool IsValueProperty(this PropertyInfo prop) =>
        prop.PropertyType.IsSimpleBuiltInType() || prop.PropertyType.IsValueType;
}
