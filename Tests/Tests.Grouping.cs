namespace Tests;

public partial class Tests
{
    [Test]
    public void Grouping()
    {
        var groupsCount = _random.Next(3, 11);

        var type = DynamicFactory.CreateType(
            ("Prop1", typeof(int)),
            ("Prop2", typeof(bool?)),
            ("Prop3", typeof(string)));

        var insts = Enumerable.Range(0, groupsCount)
            .SelectMany(x => Enumerable.Range(0, _random.Next(1, 10))
                .Select(xx => DynamicFactory.CreateInstance(type,
                    ("Prop1", x + 100),
                    ("Prop2", x % 2 == 0),
                    ("Prop3", $"Text{x}")
                )))
            .ToList();

        var groups = insts.GroupBy(x => x).ToList();

        Assert.That(insts.Count, Is.GreaterThan(groupsCount));
        Assert.That(groups.Count, Is.EqualTo(groupsCount));
    }
}