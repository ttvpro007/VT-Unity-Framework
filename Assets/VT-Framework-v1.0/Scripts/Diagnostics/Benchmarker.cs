using System;
using System.Diagnostics;
using VT.Extensions;

namespace VT.Diagnostics
{
    public static class Benchmarker
    {
        public static void Benchmark(Action action, int iterations, string description = "")
        {
            GC.Collect();
            action?.Invoke(); // invoke once to avoid initialization cost
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < iterations; i++)
            {
                action?.Invoke();
            }
            stopwatch.Stop();
            UnityEngine.Debug.Log(
                $"{description}\n" +
                $"Total iterations: {iterations.ToShortenForm()}\n" +
                $"Individual time elapsed: {stopwatch.ElapsedMilliseconds / iterations} ms\n" +
                $"Total time elapsed: {stopwatch.ElapsedMilliseconds} ms\n");
        }
    }
}