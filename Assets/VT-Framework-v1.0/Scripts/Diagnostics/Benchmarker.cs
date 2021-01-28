using System;
using System.Diagnostics;

namespace VT.Diagnostics
{
    public static class Benchmarker
    {
        public static void Benchmark(Action action, int iterations)
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
            UnityEngine.Debug.Log($"Individual time elapsed: {(stopwatch.ElapsedMilliseconds / iterations)} ms\n");
            UnityEngine.Debug.Log($"Total time elapsed:      {stopwatch.ElapsedMilliseconds} ms\n");
        }
    }
}