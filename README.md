# 🧩 Fluent Compare

**Fluent Compare** is a flexible, type-safe comparison library for .NET that lets you compare two or more sets of values using a **fluent API**.

It supports primitive types, arrays, and complex objects — and even provides advanced features like:
- Bitwise operations for byte comparisons
- Recursive comparison of nested objects
- Configurable depth limits
- Null-handling and custom comparison rules
- Fluent configuration for clear, expressive test or runtime comparisons

## Why Fluent Compare?

- **Single, consistent API** for primitives, arrays, and complex object graphs
- **Detailed diagnostics** (`ComparisonResult`) with mismatches, errors, and warnings objects
- **Configurable behavior** for comparison type, string rules, float tolerance, depth, and finish mode
- **Practical object comparison features** like reference/property mode and excluded types
- **Open-source friendly** (Apache 2.0)

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
  - `TypesExcludedFromComparison`
    - compared object properties of excluded types are skipped
    - supports exact type and assignable/base-type matching
- Aggregation options:
  - `FinishComparisonOnFirstMismatch()`
  - `FinishComparisonCollectingAllMismatches()`
- Configuration plumbing:
  - `UseConfiguration(ComparisonConfiguration)`
  - `Configure(Action<ComparisonConfiguration>)`
  - static overload: `ComparisonBuilder.Compare(t1, t2, comparisonConfiguration)`

### Notes

- Some exposed APIs are still being aligned across all comparison paths.
- For detailed coverage status and migration progress, see `docs/UnitTestingGuide.md`.

**Special support for byte arrays:**
- Bitwise operations like AND, OR, XOR, shifts, etc., can be applied before comparison

## Comparison with other libraries

### ⚡ Performance Benchmarks

`FluentCompare` is designed to be efficient, even for complex or nested object comparisons.  
The following benchmark compares `FluentCompare` against other popular .NET object comparison libraries.

Latest run (`BenchmarkDotNet v0.15.8`, `.NET 8.0.27`, `Intel i7-1185G7`, Windows 11):

| Method                        | DataSet | Mean          | Gen0      | Gen1     | Gen2    | Allocated  |
|------------------------------ |-------- |--------------:|----------:|---------:|--------:|-----------:|
| CompareWith_FluentComparison  | SetA    |      1.327 μs |    0.5302 |        - |       - |    3.25 KB |
| CompareWith_CompareNetObjects | SetA    |      6.237 μs |    1.9073 |   0.0687 |       - |   11.72 KB |
| CompareWith_AnyDiff           | SetA    |    510.511 μs |   27.3438 |   1.9531 |       - |  175.68 KB |
| CompareWith_FluentComparison  | SetB    |  7,035.112 μs |  351.5625 | 164.0625 | 85.9375 | 2383.06 KB |
| CompareWith_CompareNetObjects | SetB    | 15,635.812 μs | 1093.7500 |  31.2500 |       - | 6726.81 KB |
| CompareWith_AnyDiff           | SetB    |  1,872.770 μs |   93.7500 |   7.8125 |       - |  596.75 KB |

**Observations:**

- `FluentCompare` is competitive with other fast object comparison libraries.
- Memory usage remains low even with nested objects.

Run benchmarks locally:

```powershell
dotnet run --project .\test\FluentCompare.Benchmarks\FluentCompare.Benchmarks.csproj -c Release
```

### License

⚠️ Note: CompareNetObjects is not entirely free for commercial use.
FluentCompare is fully open-source under the Apache 2.0 license.

## Documentation

See [ubiquitous language document](./docs/UbiquitousLanguage.md)

## License

Library uses [Apache 2.0 license](./LICENSE)