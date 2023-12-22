namespace Tests;

public partial class Tests
{
    [Test]
    public void SingleTypeByProps()
    {
        var props = Enumerable.Range(0, 10).Select(x => $"Prop{x}");

        var typeA = DynamicFactory.CreateType(props);
        var typeB = DynamicFactory.CreateType(props);
        var typeC = DynamicFactory.CreateType(props.Select(x => x+"C"));

        Assert.That(typeA, Is.Not.Null);
        Assert.That(typeA, Is.EqualTo(typeB));
        Assert.That(typeC, Is.Not.Null);
        Assert.That(typeC, Is.Not.EqualTo(typeA));
    }
}