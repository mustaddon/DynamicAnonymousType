using System.Reflection.Emit;
namespace _internal;

internal static partial class TypeBuilderExt
{
    public static TypeBuilder AddGenericParams(this TypeBuilder typeBuilder, IEnumerable<string> genericParamNames, out GenericTypeParameterBuilder[] genericParams)
    {
        genericParams = typeBuilder.DefineGenericParameters(genericParamNames.ToArray());
        return typeBuilder;
    }
}
