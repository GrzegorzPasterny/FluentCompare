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
        new[] { 1, 2, 3 }, // all arrays are compared against the first one
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

## 📦 Understanding `ComparisonResult`

Every comparison returns a **structured** `ComparisonResult` object (not just a boolean), so you can make decisions in tests, runtime validation, pipelines, or logs.

`ComparisonResult` contains:

- `AllMatched` — `true` when there are no mismatches and no errors
- `WasSuccessful` — `true` when there are no errors
- `MismatchCount`, `ErrorCount`, `WarningCount`
- `Mismatches` (`IReadOnlyList<ComparisonMismatch>`)
- `Errors` (`IReadOnlyList<ComparisonError>`)
- `Warnings` (`IReadOnlyList<ComparisonError>`)

Example:

```csharp
var result = ComparisonBuilder.Create()
    .UseComparisonType(ComparisonType.EqualTo)
    .Compare(new[] { 1, 2, 3 }, new[] { 1, 9, 3 });

if (!result.AllMatched)
{
    foreach (var mismatch in result.Mismatches)
    {
        Console.WriteLine($"{mismatch.Code}: {mismatch.Message}");
    }
}
```

### Example messages and non-obvious cases

Typical mismatch/error/warning messages include:

- `FluentCompare.Mismatch.Int32.MismatchDetected: ...`
- `FluentCompare.NullPassedAsArgument: Null value was provided for comparison [VariableName = ..., Type = ...]`
- `FluentCompare.InputArrayLengthsDiffer: Array lengths differ [...]`

Non-obvious behavior worth knowing:

- `AllMatched` can be `true` **with warnings** (for example, when both objects are null and null comparison is allowed).
- With `AllowArrayComparisonOfDifferentLengths = true`, a length warning is emitted and shared-prefix elements are still compared.
- `WasSuccessful` is based on errors only; mismatches make `AllMatched` false but do not make `WasSuccessful` false.

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

`FluentCompare` is configured through `ComparisonConfiguration` (directly, or via fluent builder methods).

### Top-level configuration defaults

| Parameter | Default | What it does |
|---|---:|---|
| `ComparisonType` | `EqualTo` | Primitive comparison operator (`EqualTo`, `NotEqualTo`, `GreaterThan`, etc.) |
| `ComplexTypesComparisonMode` | `PropertyEquality` | Compare complex objects by recursive property values or by reference |
| `AllowNullComparison` | `true` | If false, null-vs-null and one-null cases produce errors/mismatches instead of permissive behavior |
| `AllowNullsInArguments` | `true` | If false, null inputs are treated as errors |
| `AllowArrayComparisonOfDifferentLengths` | `false` | If true, length difference produces warning; if false, mismatch |
| `MaximumComparisonDepth` | `5` | Recursion depth limit for complex object graphs |
| `FinishComparisonOnFirstMismatch` | `false` | If true, short-circuit on first mismatch; if false, collect all mismatches |
| `TypesExcludedFromComparison` | empty list | Excludes matching property/runtime types in object recursion (exact or assignable type match) |

### Nested configuration defaults

| Configuration | Parameter | Default | Meaning |
|---|---|---:|---|
| `StringConfiguration` | `StringComparisonType` | `Ordinal` | Controls case/culture behavior for string comparison |
| `FloatConfiguration` | `RoundingPrecision` | `15` | Decimal precision for rounding-based float comparison |
| `FloatConfiguration` | `EpsilonPrecision` | `1e-17` | Epsilon threshold for epsilon-based float comparison |
| `FloatConfiguration` | `ToleranceMethod` | `Rounding` | Float tolerance strategy (`Rounding` or `Epsilon`) |
| `ByteConfiguration` | `BitwiseOperations` | empty list | Optional pre-compare bitwise transformations for byte values |

### Fluent API mapping

- `UseComparisonType(...)`
- `UseStringComparisonType(...)`
- `WithDoublePrecision(int)` / `WithDoublePrecision(double)` / `UseDoubleToleranceMethod(...)`
- `ApplyBitwiseOperation(...)`
- `AllowNullComparison()` / `DisallowNullComparison()`
- `AllowNullsInArguments()` / `DisallowNullsInArguments()`
- `AllowArrayComparisonOfDifferentLengths()` / `DisallowArrayComparisonOfDifferentLengths()`
- `UsePropertyEquality()` / `UseReferenceEquality()` / `UseComplexTypeComparisonMode(...)`
- `SetComparisonDepth(...)`
- `FinishComparisonOnFirstMismatch()` / `FinishComparisonCollectingAllMismatches()`
- `UseConfiguration(ComparisonConfiguration)` / `Configure(Action<ComparisonConfiguration>)`
- Static overload: `ComparisonBuilder.Compare(t1, t2, comparisonConfiguration)`

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
| CompareWith_FluentComparison  | SetA    |      1.257 μs |    0.5302 |        - |       - |    3.25 KB |
| CompareWith_CompareNetObjects | SetA    |      3.820 μs |    1.9073 |   0.0687 |       - |   11.72 KB |
| CompareWith_AnyDiff           | SetA    |    233.289 μs |   28.3203 |   2.9297 |       - |  175.42 KB |
| CompareWith_FluentComparison  | SetB    |  1,763.205 μs |  257.8125 | 171.8750 | 85.9375 | 1521.18 KB |
| CompareWith_CompareNetObjects | SetB    |  6,678.865 μs | 1093.7500 |   7.8125 |       - |  6719.7 KB |
| CompareWith_AnyDiff           | SetB    |    597.523 μs |   95.7031 |   3.9063 |       - |  595.77 KB |

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