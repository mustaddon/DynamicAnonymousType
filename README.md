# DynamicAnonymousType [![NuGet version](https://badge.fury.io/nu/DynamicAnonymousType.svg?v101)](http://badge.fury.io/nu/DynamicAnonymousType)
Dynamic creating Anonymous (similar) Types at program runtime

```C#
using DynamicAnonymousType;


Type type = DynamicFactory.CreateType(
    ("Id", typeof(int)),
    ("Name", typeof(string)),
    ("Date", typeof(DateTime?)));

dynamic instance = type.CreateInstance(
    ("Id", 1),
    ("Name", "Text1"),
    ("Date", DateTime.Now));
```
[Program.cs](https://github.com/mustaddon/DynamicAnonymousType/tree/main/Examples/Program.cs)
