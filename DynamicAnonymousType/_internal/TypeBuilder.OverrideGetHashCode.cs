using System.Reflection;
using System.Reflection.Emit;
namespace DynamicAnonymousType._internal;

internal static partial class TypeBuilderExt
{
    public static TypeBuilder OverrideGetHashCode(this TypeBuilder typeBuilder, FieldBuilder[] fields)
    {
        var method = typeBuilder.DefineMethod(nameof(object.GetHashCode),
            MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.ReuseSlot | MethodAttributes.HideBySig,
            typeof(int),
            Type.EmptyTypes);

        var methodIL = method.GetILGenerator();

        methodIL.Emit(OpCodes.Ldc_I4, 1442674604);

        foreach (var field in fields)
        {
            var equalityComparerT = EqualityComparerType.MakeGenericType(field.FieldType);
            methodIL.Emit(OpCodes.Ldc_I4, -1521134295);
            methodIL.Emit(OpCodes.Mul);
            methodIL.Emit(OpCodes.Call, TypeBuilder.GetMethod(equalityComparerT, EqualityComparerDefault));
            methodIL.Emit(OpCodes.Ldarg_0);
            methodIL.Emit(OpCodes.Ldfld, field);
            methodIL.Emit(OpCodes.Callvirt, TypeBuilder.GetMethod(equalityComparerT, EqualityComparerGetHashCode));
            methodIL.Emit(OpCodes.Add);
        }

        methodIL.Emit(OpCodes.Ret);

        return typeBuilder;
    }

}
