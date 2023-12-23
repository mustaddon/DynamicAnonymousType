namespace Tests;

public partial class Tests
{
    [Test]
    public void ThousandProps()
    {
        var propVals = Enumerable.Range(0, 1000)
            .Select(x => ($"Prop{x}", (object?)(x + 1000)));

        var type = DynamicFactory.CreateType(propVals.Select(x => (x.Item1, typeof(int))));

        dynamic instA = DynamicFactory.CreateInstance(type, propVals);
        dynamic instB = DynamicFactory.CreateInstance(type, propVals);
        dynamic instC = DynamicFactory.CreateInstance(type, propVals.Select(x => (x.Item1, (object?)((int)x.Item2! + 100))));
     
        Assert.That(instA, Is.Not.Null);
        Assert.That(object.Equals(instA, instB), Is.True);
        Assert.That(object.Equals(instA, instC), Is.False);
        Assert.That(instA.GetHashCode(), Is.EqualTo(instB.GetHashCode()));
        Assert.That(instA.GetHashCode(), Is.Not.EqualTo(instC.GetHashCode()));
        Assert.That(string.IsNullOrWhiteSpace(instA.ToString()), Is.False);
    }
}