using _internal;

namespace Tests;

public partial class Tests
{
    [Test]
    public void ToStringT()
    {
        var propVals = new Dictionary<string, object?>() {
            { "Id", 100 },
            { "Name", "Text" },
            { "Date", DateTime.Now },
            { "Bool", true },
            { "Enum", BindingFlags.Public },
            { "Null", null }
        };

        var type = DynamicFactory.CreateType(propVals.Select(x => (x.Key, x.Value?.GetType() ?? typeof(object))));

        dynamic instance = DynamicFactory.CreateInstance(type, propVals);

        Assert.That(instance.ToString(), Is.EqualTo(Common.ToString(instance)));
    }
}