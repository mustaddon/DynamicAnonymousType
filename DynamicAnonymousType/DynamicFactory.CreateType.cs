using DynamicAnonymousType._internal;
using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;

namespace DynamicAnonymousType;

public static partial class DynamicFactory
{
    public static Type CreateType(params ValueTuple<string, Type>[] propTypes) => CreateType(propTypes.AsEnumerable());
    public static Type CreateType(IEnumerable<ValueTuple<string, Type>> propTypes)
    {
        return CreateType(propTypes.Select(x => x.Item1))
            .MakeGenericType(propTypes.Select(x => x.Item2).ToArray());
    }

    public static Type CreateType(params string[] propNames) => CreateType(propNames.AsEnumerable());
    public static Type CreateType(IEnumerable<string> propNames)
    {
        var buffered = propNames.Buffer();
        return _types.GetOrAdd(string.Join("|", buffered), x => new Lazy<Type>(() => DefineType(buffered))).Value;
    }

    private static TypeInfo DefineType(IReadOnlyCollection<string> propNames)
    {
        var name = $"DynamicAnonymousType{Interlocked.Increment(ref _lastTypeNumber)}`{propNames.Count}";

        return _module.DefineType(name, TypeAttributes.Public | TypeAttributes.Class)
            .AddGenericParams(propNames.Select((x, i) => "T" + i), out var genericParams)
            .AddProperties(propNames, genericParams, out var fields)
            .OverrideEquals(fields)
            .OverrideGetHashCode(fields)
            .OverrideToString()
            .CreateTypeInfo()!;
    }

    private static readonly ConcurrentDictionary<string, Lazy<Type>> _types = new();

    private static int _lastTypeNumber = -1;

    private static readonly ModuleBuilder _module = AssemblyBuilder
        .DefineDynamicAssembly(new AssemblyName("DynamicAnonymousType.Dynamic"), AssemblyBuilderAccess.Run)
        .DefineDynamicModule("DynamicAnonymousType.Dynamic");
}
