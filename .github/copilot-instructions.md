# Copilot Instructions

## General Guidelines
- Keep changes minimal and aligned with the existing architecture and layering.
- Follow existing naming patterns and namespaces.
- Prefer existing libraries and patterns already used in the solution.
- Use [Ubiquitous language document](../docs/UbiquitousLanguage.md) as a reference for terminology and concepts.

## Open-source free library
- Keep this library target multiple .NET versions (e.g., .NET Standard 2.0 or .NET 6) to maximize compatibility.

## C#/.NET Conventions
- Target .NET 8 and keep nullable reference types enabled.
- Use block-scoped namespaces and Allman braces to match existing files.
- Keep private/internal fields prefixed with `_` and use `readonly` where appropriate.
- Use XML documentation for public models and APIs when it exists in surrounding code.
- Use `required` for mandatory model properties and nullable annotations (`?`) for optional references.
- Define interfaces with clear, single-purpose method groups and avoid mixing unrelated responsibilities.
- Name interfaces with an `I` prefix and methods with explicit, action-oriented names.
- Prefer async methods returning `Task`/`Task<T>` for I/O or long-running work; keep synchronous interfaces for pure mapping or CPU-bound work.
- Provide optional parameters with sensible defaults (e.g., cancellation tokens defaulted to `default`, optional timeouts as nullable `TimeSpan?`).
- Use XML documentation on interfaces and members to describe intent, parameters, and return values.

## Testing
- Tests use xUnit with Shouldly and Moq.
- Prefer data-driven tests with `MemberData`, or `TheoryData` when applicable.
- Aggregate inputs and outputs into `TheoryData` classes for each `ComparisonBuilder` entry point.
- Include separate cases for `Compare(variableName1, variableName2)` vs `Compare(literal1, literal2)` because caller argument expressions affect mismatch/error messages.
- Always log mismatch/error/warning messages via `ITestOutputHelper` in tests.
