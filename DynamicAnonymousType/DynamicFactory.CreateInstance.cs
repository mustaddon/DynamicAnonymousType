namespace DynamicAnonymousType;

public static partial class DynamicFactory
{
    public static object CreateInstance(this Type type, params ValueTuple<string, object?>[] propVals) => CreateInstance(type, propVals.AsEnumerable());
    public static object CreateInstance(this Type type, IEnumerable<ValueTuple<string, object?>> propVals)
    {
        return CreateInstance(type, propVals.Select(x => new KeyValuePair<string, object?>(x.Item1, x.Item2)));
    }

    public static object CreateInstance(this Type type, IEnumerable<KeyValuePair<string, object?>>? propVals = null)
    {
        var instance = Activator.CreateInstance(type)!;

        if (propVals != null)
            foreach (var kvp in propVals)
                if (kvp.Value != null)
                    type.GetProperty(kvp.Key)!.SetValue(instance, kvp.Value);

        return instance;
    }


}
