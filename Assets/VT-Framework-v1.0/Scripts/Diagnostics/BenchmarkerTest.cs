using UnityEngine;
using VT.Diagnostics;
using VT.Math;

public class BenchmarkerTest : MonoBehaviour
{
    public ulong range = 2;
    public int iterations = 1;
    void Start()
    {
        //Debug.Log("Atkin");
        //Benchmarker.Benchmark(GetPrimesAtkin, iterations);
        //Debug.Log("Eratosthenes with culling");
        //Benchmarker.Benchmark(GetPrimesEratosthenes, iterations);
        Debug.Log("Eratosthenes with culling and multi threaded");
        Benchmarker.Benchmark(GetPrimesEratosthenesMultiThreaded, iterations);
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