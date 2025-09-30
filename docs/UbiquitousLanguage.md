# Ubiquitous language

1. Comparison returns the comparison result object.
2. If all values within all sets are matching, then the comparison result should not contain any mismatches.
3. Values can be compared as a single value or as a set.
4. A single value can be considered as a set containing one value (set of length 1).
5. Particular values in the sets are compared with one another in a consecutive order (set1[0] compared with set2[0] and so on).
6. If sets don't contain the same number of elements, but all of the values match, the comparison is considered partially matching.
7. Values can be compared in a variety of different types (e.g., `EqualTo`, `GreaterThan`, `NotEqualTo`, ...).
8. The custom comparison type can be defined.
9. It is possible to compare primitive types, as well as complex types.
10. There can be different modes of comparison:
    1. Primitive types:
       1. Strict:
          1. The types of the values must be the same.
       2. Loose:
          1. The types of values don't need to be the same. The comparison engine will try to cast objects to a common type.
    2. Complex types:
       1. Strict:
          1. The type of objects in comparison must be the same.
          2. All of the type's public properties must match.
       2. Loose
          1. The type of objects in comparison might be different.
          2. Only the types' public properties that have the same names and type are taken into account.
11. The comparison can return warnings.
12. The process of comparison is divided into 3 steps:
    1.  Comparison definition - All of the comparison options are set.
    2.  Data input - Data are provided to be compared.
    3.  Run - Comparison runs and returns the comparison result.

## Examples
1. **Single Value Comparison**:
   - Comparing `5` (int) to `5` (int) with `EqualTo` returns a result with no mismatches.
   - Comparing `5` (int) to `"5"` (string) in Loose mode may return a warning due to type mismatch.

2. **Set Comparison**:
   - Comparing `{1, 2, 3}` to `{1, 2, 3}` with `EqualTo` returns no mismatches.
   - Comparing `{1, 2}` to `{1, 2, 3}` may be partially matching if defined as such.

3. **Complex Type Comparison**:
   - Strict: `Person { Name: "John", Age: 30 }` vs. `Person { Name: "John", Age: 30 }` → Matches.
   - Loose: `Person { Name: "John", Age: 30 }` vs. `Employee { Name: "John", Age: 30, Role: "Dev" }` → Matches on shared properties.

