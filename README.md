# DynamicAnonymousType [![NuGet version](https://badge.fury.io/nu/DynamicAnonymousType.svg?)](http://badge.fury.io/nu/DynamicAnonymousType)
Dynamic creating Anonymous (similar) Types at program runtime

## Example
```C#
using DynamicAnonymousType;


Type type = DynamicFactory.CreateType("Id", "Name", "Date")
    .MakeGenericType(typeof(int), typeof(string), typeof(DateTime?));

dynamic instance1 = type.CreateInstance(new() {
    { "Id", 1 },
    { "Name", "Text1" },
    { "Date", DateTime.Now },
});

// OR

dynamic instance2 = type.CreateInstance();
instance2.Id = 2;
instance2.Name = "Text2";
instance2.Date = DateTime.Now;
```
[Program.cs](https://github.com/mustaddon/DynamicAnonymousType/tree/main/Examples/Program.cs)