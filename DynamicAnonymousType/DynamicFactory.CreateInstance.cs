namespace DynamicAnonymousType;

public static partial class DynamicFactory
{
    public static object CreateInstance(this Type type, Dictionary<string, object?> propVals)
    {
        return CreateInstance(type, propVals as IEnumerable<KeyValuePair<string, object?>>);
    }
    
    public static object CreateInstance(this Type type, IEnumerable<KeyValuePair<string, object?>>? propVals = null)
    {
        var instance = Activator.CreateInstance(type)!;

        if (propVals != null)
            foreach (var kvp in propVals)
                if(kvp.Value != null)
                    type.GetProperty(kvp.Key)!.SetValue(instance, kvp.Value);
        
        return instance;
    }


}
