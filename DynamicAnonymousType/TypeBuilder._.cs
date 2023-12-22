using System.Reflection;
using System.Reflection.Emit;
namespace DynamicAnonymousType;

internal static partial class TypeBuilderExt
{
    static readonly Type EqualityComparerType = typeof(EqualityComparer<>);
    static readonly MethodInfo EqualityComparerDefault = EqualityComparerType.GetProperty(nameof(EqualityComparer<int>.Default), BindingFlags.Static | BindingFlags.Public)!.GetMethod!;
    static readonly MethodInfo EqualityComparerEquals = EqualityComparerType.GetMethod(nameof(EqualityComparer<int>.Equals), [EqualityComparerType.GetGenericArguments().First(), EqualityComparerType.GetGenericArguments().First()])!;
    static readonly MethodInfo EqualityComparerGetHashCode = EqualityComparerType.GetMethod(nameof(EqualityComparer<int>.GetHashCode), [EqualityComparerType.GetGenericArguments().First()])!;
}
