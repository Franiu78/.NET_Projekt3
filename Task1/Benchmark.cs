using System.Diagnostics;

namespace Task1;

public sealed record BenchmarkRow(int Threads, double AverageMilliseconds, Matrix Result);

// Uruchamia wiele prób i zwraca średni czas dla każdej konfiguracji wątków.
public static class Benchmark
{
    // Uruchamia wiele prób i zwraca średni czas dla każdej konfiguracji wątków.
    public static List<BenchmarkRow> Run(
        Matrix left,
        Matrix right,
        int[] threadCounts,
        int repetitions,
        Func<Matrix, Matrix, int, Matrix> multiply)
    {
        if (repetitions <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(repetitions));
        }

        var results = new List<BenchmarkRow>();

        foreach (var threads in threadCounts)
        {
            var stopwatchTotal = 0.0;
            Matrix? lastResult = null;

            for (var run = 0; run < repetitions; run++)
            {
                var stopwatch = Stopwatch.StartNew();
                lastResult = multiply(left, right, threads);
                stopwatch.Stop();

                stopwatchTotal += stopwatch.Elapsed.TotalMilliseconds;
            }

            results.Add(new BenchmarkRow(threads, stopwatchTotal / repetitions, lastResult!));
        }

        return results;
    }
}