using System.Reflection;
using System.Text;

namespace MermaidClassDiagramGenerator;

public class CsToMermaid
{
    public static void StartClassDiagram(StringBuilder builder)
    {
        builder.Append("classDiagram\n");
    }
    public static void CreateClass(StringBuilder builder, Type classType, List<PropertyInfo> properties)
    {
        var innerBuilder = new StringBuilder();
        if (properties.Count == 0)
        {
            innerBuilder.AppendLine($"class {classType.GetFullGenericTypeName()}");
        }
        else
        {
            innerBuilder.AppendLine($$"""class {{classType.GetFullGenericTypeName()}}{""");
            foreach (var property in properties)
            {
                var accesModifier = GetPropertyAccessModifier(property);
                innerBuilder.AppendLine($"""  {accesModifier}{property.PropertyType.GetFullGenericTypeName()} {property.Name}""");
            }
            innerBuilder.AppendLine("}");
        }

        var stringToAdd = innerBuilder.ToString();
        if (!builder.ToString().Contains(stringToAdd))
        {
            builder.AppendLine(stringToAdd);
        }
        innerBuilder.Clear();
    }
    public static void CreateComposition(StringBuilder builder, Type parent, Type child)
    {
        var stringToAdd = $"{parent.GetFullGenericTypeName()} o-- {child.GetFullGenericTypeName()}";
        if (!builder.ToString().Contains(stringToAdd))
        {
            builder.AppendLine(stringToAdd);
        }
    }
    public static void CreateCompositionCollection(StringBuilder builder, Type parent, Type child)
    {
        var stringToAdd = $"""{parent.GetFullGenericTypeName()} "0" o-- "*" {child.GetFullGenericTypeName()}""";
        if (!builder.ToString().Contains(stringToAdd))
        {
            builder.AppendLine(stringToAdd);
        }
    }

    public static void CreateInheritance(StringBuilder builder, Type parent, Type child)
    {
        var stringToAdd = $"{parent.GetFullGenericTypeName()} <|-- {child.GetFullGenericTypeName()}";
        if (!builder.ToString().Contains(stringToAdd))
        {
            builder.AppendLine(stringToAdd);
        }
    }
    
    public static string GetPropertyAccessModifier(PropertyInfo property)
    {
        var getMethod = property.GetMethod;
        var setMethod = property.SetMethod;

        AccesModifier? getAccess = getMethod is not null ? GetMethodAccess(getMethod!) : null;
        AccesModifier? setAccess = setMethod is not null ? GetMethodAccess(setMethod!) : null;

        var accesModifier = (getAccess, setAccess) switch
        {
            (AccesModifier.Private, _) => AccesModifier.Private,
            (_, AccesModifier.Private) => AccesModifier.Private,
            (AccesModifier.PrivateProtected, _) => AccesModifier.PrivateProtected,
            (_, AccesModifier.PrivateProtected) => AccesModifier.PrivateProtected,
            (AccesModifier.Protected, _) => AccesModifier.Protected,
            (_, AccesModifier.Protected) => AccesModifier.Protected,
            (AccesModifier.ProtectedInternal, _) => AccesModifier.ProtectedInternal,
            (_, AccesModifier.ProtectedInternal) => AccesModifier.ProtectedInternal,
            (AccesModifier.Internal, _) => AccesModifier.Internal,
            (_, AccesModifier.Internal) => AccesModifier.Internal,
            (AccesModifier.Public, _) => AccesModifier.Public,
            (_, AccesModifier.Public) => AccesModifier.Public,
            _ => throw new ArgumentException("Unknown access modifier"),
        };

        return accesModifier switch
        {
            AccesModifier.Private => "-",
            AccesModifier.PrivateProtected => "-#",
            AccesModifier.Protected => "#",
            AccesModifier.ProtectedInternal => "#i",
            AccesModifier.Internal => "i",
            AccesModifier.Public => "+",
            _ => throw new ArgumentException("Unknown access modifier"),
        };
    }
    public enum AccesModifier
    {
        Public,
        Private,
        Protected,
        Internal,
        ProtectedInternal,
        PrivateProtected,
    }
    public static AccesModifier GetMethodAccess(MethodInfo method)
    {
        if (method.IsPublic) return AccesModifier.Public;
        if (method.IsPrivate) return AccesModifier.Private;
        if (method.IsFamily) return AccesModifier.Protected;
        if (method.IsAssembly) return AccesModifier.Internal;
        if (method.IsFamilyOrAssembly) return AccesModifier.ProtectedInternal;
        if (method.IsFamilyAndAssembly) return AccesModifier.PrivateProtected;

        throw new ArgumentException("Unknown access modifier");
    }
    
}
