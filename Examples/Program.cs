using System.Text.Json;
using DynamicAnonymousType;


var type = DynamicFactory.CreateType("Id", "Name", "Date")
    .MakeGenericType(typeof(int), typeof(string), typeof(DateTime?));

dynamic instance1 = type.CreateInstance(new() {
    { "Id", 1 },
    { "Name", "Text1" },
    { "Date", DateTime.Now },
});

Console.WriteLine(JsonSerializer.Serialize(instance1));

dynamic instance2 = type.CreateInstance();
instance2.Id = 2;
instance2.Name = "Text2";

Console.WriteLine(JsonSerializer.Serialize(instance2));

