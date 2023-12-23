using DynamicAnonymousType;
using System.Text.Json;


// Creating Type
Type type = DynamicFactory.CreateType(
    ("Id", typeof(int)),
    ("Name", typeof(string)),
    ("Date", typeof(DateTime?)));


// Creating Instance
dynamic instance1 = type.CreateInstance(
    ("Id", 1),
    ("Name", "Text1"),
    ("Date", DateTime.Now));

Console.WriteLine(JsonSerializer.Serialize(instance1));

// OR
dynamic instance2 = type.CreateInstance();
instance2.Id = 2;
instance2.Name = "Text2";

Console.WriteLine(JsonSerializer.Serialize(instance2));

