﻿using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;

namespace DynamicAnonymousType;

public static partial class DynamicFactory
{
    public static Type CreateType(params string[] propNames)
    {
        return CreateType(propNames as IEnumerable<string>);
    }

    public static Type CreateType(IEnumerable<string> propNames)
    {
        return Types.GetOrAdd(string.Join("|", propNames), x => new Lazy<Type>(() => DefineType(propNames))).Value;
    }

    private static TypeInfo DefineType(IEnumerable<string> propNames)
    {
        var name = $"DynamicAnonymousType_{Guid.NewGuid():N}";
        
        return Module.DefineType(name, TypeAttributes.Public | TypeAttributes.Class)
            .AddGenericParams(propNames.Select((x,i) => "T" + i), out var genericParams)
            .AddProperties(propNames, genericParams, out var fields)
            .OverrideEquals(fields)
            .OverrideGetHashCode(fields)
            //.OverrideToString() --> no one needs
            .CreateTypeInfo()!;
    }

    private static readonly ConcurrentDictionary<string, Lazy<Type>> Types = new();

    private const string AssemblyName = "DynamicAnonymousType.Dynamic";

    private static readonly ModuleBuilder Module = AssemblyBuilder
        .DefineDynamicAssembly(new AssemblyName(AssemblyName), AssemblyBuilderAccess.Run)
        .DefineDynamicModule(AssemblyName);
}