namespace Tests;

public partial class Tests
{
    [Test]
    public void HashCode()
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
        Assert.That(instA.GetHashCode(), Is.EqualTo(instA.GetHashCode()));
        Assert.That(instA.GetHashCode(), Is.EqualTo(instB.GetHashCode()));
        Assert.That(instA.GetHashCode(), Is.Not.EqualTo(instC.GetHashCode()));
        Assert.That(instA.GetHashCode(), Is.Not.EqualTo(instD.GetHashCode()));
        Assert.That(instC.GetHashCode(), Is.Not.EqualTo(instD.GetHashCode()));
    }
}