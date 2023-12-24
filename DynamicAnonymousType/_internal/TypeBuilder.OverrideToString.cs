using System.Reflection;
using System.Reflection.Emit;
using System.Text;
namespace _internal;

internal static partial class TypeBuilderExt
{
    public static TypeBuilder OverrideToString(this TypeBuilder typeBuilder)
    {
        var method = typeBuilder.DefineMethod(nameof(object.ToString),
            MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.ReuseSlot | MethodAttributes.HideBySig,
            typeof(string),
            Type.EmptyTypes);

        var methodIL = method.GetILGenerator();

        methodIL.DeclareLocal(typeof(string));

        // return Common.ToString(this);
        methodIL.Emit(OpCodes.Nop);
        methodIL.Emit(OpCodes.Ldarg_0);
        methodIL.Emit(OpCodes.Call, CommonToString);
        methodIL.Emit(OpCodes.Stloc_0);
        methodIL.Emit(OpCodes.Ldloc_0);
        methodIL.Emit(OpCodes.Ret);

        return typeBuilder;
    }

    static readonly MethodInfo CommonToString = typeof(Common).GetMethod(nameof(Common.ToString), BindingFlags.Public | BindingFlags.Static)!;
}

public static partial class Common
{
    public static string ToString(object obj)
    {
        var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var builder = new StringBuilder();
        builder.Append("{ ");
        for (var i = 0; i < props.Length; i++)
        {
            if (i > 0) builder.Append(", ");
            builder.Append(props[i].Name);
            builder.Append(" = ");
            builder.Append(props[i].GetValue(obj)?.ToString() ?? string.Empty);
        }
        builder.Append(" }");
        return builder.ToString();
    }
}
