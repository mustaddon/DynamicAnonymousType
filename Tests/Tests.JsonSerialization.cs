using System.Text;
using System.Text.Json;

namespace Tests;

public partial class Tests
{
    [Test]
    public void JsonSerialization()
    {
        var propVals = new Dictionary<string, object?>() {
            { "Id", 100 },
            { "Name", "Text" },
            { "Date", DateTime.Now },
            { "Bool", true },
            { "Enum", BindingFlags.Public },
            { "Null", null }
        };

        var type = DynamicFactory.CreateType(propVals.Select(x => x.Key))
            .MakeGenericType(propVals.Select(x => x.Value?.GetType() ?? typeof(object)).ToArray());

        dynamic instance = DynamicFactory.CreateInstance(type, propVals);

        var json = JsonSerializer.Serialize(instance);

        dynamic deserialized = JsonSerializer.Deserialize(json, type);

        Assert.That(object.Equals(instance, deserialized), Is.True);
    }
}