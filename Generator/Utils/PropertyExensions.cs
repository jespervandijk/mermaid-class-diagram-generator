using System.Reflection;

namespace Haas.Documentation.Utils;

public static class PropertyExensions
{
    public static bool IsValueProperty(this PropertyInfo prop) =>
        prop.PropertyType.IsSimpleBuiltInType() || prop.PropertyType.IsValueType;
    
}
