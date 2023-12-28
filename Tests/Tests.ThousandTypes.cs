namespace Tests;

public partial class Tests
{
    [Test]
    public void ThousandTypes()
    {
        var types = Enumerable.Range(0, 1000)
            .Select(x => DynamicFactory.CreateType(
                Enumerable.Range(0, _random.Next(1, 10))
                    .Select(xx => ($"Prop{x}_{xx}", typeof(int))).ToList()
            )).ToList();

        var unique = new HashSet<Type>(types.Concat(types));

        Assert.That(types.Count, Is.EqualTo(unique.Count));
    }
}