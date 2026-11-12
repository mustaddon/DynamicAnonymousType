# DynamicAnonymousType [![NuGet version](https://badge.fury.io/nu/DynamicAnonymousType.svg?v107)](http://badge.fury.io/nu/DynamicAnonymousType)

Create anonymous-like .NET types at runtime without defining a class in advance.

DynamicAnonymousType lets you generate a CLR type with named properties and then create instances of it on demand. It is useful when you need a lightweight DTO-like object, a dynamic shape for data transfer, or a runtime-generated type that behaves similarly to an anonymous object.

## Features

- Generate CLR types at runtime with strongly typed properties
- Create instances dynamically using property/value tuples
- Reuse the same generated type for repeated property sets
- Includes basic `Equals`, `GetHashCode`, and `ToString` behavior for generated instances

## Installation

```bash
dotnet add package DynamicAnonymousType
```

## Quick start

```csharp
using DynamicAnonymousType;

Type type = DynamicFactory.CreateType(
    ("Id", typeof(int)),
    ("Name", typeof(string)),
    ("Date", typeof(DateTime?)));

dynamic instance = type.CreateInstance(
    ("Id", 1),
    ("Name", "Text1"),
    ("Date", DateTime.Now));

Console.WriteLine(instance);
Console.WriteLine(instance.Name);
```

Example output:

```text
{ Id = 1, Name = Text1, Date = 11/12/2026 10:20:35 }
Text1
```

You can also create an instance first and assign values later:

```csharp
dynamic person = type.CreateInstance();
person.Id = 2;
person.Name = "Text2";
```

## Why use it?

This library is a compact way to build runtime-shaped objects when you do not want to define a concrete model class up front. It is especially handy for:

- data mapping and transformations
- temporary DTOs
- scaffolding code or tests
- dynamic serialization scenarios

## Example project

See the sample app in [Examples/Program.cs](https://github.com/mustaddon/DynamicAnonymousType/tree/main/Examples/Program.cs).

## License

This project is licensed under the MIT License.