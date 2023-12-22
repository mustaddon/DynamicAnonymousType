using System.Reflection;
using System.Reflection.Emit;
using System.Text;
namespace DynamicAnonymousType;

internal static partial class TypeBuilderExt
{
    public static TypeBuilder OverrideToString(this TypeBuilder typeBuilder)
    {
        var method = typeBuilder.DefineMethod(nameof(object.ToString),
            MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.ReuseSlot | MethodAttributes.HideBySig,
            typeof(string),
            Type.EmptyTypes);

        var methodIL = method.GetILGenerator();

        methodIL.DeclareLocal(typeof(PropertyInfo[]));
        methodIL.DeclareLocal(typeof(StringBuilder));
        methodIL.DeclareLocal(typeof(int));
        methodIL.DeclareLocal(typeof(bool));
        methodIL.DeclareLocal(typeof(bool));
        methodIL.DeclareLocal(typeof(string));

        var labelForStart = methodIL.DefineLabel();
        var labelForIf = methodIL.DefineLabel();
        var labelForSepless = methodIL.DefineLabel();
        var labelForHasVal = methodIL.DefineLabel();
        var labelForNotVal = methodIL.DefineLabel();

        methodIL.Emit(OpCodes.Nop);

        // props = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        methodIL.Emit(OpCodes.Ldarg_0);
        methodIL.Emit(OpCodes.Call, ObjectGetType);
        methodIL.Emit(OpCodes.Ldc_I4, (int)(BindingFlags.Public | BindingFlags.Instance));
        methodIL.Emit(OpCodes.Callvirt, TypeGetProperties);
        methodIL.Emit(OpCodes.Stloc_0);

        // builder = new StringBuilder();
        methodIL.Emit(OpCodes.Newobj, StringBuilderConstructor);
        methodIL.Emit(OpCodes.Stloc_1);

        // builder.Append("{ ");
        methodIL.Emit(OpCodes.Ldloc_1);
        methodIL.Emit(OpCodes.Ldstr, "{ ");
        methodIL.Emit(OpCodes.Callvirt, StringBuilderAppend);

        methodIL.Emit(OpCodes.Pop);
        methodIL.Emit(OpCodes.Ldc_I4_0);
        methodIL.Emit(OpCodes.Stloc_2);

        methodIL.MarkLabel(labelForStart);
        methodIL.Emit(OpCodes.Nop);

        // if (i > 0) builder.Append(", ");
        methodIL.Emit(OpCodes.Ldloc_2);
        methodIL.Emit(OpCodes.Ldc_I4_0);
        methodIL.Emit(OpCodes.Cgt);
        methodIL.Emit(OpCodes.Stloc_3);
        methodIL.Emit(OpCodes.Ldloc_3);
        methodIL.Emit(OpCodes.Brfalse_S, labelForSepless);
        methodIL.Emit(OpCodes.Ldloc_1);
        methodIL.Emit(OpCodes.Ldstr, ", ");
        methodIL.Emit(OpCodes.Callvirt, StringBuilderAppend);
        methodIL.Emit(OpCodes.Pop);

        // builder.Append(props[i].Name);
        methodIL.MarkLabel(labelForSepless);
        methodIL.Emit(OpCodes.Ldloc_1);
        methodIL.Emit(OpCodes.Ldloc_0);
        methodIL.Emit(OpCodes.Ldloc_2);
        methodIL.Emit(OpCodes.Ldelem_Ref);
        methodIL.Emit(OpCodes.Callvirt, MethodInfoName);
        methodIL.Emit(OpCodes.Callvirt, StringBuilderAppend);
        methodIL.Emit(OpCodes.Pop);

        // builder.Append(" = ");
        methodIL.Emit(OpCodes.Ldloc_1);
        methodIL.Emit(OpCodes.Ldstr, " = ");
        methodIL.Emit(OpCodes.Callvirt, StringBuilderAppend);
        methodIL.Emit(OpCodes.Pop);

        // builder.Append(props[i].GetValue(this)?.ToString());
        methodIL.Emit(OpCodes.Ldloc_1);
        methodIL.Emit(OpCodes.Ldloc_0);
        methodIL.Emit(OpCodes.Ldloc_2);
        methodIL.Emit(OpCodes.Ldelem_Ref);
        methodIL.Emit(OpCodes.Ldarg_0);
        methodIL.Emit(OpCodes.Callvirt, PropertyInfoGetValue);
        methodIL.Emit(OpCodes.Dup);
        methodIL.Emit(OpCodes.Brtrue_S, labelForHasVal);

        // builder.Append(null);
        methodIL.Emit(OpCodes.Pop);
        methodIL.Emit(OpCodes.Ldnull);
        methodIL.Emit(OpCodes.Br_S, labelForNotVal);

        methodIL.MarkLabel(labelForHasVal);
        methodIL.Emit(OpCodes.Callvirt, ObjectToString);
        methodIL.MarkLabel(labelForNotVal);
        methodIL.Emit(OpCodes.Callvirt, StringBuilderAppend);
        methodIL.Emit(OpCodes.Pop);

        // i++;
        methodIL.Emit(OpCodes.Nop);
        methodIL.Emit(OpCodes.Ldloc_2);
        methodIL.Emit(OpCodes.Ldc_I4_1);
        methodIL.Emit(OpCodes.Add);
        methodIL.Emit(OpCodes.Stloc_2);

        // if(i<props.Length)
        methodIL.MarkLabel(labelForIf);
        methodIL.Emit(OpCodes.Ldloc_2);
        methodIL.Emit(OpCodes.Ldloc_0);
        methodIL.Emit(OpCodes.Ldlen);
        methodIL.Emit(OpCodes.Conv_I4);
        methodIL.Emit(OpCodes.Clt);
        methodIL.Emit(OpCodes.Stloc_S, 4);
        methodIL.Emit(OpCodes.Ldloc_S, 4);
        methodIL.Emit(OpCodes.Brtrue_S, labelForStart);


        // builder.Append(" }");
        methodIL.Emit(OpCodes.Ldloc_1);
        methodIL.Emit(OpCodes.Ldstr, " }");
        methodIL.Emit(OpCodes.Callvirt, StringBuilderAppend);

        // return builder.ToString();
        methodIL.Emit(OpCodes.Pop);
        methodIL.Emit(OpCodes.Ldloc_1);
        methodIL.Emit(OpCodes.Callvirt, ObjectToString);
        methodIL.Emit(OpCodes.Stloc_S, 5);
        methodIL.Emit(OpCodes.Ldloc_S, 5);
        methodIL.Emit(OpCodes.Ret);

        return typeBuilder;
    }

    static readonly ConstructorInfo StringBuilderConstructor = typeof(StringBuilder).GetConstructor(Type.EmptyTypes)!;
    static readonly MethodInfo StringBuilderAppend = typeof(StringBuilder).GetMethod(nameof(StringBuilder.Append), [typeof(string)])!;
    static readonly MethodInfo TypeGetProperties = typeof(Type).GetMethod(nameof(Type.GetProperties), [typeof(BindingFlags)])!;
    static readonly MethodInfo ObjectGetType = typeof(object).GetMethod(nameof(object.GetType), Type.EmptyTypes)!;
    static readonly MethodInfo ObjectToString = typeof(object).GetMethod(nameof(object.ToString), Type.EmptyTypes)!;
    static readonly MethodInfo MethodInfoName = typeof(MethodInfo).GetProperty(nameof(MethodInfo.Name), BindingFlags.Public | BindingFlags.Instance)!.GetMethod!;
    static readonly MethodInfo PropertyInfoGetValue = typeof(PropertyInfo).GetMethod(nameof(PropertyInfo.GetValue), [typeof(object)])!;
}
