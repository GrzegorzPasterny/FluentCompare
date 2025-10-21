using Bogus;

using FluentCompare.Tests.Shared.Models;

namespace FluentCompare.UnitTests;

public static class TestDataGenerator
{
    public static ClassWithAllSupportedTypes CreateClassWithAllSupportedTypes(int depth = 1)
    {
        if (depth > 2) // prevent infinite recursion
            return null;

        var faker = new Faker<ClassWithAllSupportedTypes>()
            .RuleFor(x => x.Int, f => f.Random.Int(0, 100))
            .RuleFor(x => x.IntArray, f => f.Make(3, () => f.Random.Int(0, 100)).ToArray())
            .RuleFor(x => x.String, f => f.Lorem.Word())
            .RuleFor(x => x.StringArray, f => f.Make(3, () => f.Lorem.Word()).ToArray())
            .RuleFor(x => x.Double, f => f.Random.Double(0, 100))
            .RuleFor(x => x.DoubleArray, f => f.Make(3, () => f.Random.Double(0, 100)).ToArray())
            .RuleFor(x => x.Object, f => f.Random.Word())
            .RuleFor(x => x.ObjectArray, f => f.Make(3, () => (object)f.Random.Word()).ToArray())
            .RuleFor(x => x.NestedClass, _ => CreateClassWithAllSupportedTypes(depth + 1))
            .RuleFor(x => x.NestedClassArray, _ => Enumerable.Range(0, 2)
                                                             .Select(_ => CreateClassWithAllSupportedTypes(depth + 1))
                                                             .ToArray());

        return faker.Generate();
    }
}
