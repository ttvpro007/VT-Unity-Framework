using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VT.Diagnostics;
using VT.Extensions;
using VT.Math;

public class BenchmarkerTest : MonoBehaviour
{
    public ulong range = 2;
    public int iterations = 1;

    [Sirenix.OdinInspector.Button]
    void Start()
    {
        //Debug.Log("Atkin");
        //Benchmarker.Benchmark(GetPrimesAtkin, iterations);
        //Debug.Log("Eratosthenes with culling");
        //Benchmarker.Benchmark(GetPrimesEratosthenes, iterations);
        //Debug.Log("Eratosthenes with culling and multi threaded");
        //Benchmarker.Benchmark(GetPrimesEratosthenesMultiThreaded, iterations);

        var ints = Enumerable.Range(0, (int)range);
        Debug.Log($"Shuffling {range} numbers");

        IEnumerable<int> results = null;

        Debug.Log("Shuffle IEnumerable");
        Benchmarker.Benchmark(() => results = ShuffleIEnumerable(ints), iterations);

        Debug.Log("List: ");
        string display = string.Empty;
        int count = 0;
        foreach (var i in ints)
        {
            count++;
            display += $"[{i}]" + (count % 50 == 0 ? "\n" : " ");
        }
        Debug.Log(display);

        Debug.Log("Result: ");
        display = string.Empty;
        count = 0;
        foreach (var result in results)
        {
            count++;
            display += $"[{result}]" + (count % 50 == 0 ? "\n" : " ");
        }
        Debug.Log(display);
    }

    private IEnumerable<int> ShuffleIEnumerable(IEnumerable<int> ints)
    {
        return ints .Shuffle();
    }

    private void GetPrimesAtkin()
    {
        var a = new SieveOfAtkin((ulong)range);
        Debug.Log($"Total {a.Count} prime numbers in range {range}");
    }

    private void GetPrimesEratosthenes()
    {
        var a = new SieveOfEratosthenes((ulong)range);
        Debug.Log($"Total {a.Count} prime numbers in range {range}");
    }

    private void GetPrimesEratosthenesMultiThreaded()
    {
        var a = SieveOfEratosthenesMultiThreaded.CountTo((ulong)range);
        Debug.Log($"Total {a} prime numbers in range {range}");
    }
}