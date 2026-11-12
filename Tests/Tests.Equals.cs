namespace Tests;

public partial class Tests
{
    [Test]
    public void Equals()
    {
        var propVals = Enumerable.Range(0, 5).SelectMany(x => new ValueTuple<string, object?>[] {
            ($"Int{x}", x + 1000),
            ($"Bool{x}", x%2 == 0),
            ($"String{x}", $"Text{x}"),
        });

        var type = DynamicFactory.CreateType(propVals.Select(x => (x.Item1, x.Item2!.GetType())));

        dynamic instA = DynamicFactory.CreateInstance(type, propVals);
        dynamic instB = DynamicFactory.CreateInstance(type, propVals);
        dynamic instC = DynamicFactory.CreateInstance(type, propVals.Select(x => (x.Item1, x.Item1.StartsWith("String") ? x.Item2 + "C" : x.Item2)));
        dynamic instD = DynamicFactory.CreateInstance(type, propVals.Select(x => (x.Item1, x.Item1.StartsWith("Bool") ? x.Item2!.Equals(false) : x.Item2)));

        Assert.That(instA, Is.Not.Null);
        Assert.That(object.Equals(instA, instA), Is.True);
        Assert.That(object.Equals(instA, instB), Is.True);
        Assert.That(object.Equals(instA, instC), Is.False);
        Assert.That(object.Equals(instA, instD), Is.False);
        Assert.That(object.Equals(instC, instD), Is.False);
    }

    [Test]
    public void NullValuesAreAssigned()
    {
        var type = DynamicFactory.CreateType(
            ("Id", typeof(int?)),
            ("Name", typeof(string)));

        dynamic instance = DynamicFactory.CreateInstance(type,
            new[]
            {
                new KeyValuePair<string, object?>("Id", null),
                new KeyValuePair<string, object?>("Name", null)
            });

        Assert.That((int?)instance.Id, Is.Null);
        Assert.That((string?)instance.Name, Is.Null);
    }
}