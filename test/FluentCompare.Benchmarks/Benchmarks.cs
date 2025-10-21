using BenchmarkDotNet.Attributes;

namespace FluentCompare.Benchmarks;

[MemoryDiagnoser]
[SimpleJob(launchCount: 1, warmupCount: 3, iterationCount: 8)]
public class Benchmarks
{
    private int[] _data;

    [GlobalSetup]
    public void Setup()
    {
        _data = Enumerable.Range(1, 1000).ToArray();
    }

    [Benchmark]
    public int SumWithForLoop()
    {
        int sum = 0;
        for (int i = 0; i < _data.Length; i++)
            sum += _data[i];
        return sum;
    }

    [Benchmark]
    public int SumWithLinq()
    {
        return _data.Sum();
    }
}
