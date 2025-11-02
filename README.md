# 🧩 Fluent Compare

**Fluent Compare** is a flexible, type-safe comparison library for .NET that lets you compare two or more sets of values using a **fluent API**.

It supports primitive types, arrays, and complex objects — and even provides advanced features like:
- Bitwise operations for byte comparisons
- Recursive comparison of nested objects
- Configurable depth limits
- Null-handling and custom comparison rules
- Fluent configuration for clear, expressive test or runtime comparisons

---

## 🚀 Quick Start

### ✨ Compare simple values

```csharp
var result = ComparisonBuilder.Create()
    .Compare(1, 1);

Console.WriteLine(result.AllMatched); // True
```

### 🔢 Compare arrays

```csharp
var result = ComparisonBuilder.Create()
    .Compare(new[] { 1, 2, 3 }, new[] { 1, 2, 3 });

Console.WriteLine(result.AllMatched); // True
```

### 🧮 Compare multiple arrays at once
```csharp
var result = ComparisonBuilder.Create()
    .Compare(
        new[] { 1, 2, 3 },
        new[] { 1, 2, 3 },
        new[] { 1, 2, 3 }
    );

Console.WriteLine(result.AllMatched); // True
```

### ⚖️ Use a different comparison type
```csharp
var result = ComparisonBuilder.Create()
    .UseComparisonType(ComparisonType.GreaterThan)
    .Compare(new[] { 2, 3, 4 }, new[] { 1, 2, 3 });

Console.WriteLine(result.AllMatched); // True
```

###  Compare with rounding precision to 4 decimal places
```csharp
var result = ComparisonBuilder.Create()
    .WithDoublePrecision(4)
    .Compare(1.23456, 1.23459);

Console.WriteLine(result.AllMatched); // True
```

## 🧩 Comparing Complex Types

`FluentCompare` can compare **complex objects** (classes, structs, or records) that contain primitive or nested types.  
You control how deep and how precisely this comparison is performed using `ComplexTypesComparisonMode` and related configuration options.

---

### 🔧 Example 1: Property-by-property comparison

By default, `FluentCompare` compares objects by recursively comparing each of their public properties.

```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

var person1 = new Person { Name = "Alice", Age = 30 };
var person2 = new Person { Name = "Alice", Age = 30 };

var result = ComparisonBuilder.Create()
    .Compare(person1, person2);

Console.WriteLine(result.AllMatched); // True
```

## Documentation

See [ubiquitous language document](./docs/UbiquitousLanguage.md)

## License

Library uses [Apache 2.0 license](./LICENSE)