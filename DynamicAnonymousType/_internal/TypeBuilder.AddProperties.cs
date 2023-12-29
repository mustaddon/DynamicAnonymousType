using System.Reflection;
using System.Reflection.Emit;
namespace DynamicAnonymousType._internal;

internal static partial class TypeBuilderExt
{
    public static TypeBuilder AddProperties(this TypeBuilder typeBuilder, IEnumerable<string> propNames, GenericTypeParameterBuilder[] genericParams, out FieldBuilder[] fields)
    {
        fields = new FieldBuilder[genericParams.Length];

        var i = -1;

        foreach (var name in propNames)
        {
            var genericParam = genericParams[++i];
            var property = typeBuilder.DefineProperty(name, PropertyAttributes.HasDefault, genericParam, null);
            var field = (fields[i] = typeBuilder.DefineField("<>f" + i, genericParam, FieldAttributes.Private));
            var attrs = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            // getter
            var getMethod = typeBuilder.DefineMethod("get_" + name, attrs, genericParam, Type.EmptyTypes);
            var getMethodIL = getMethod.GetILGenerator();
            getMethodIL.Emit(OpCodes.Ldarg_0);
            getMethodIL.Emit(OpCodes.Ldfld, field);
            getMethodIL.Emit(OpCodes.Ret);
            property.SetGetMethod(getMethod);

            // setter
            var setMethod = typeBuilder.DefineMethod("set_" + name, attrs, null, [genericParam]);
            var setMethodIL = setMethod.GetILGenerator();
            setMethodIL.Emit(OpCodes.Ldarg_0);
            setMethodIL.Emit(OpCodes.Ldarg_1);
            setMethodIL.Emit(OpCodes.Stfld, field);
            setMethodIL.Emit(OpCodes.Ret);
            property.SetSetMethod(setMethod);
        }

        return typeBuilder;
    }

}
