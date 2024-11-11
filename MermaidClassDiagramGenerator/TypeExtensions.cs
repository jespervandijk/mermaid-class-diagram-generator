using System.Collections;
using System.Text;

namespace MermaidClassDiagramGenerator;

public static class TypeExtensions
{
    public static bool InheritsFromGenericType(this Type type, Type genericBaseType)
    {
        var baseType = type.BaseType;
        return baseType is not null && 
               baseType != typeof(object) && 
               baseType.IsGenericType &&
               baseType.GetGenericTypeDefinition() == genericBaseType;
    }

    public static bool InheritsFrom(this Type type, Type baseType)
        => type.BaseType == baseType;

    public static bool Implements(this Type type, Type interfaceType) 
        => interfaceType.IsInterface && type.GetInterfaces().Contains(interfaceType);
    
    public static bool ImplementsGenericInterface(this Type type, Type genericInterfaceType) 
        => genericInterfaceType.IsInterface && type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == genericInterfaceType);
    
    private static readonly HashSet<Type> SimpleBuiltInTypes =
    [
        typeof(bool),
        typeof(byte),
        typeof(sbyte),
        typeof(char),
        typeof(decimal),
        typeof(double),
        typeof(float),
        typeof(int),
        typeof(uint),
        typeof(long),
        typeof(ulong),
        typeof(short),
        typeof(ushort),
        typeof(string),
        typeof(object)
    ];

    internal static bool IsSimpleBuiltInType(this Type type)
    {
        return SimpleBuiltInTypes.Contains(type);
    }
    
    internal static Type GetNonNullableType(this Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            return Nullable.GetUnderlyingType(type)!;
        }
        return type;
    }
    
    internal static bool IsCollectionType(this Type type, out Type? elementType)
    {
        elementType = null;

        if (type.IsArray)
        {
            elementType = type.GetElementType();
            return true;
        }
        
        var iCollectionInterface = type
            .GetInterfaces()
            .FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ICollection<>));

        if (iCollectionInterface is not null)
        {
            elementType = iCollectionInterface.GetGenericArguments()[0];
            return true;
        }
        
        var iReadOnlyCollectionInterface = type
            .GetInterfaces()
            .FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IReadOnlyCollection<>));

        if (iReadOnlyCollectionInterface is not null)
        {
            elementType = iReadOnlyCollectionInterface.GetGenericArguments()[0];
            return true;
        }
        
        if (typeof(ICollection).IsAssignableFrom(type))
        {
            elementType = typeof(object);
            return true;
        }

        return false;
    }
    
    internal static string GetFullGenericTypeName(this Type type)
    {
        if (!type.IsGenericType)
        {
            return type.Name;
        }

        var typeName = new StringBuilder();
        var name = type.Name;
        
        // Remove backtick
        var backtickIndex = name.IndexOf('`');
        if (backtickIndex > 0)
        {
            name = name.Substring(0, backtickIndex);
        }

        typeName.Append(name);
        typeName.Append('<');
        
        var typeParameters = type.GetGenericArguments();
        for (int i = 0; i < typeParameters.Length; i++)
        {
            if (i > 0)
            {
                typeName.Append(", ");
            }
            typeName.Append(GetFullGenericTypeName(typeParameters[i]));
        }
        typeName.Append('>');

        return typeName.ToString();
    }
}
