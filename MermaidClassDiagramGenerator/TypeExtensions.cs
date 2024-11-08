using System.Collections;
using System.Text;

namespace MermaidClassDiagramGenerator;

public static class TypeExtensions
{
    private static readonly HashSet<Type> SimpleBuiltInTypes = new HashSet<Type>
    {
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
        typeof(object),
    };

    public static bool IsSimpleBuiltInType(this Type type)
    {
        return SimpleBuiltInTypes.Contains(type);
    }
    public static Type GetNonNullableType(this Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            return Nullable.GetUnderlyingType(type)!;
        }
        return type;
    }
    
    public static bool IsCollectionType(this Type type, out Type? elementType)
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
    
    public static bool InheritsFromGenericType(this Type type, Type genericType)
    {
        var baseType = type.BaseType;
        if (baseType != null && baseType != typeof(object))
        {
            if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }
        }
        return false;
    }
    
    public static string GetFullGenericTypeName(this Type type)
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
