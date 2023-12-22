namespace Tests;

public partial class Tests
{
    [Test]
    public void Equals()
    {
        var propVals = Enumerable.Range(0, 5)
            .SelectMany(x => new [] {
                new KeyValuePair<string, object?>($"Int{x}", x + 1000),
                new KeyValuePair<string, object?>($"Bool{x}", x%2 == 0),
                new KeyValuePair<string, object?>($"String{x}", $"Text{x}"),
            });

        var type = DynamicFactory.CreateType(propVals.Select(x => x.Key))
            .MakeGenericType(propVals.Select(x=> x.Value!.GetType()).ToArray());

        dynamic instA = DynamicFactory.CreateInstance(type, propVals);
        dynamic instB = DynamicFactory.CreateInstance(type, propVals);
        dynamic instC = DynamicFactory.CreateInstance(type, propVals.Select(x => new KeyValuePair<string, object?>(x.Key, x.Key.StartsWith("String") ? x.Value+"C" : x.Value)));
        dynamic instD = DynamicFactory.CreateInstance(type, propVals.Select(x => new KeyValuePair<string, object?>(x.Key, x.Key.StartsWith("Bool") ? x.Value!.Equals(false) : x.Value)));

        Assert.That(instA, Is.Not.Null);
        Assert.That(object.Equals(instA, instA), Is.True);
        Assert.That(object.Equals(instA, instB), Is.True);
        Assert.That(object.Equals(instA, instC), Is.False);
        Assert.That(object.Equals(instA, instD), Is.False);
        Assert.That(object.Equals(instC, instD), Is.False);
    }
}