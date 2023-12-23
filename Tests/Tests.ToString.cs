using System.Text;

namespace Tests;

public partial class Tests
{
    //[Test]
    public void ToStringM()
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

        var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var builder = new StringBuilder();
        builder.Append("{ ");
        for (var i = 0; i < props.Length; i++)
        {
            if (i > 0) builder.Append(", ");
            builder.Append(props[i].Name);
            builder.Append(" = ");
            builder.Append(props[i].GetValue(instance)?.ToString() ?? string.Empty);
        }
        builder.Append(" }");

        Assert.That(instance.ToString(), Is.EqualTo(builder.ToString()));
    }
}