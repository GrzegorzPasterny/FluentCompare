# Fluent Compare

Fluent compare is a library to compare 2 (or more) sets of values using Fluent API approach.

## Usage examples

*Comparison with default configuration (simple version)*
```
var result = new ComparisonBuilder()
    .Compare([1, 2, 3], [1, 2, 3], [1, 2, 3])
```

*Comparison with default configuration*
```
var result = new ComparisonBuilder()
    .Compare([1, 2, 3], [1, 2, 3]);
```

*Comparison with default configuration for 3 arrays*
```
var result = new ComparisonBuilder()
    .Compare([1, 2, 3], [1, 2, 3], [1, 2, 3])
```

*Check if every value in first array is greater than corresponding value in second array*
```
var result = new ComparisonBuilder()
    .UseComparisonType(ComparisonType.GreaterThan)
    .Compare([2, 3, 4], [1, 2, 3]);
```

## Documentation

See [ubiquitous language document](./docs/UbiquitousLanguage.md)

## License

Library uses [Apache 2.0 license](./LICENSE)