using System.Reflection;
using System.Reflection.Emit;
namespace _internal;

internal static partial class TypeBuilderExt
{
    public static TypeBuilder OverrideEquals(this TypeBuilder typeBuilder, FieldBuilder[] fields)
    {
        var method = typeBuilder.DefineMethod(nameof(object.Equals),
            MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.ReuseSlot | MethodAttributes.HideBySig,
            typeof(bool),
            [typeof(object)]);

        var methodIL = method.GetILGenerator();

        methodIL.DeclareLocal(typeBuilder);

        var labelTrue = methodIL.DefineLabel();
        var labelFalse = methodIL.DefineLabel();
        var labelRet = methodIL.DefineLabel();

        // if (this == arg1) return true;
        methodIL.Emit(OpCodes.Ldarg_1);
        methodIL.Emit(OpCodes.Isinst, typeBuilder);
        methodIL.Emit(OpCodes.Stloc_0);
        methodIL.Emit(OpCodes.Ldarg_0);
        methodIL.Emit(OpCodes.Ldloc_0);
        methodIL.Emit(OpCodes.Beq, labelTrue);

        // if (arg1 is not CurrentType<T1, T2, ...>) return false;
        methodIL.Emit(OpCodes.Ldloc_0);
        methodIL.Emit(OpCodes.Brfalse, labelFalse);

        // if (!EqualityComparer<T1>.Default.Equals(x.Id, Id)) return false;
        foreach (var field in fields)
        {
            var equalityComparerT = EqualityComparerType.MakeGenericType(field.FieldType);
            methodIL.Emit(OpCodes.Call, TypeBuilder.GetMethod(equalityComparerT, EqualityComparerDefault));
            methodIL.Emit(OpCodes.Ldarg_0);
            methodIL.Emit(OpCodes.Ldfld, field);
            methodIL.Emit(OpCodes.Ldloc_0);
            methodIL.Emit(OpCodes.Ldfld, field);
            methodIL.Emit(OpCodes.Callvirt, TypeBuilder.GetMethod(equalityComparerT, EqualityComparerEquals));
            methodIL.Emit(OpCodes.Brfalse, labelFalse);
        }

        methodIL.Emit(OpCodes.Br_S, labelTrue);

        // return false;
        methodIL.MarkLabel(labelFalse);
        methodIL.Emit(OpCodes.Ldc_I4_0);
        methodIL.Emit(OpCodes.Br_S, labelRet);

        // return true;
        methodIL.MarkLabel(labelTrue);
        methodIL.Emit(OpCodes.Ldc_I4_1);

        methodIL.MarkLabel(labelRet);
        methodIL.Emit(OpCodes.Ret);

        return typeBuilder;
    }

}
