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

### 🔢 Compare doubles with precision
```csharp
var result = ComparisonBuilder.Create()
    .WithDoublePrecision(4)
    .Compare(1.23456, 1.23459);

Console.WriteLine(result.AllMatched); // True
```

Or using epsilon precision:

```csharp
var result = ComparisonBuilder.Create()
    .WithDoublePrecision(1e-5) // epsilon
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

🔧 Example 2: Reference comparison

```csharp
var result = ComparisonBuilder.Create()
    .UseReferenceEquality()
    .Compare(person1, person2);

Console.WriteLine(result.AllMatched); // False, different object references
```

## 🧪 Supported Types

`FluentCompare` can compare the following types:

**Primitive types:**
- *numeric*: `int`, `long`, `short`, `byte`
- *floating point*: `double`, `float`, `Half`, `NFloat`, `decimal` (with configurable precision or epsilon tolerance)
- `bool`
- `string`

**Arrays:**
- Arrays of any supported primitive type, e.g. `int[]`, `double[]`, `string[]`
- Nested arrays are supported, e.g. `int[][]`, `object[][]`

**Complex types:**
- Classes, structs, and records
- Objects containing primitive types, arrays, or other complex types
- Supports recursive property-by-property comparison and reference equality comparison

## ⚙️ Configuration options (implemented and test-covered)

The following options are implemented and covered by unit tests:

- `UseComparisonType(...)`
  - `EqualTo`, `NotEqualTo`, `GreaterThan`, `LessThan`, `GreaterThanOrEqualTo`, `LessThanOrEqualTo`
- `UseStringComparisonType(...)`
  - including case-sensitive and case-insensitive modes (for example `Ordinal`, `OrdinalIgnoreCase`)
- Floating-point tolerance configuration:
  - `WithDoublePrecision(int roundingPrecision)`
  - `WithDoublePrecision(double epsilonPrecision)`
  - `UseDoubleToleranceMethod(...)`
- Byte bitwise configuration:
  - `ApplyBitwiseOperation(...)` (all overloads)
- Null-handling options:
  - `AllowNullComparison()` / `DisallowNullComparison()`
  - `AllowNullsInArguments()` / `DisallowNullsInArguments()`
- Complex-type comparison options:
  - `UsePropertyEquality()` / `UseReferenceEquality()`
  - `UseComplexTypeComparisonMode(...)`
  - `SetComparisonDepth(...)`
- Aggregation options:
  - `FinishComparisonOnFirstMismatch()`
  - `FinishComparisonCollectingAllMismatches()`
- Configuration plumbing:
  - `UseConfiguration(ComparisonConfiguration)`
  - `Configure(Action<ComparisonConfiguration>)`
  - static overload: `ComparisonBuilder.Compare(t1, t2, comparisonConfiguration)`

### Notes

- Some exposed APIs are still being aligned across all comparison paths (for example, some array-length policy scenarios).
- For detailed coverage status and migration progress, see `docs/UnitTestingGuide.md`.

**Special support for byte arrays:**
- Bitwise operations like AND, OR, XOR, shifts, etc., can be applied before comparison

## Comparison with other libraries

### ⚡ Performance Benchmarks

`FluentCompare` is designed to be efficient, even for complex or nested object comparisons.  
The following benchmark compares `FluentCompare` against other popular .NET object comparison libraries:

| Method                        | Mean       | Error      | StdDev     | Gen0    | Gen1    | Allocated |
|-------------------------------|-----------:|-----------:|-----------:|--------:|--------:|----------:|
| CompareWith_FluentComparison  |   7.603 μs |  2.7598 μs |  1.4434 μs |  2.7161 |       - |  16.76 KB |
| CompareWith_CompareNetObjects |   6.323 μs |  0.2566 μs |  0.1342 μs |  2.6398 |  0.0992 |   16.2 KB |
| CompareWith_AnyDiff           | 402.616 μs | 57.0189 μs | 25.3167 μs | 70.3125 | 15.6250 | 434.91 KB |

**Observations:**

- `FluentCompare` is competitive with other fast object comparison libraries.
- Memory usage remains low even with nested objects.
- Libraries like `AnyDiff` are significantly slower and allocate much more memory.

### License

⚠️ Note: CompareNetObjects is not entirely free for commercial use.
FluentCompare is fully open-source under the Apache 2.0 license.

## Documentation

See [ubiquitous language document](./docs/UbiquitousLanguage.md)

## License

Library uses [Apache 2.0 license](./LICENSE)