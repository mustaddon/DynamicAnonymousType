using System.Reflection;
using System.Reflection.Emit;
namespace DynamicAnonymousType;

internal static partial class TypeBuilderExt
{
    public static TypeBuilder AddGenericParams(this TypeBuilder typeBuilder, IEnumerable<string> genericParamNames, out GenericTypeParameterBuilder[] genericParams)
    {
        genericParams = typeBuilder.DefineGenericParameters(genericParamNames.ToArray());
        return typeBuilder;
    }
}
