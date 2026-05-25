# Unit Testing Guide for FluentCompare

This document defines a **strict, generic unit testing convention** for all compared types in `FluentCompare.UnitTests`.

It is intended to be used as both:

- a contributor contract for new tests,
- a migration checklist to align existing tests.

## 1. Testing stack and style

- Framework: xUnit (`[Theory]`, `[MemberData]`, `TheoryData`)
- Assertions: Shouldly
- Pattern: **data-driven first**

### Mandatory style rules

1. Prefer `[Theory]` + `TheoryData` for all matrix-like scenarios.
2. Keep assertions uniform:
   - `MismatchCount`
   - `ErrorCount`
   - optional first code (`Mismatch` or `Error`)
3. Use configurator delegates in rows when configuration affects behavior:
   - `Func<ComparisonBuilder, ComparisonBuilder>`
4. Use one test method per `ComparisonBuilder` entry point.
5. When caller argument expressions affect messages, include both:
   - named-variable invocation (for example: `Compare(leftValue, rightValue)`), and
   - literal/direct invocation (for example: `Compare(1, 2)`).
   - If literal invocation is a single/special case, use a separate explicit `[Fact]`.
6. Always log comparison output using `ITestOutputHelper`, including mismatch/error/warning codes and messages.
7. Logging must always include `ComparisonResult.ToString()` output (single source summary) written to `ITestOutputHelper`.

---

## 2. Strict file convention (required)

For each type token {Type} there must be exactly **two** test files in its folder:

1. {Type}ComparisonTests.cs
2. {Type}ArrayComparisonTests.cs

### Responsibility split

### TypeComparisonTests.cs covers

- scalar pair: `Compare(T left, T right)`
- params scalar: `Compare(params T[] values)`
- object overload for scalar/nullable scalar where applicable

### TypeArrayComparisonTests.cs covers

- array pair: `Compare(T[] left, T[] right)`
- params jagged: `Compare(params T[][] values)`
- nullable array pair (value types): `Compare(T?[] left, T?[] right)`
- nullable jagged params (value types): `Compare(params T?[][] values)`

---

## 3. Strict method naming convention (required)

Use exactly the following naming template (replace {Type} with concrete type token, e.g., Byte, Int, String):

### In TypeComparisonTests.cs

- Compare_{Type}Pair_UsesExpectedOutcome
- Compare_Params{Type}_UsesExpectedOutcome
- Compare_ObjectOverload_Nullable{Type}_UsesExpectedOutcome *(value types / when applicable)*

### In TypeArrayComparisonTests.cs

- Compare_{Type}ArrayPair_UsesExpectedOutcome
- Compare_{Type}ArrayParams_UsesExpectedOutcome
- Compare_Nullable{Type}ArrayPair_UsesExpectedOutcome *(value types)*
- Compare_Nullable{Type}ArrayParams_UsesExpectedOutcome *(value types)*

If a case is not applicable (e.g., nullable reference scalar), omit the method and document N/A in status.

---

## 4. Strict TheoryData convention (required)

Each method above should have a matching `TheoryData` property.

### Canonical shapes

#### Scalar pair with configurable behavior

`TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, T, T, int, int, string?>`

#### Params scalar

`TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, T[]?, int, int, string?>`

#### Array pair

`TheoryData<T[]?, T[]?, int, int, string?>`

#### Params jagged

`TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, T[][]?, int, int, string?>`

#### Nullable array pair (value types)

`TheoryData<T?[]?, T?[]?, bool useObjectOverload, int, int, string?>`

#### Nullable jagged params (value types)

`TheoryData<Func<ComparisonBuilder, ComparisonBuilder>, T?[][]?, int, int, string?>`

---

## 5. Required scenario matrix per entry point

Each applicable entry point must include rows for:

- Equal values/arrays
- Mismatch values/arrays
- Null argument (where possible)
- NotEnoughObjectsToCompare (`params` with less than 2)
- InputArrayLengthsDiffer (pair arrays)
- Null inner array/null element (jagged/nullable)
- `AllowNullComparison` and `DisallowNullComparison()` behavior
- ComparisonType behavior (`EqualTo`, `NotEqualTo`, `GreaterThan`, `LessThan`, etc.)
- Type-specific config:
  - Byte: bitwise operations
  - String: `StringComparison`
  - Double: precision/tolerance
- Caller argument expression variants (where message text depends on argument form):
  - named variables, and
  - literals/direct values.

For primitive and numeric types, the scalar pair and array-pair entry points must include, for every `ComparisonType` value, at least one match case and one not-match case.

For the integer family, apply this to all supported integer widths/signs (not only `int`), including `short`, `long`, and unsigned variants where supported by the API.

### Message-validation requirement

If the compared entry point includes expression names in generated messages, tests must validate message behavior for both invocation forms:

- Named variables: assert message contains source variable names.
- Literals/direct values: assert message follows expected non-variable form (for example default labels, literal formatting, or simply code + count if exact text is unstable).

Recommended pattern:

- Keep the main matrix data-driven for normal scenarios.
- Add one dedicated `[Fact]` for literal/direct invocation when it would otherwise require a mostly-false boolean flag in theory rows.

---

## 6. Shared helper convention per test class

Each class should provide:

- `CreateBuilder(...)`
- `AssertFirstMismatchCode(...)`
- `AssertFirstErrorCode(...)`

This guarantees consistent diagnostics and assertion style.

### Required logging behavior

The test class should log comparison details in assertion helpers (or equivalent local helper methods), and must write:

1. `result.ToString()` *(mandatory; use `ComparisonResult.ToString()` as the baseline log output)*
2. each mismatch code + message
3. each error code + message
4. each warning code + message

This ensures test output contains actionable diagnostics in CI and local runs.

---

## 7. Current implementation status vs strict convention

### Legend
- ✅ Aligned with strict convention
- 🟡 Partially aligned
- ❌ Not aligned
- ⏭️ N/A

| Type | Scalar file exists | Array file exists | Method naming aligned | TheoryData-first | Status |
|---|---:|---:|---:|---:|---|
| Bool | ✅ `BoolComparisonTests.cs` | ✅ `BoolArrayComparisonTests.cs` | ✅ | ✅ | ✅ |
| Byte | ✅ `ByteComparisonTests.cs` | ✅ `ByteArrayComparisonTests.cs` | ✅ | ✅ | ✅ |
| Int (`int`) | ✅ `IntComparisonTests.cs` | ✅ `IntArrayComparisonTests.cs` | ✅ | ✅ | ✅ |
| Short (`short`) | ✅ `ShortComparisonTests.cs` | ✅ `ShortArrayComparisonTests.cs` | ✅ | ✅ | ✅ |
| Long (`long`) | ✅ `LongComparisonTests.cs` | ✅ `LongArrayComparisonTests.cs` | ✅ | ✅ | ✅ |
| Double | ✅ `DoubleComparisonTests.cs` | ❌ | ❌ | ❌ (mostly Fact-based) | ❌ |
| String | ✅ `StringComparisonTests.cs` | ✅ `StringArrayComparisonTests.cs` | ✅ | ✅ | ✅ |
| Object/Complex | 🟡 (multiple split-by-shape files) | 🟡 | ❌ | 🟡 | 🟡 |

### Explicit known gap still present

- `test/FluentCompare.UnitTests/Objects/ParamsObjectsComparisonTests.cs`
  - `Compare_ParamsObject_ShouldReturnError` is `[Theory(Skip = "Not implemented")]`.

---

## 8. Mandatory migration backlog (to become fully strict)

1. **Doubles**
   - Split into:
     - `DoubleComparisonTests.cs` (scalar/params scalar/object scalar)
     - `DoubleArrayComparisonTests.cs` (array pair/params jagged/nullable arrays if supported)
   - Convert repeated `[Fact]` cases into `TheoryData` matrices.

2. **Objects**
   - Define explicit type tokens for object comparison families (e.g., `Object`, `ObjectClass`, `ObjectAnonymous`) and enforce the 2-file rule within each family.

---

## 9. Existing file inventory (for traceability)

### Bools
- `test/FluentCompare.UnitTests/Bools/BoolComparisonTests.cs`
- `test/FluentCompare.UnitTests/Bools/BoolArrayComparisonTests.cs`

### Bytes
- `test/FluentCompare.UnitTests/Bytes/ByteComparisonTests.cs`
- `test/FluentCompare.UnitTests/Bytes/ByteArrayComparisonTests.cs`

### Integers
- `test/FluentCompare.UnitTests/Integers/IntComparisonTests.cs`
- `test/FluentCompare.UnitTests/Integers/IntArrayComparisonTests.cs`
- `test/FluentCompare.UnitTests/Integers/ShortComparisonTests.cs`
- `test/FluentCompare.UnitTests/Integers/ShortArrayComparisonTests.cs`
- `test/FluentCompare.UnitTests/Integers/LongComparisonTests.cs`
- `test/FluentCompare.UnitTests/Integers/LongArrayComparisonTests.cs`

### Doubles
- `test/FluentCompare.UnitTests/Doubles/DoubleComparisonTests.cs` *(currently mixed scalar+array, Fact-heavy)*

### Strings
- `test/FluentCompare.UnitTests/Strings/StringComparisonTests.cs`
- `test/FluentCompare.UnitTests/Strings/StringArrayComparisonTests.cs`

### Nullability and Objects (cross-cutting)
- `test/FluentCompare.UnitTests/Nullability/NullabilityTests.cs`
- `test/FluentCompare.UnitTests/Objects/AnonymousObjectsComparisonTests.cs`
- `test/FluentCompare.UnitTests/Objects/ClassArrayComparisonTests.cs`
- `test/FluentCompare.UnitTests/Objects/ClassComparisonTests.cs`
- `test/FluentCompare.UnitTests/Objects/ComplexClassTestsUsingBogus.cs`
- `test/FluentCompare.UnitTests/Objects/ParamsObjectsComparisonTests.cs`

---

## 10. Quick checklist for adding a new type

When adding support for a new primitive or comparable type:

1. Add two files:
   - {Type}ComparisonTests.cs
   - {Type}ArrayComparisonTests.cs
2. Add required methods per section 3.
3. Add matching `TheoryData` per section 4.
4. Add scenario rows per section 5.
5. Ensure object-overload coverage where relevant.
6. Ensure configuration-specific rows where relevant.
7. Run full test suite and update status table in section 7.

---

## 11. Additional type-specific test ideas (current + future)

This section defines extra, type-focused scenarios to improve confidence beyond the baseline matrix in section 5.

## Bools

- Cross-check all `ComparisonType` semantics against a simple truth table.
- Validate stable message wording for all mismatch outcomes.
- Validate nullable-object overload behavior when one side is `null` and nulls are disallowed.

## Bytes

- Bitwise operation composition order (e.g., AND->XOR vs XOR->AND) should produce distinct outcomes.
- Boundary bit patterns (`0x00`, `0xFF`, alternating bits) for each bitwise operation.
- Shift behavior with shift count edge values (`0`, `7`, `8`, very large values).
- Future: if per-index bitwise configuration is added, validate mixed per-index masks in one array.

## Integers

- Full signed range edge checks (`int.MinValue`, `int.MaxValue`) for all comparison types.
- Overflow-adjacent comparisons (without overflow operations) to verify ordering logic only.
- Mirror the full baseline matrix for all integer-family types (at minimum: `short`, `int`, `long`; plus unsigned types if/when exposed by `ComparisonBuilder`).
- Include type-specific edge values for each integral type (`MinValue`, `MaxValue`, zero, negative/positive boundaries where applicable).
- Future: if tolerance/range-window comparison is added for integer families, validate inclusive/exclusive boundaries.

## Doubles / Floating-point

- `NaN`, `+Infinity`, `-Infinity`, `-0.0` vs `0.0` handling per comparison type.
- Rounding mode coverage by precision and tie-breaking boundaries.
- Epsilon mode around threshold boundaries (`diff < eps`, `diff == eps`, `diff > eps`).
- Future: if ULP-based tolerance is introduced, add ULP-distance boundary tests.

## Strings

- Culture-sensitive cases (e.g., Turkish-I style casing) across configured `StringComparison` modes.
- Unicode normalization differences (composed vs decomposed forms).
- Whitespace and control character differences (`\t`, `\r\n`, zero-width characters).
- Future: if optional normalization/trim rules are introduced, validate enabled vs disabled behavior.

## Arrays (all primitive/reference types)

- Empty array vs empty array behavior for pair and params forms.
- Very large arrays for aggregation behavior and deterministic mismatch indexing.
- Multiple mismatches in one run: verify ordering and count consistency.
- Future: if early-stop mode is introduced, validate first-mismatch short-circuit behavior.

## Nullable arrays and jagged arrays

- Mixed null placements in jagged structures (null row, null element, both sides null).
- Validate `DisallowNullComparison()` escalation path (mismatch->error expectations by API contract).
- Future: if per-level null policy is introduced, validate root vs inner-level null-policy precedence.

## Object / Complex types

- Recursive graphs with cycles (if supported) and depth limiting behavior.
- Property order independence and ignored-property configuration (if introduced).
- Reference-equality vs property-equality mode deltas for identical value objects.
- Future: if custom property comparers are introduced, validate comparer precedence and fallback.

## Cross-cutting / diagnostics

- Snapshot tests for `ComparisonResult.ToString()` output shape (summary + sections).
- Verify mismatch/error/warning codes remain stable (contract tests for public error-code surface).
- Verify caller-argument-expression rendering for named variables, literals, and complex expressions.
- Future: if localization is introduced, validate code stability independent from message language.
