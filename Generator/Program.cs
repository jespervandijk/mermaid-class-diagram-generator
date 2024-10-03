using Haas.Documentation.Utils;
using System.Reflection;
using Generator.Utils;

var assemblies = new List<Assembly>()
{
    Assembly.Load("Abn.Hypotheekdossiers.Domain.LopendeHypotheken"),
    Assembly.Load("Abn.Hypotheekdossiers.Domain.Huishoudens"),
    Assembly.Load("Abn.Hypotheekdossiers.Domain.Uitgangspunten"),
    Assembly.Load("Abn.Hypotheekdossiers.Domain"),
    Assembly.Load("Abn"),
};

var path = "C:\\repos\\Hypotheek\\services\\Hypotheekdossier\\Haas.Documentation\\Output\\diagram.md";

var aggregateRootType = typeof(Aggregate<,>);
var allAsemblyTypes = assemblies.SelectMany(a => a.GetTypes());
var aggregateRoots = allAsemblyTypes.Where(type => type.InheritsFromGenericType(aggregateRootType)).ToList();

var generator = new DomainDiagramGenerator(path, assemblies, aggregateRoots, false);

generator.Generate();
