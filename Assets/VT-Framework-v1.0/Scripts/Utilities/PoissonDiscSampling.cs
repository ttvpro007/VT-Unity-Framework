using System;
using System.Collections.Generic;
using UnityEngine;
using VT.Extensions;

namespace VT.Utilities
{
    //https://www.cs.ubc.ca/~rbridson/docs/bridson-siggraph07-poissondisk.pdf
    public static class PoissonDiscSampling
    {
        public static List<Vector2> GetSamples2D(float radius, Vector2 dimensionSize, int sampleLimit = 1000, int samplingLimit = 30)
        {
            radius = Mathf.Max(0.1f, radius);

            // Step 0
            Grid2D<int> grid = new Grid2D<int>(dimensionSize, radius / Mathf.Sqrt(2), -1);

            List<Vector2> activeList = new List<Vector2>();
            List<Vector2> sampleList = new List<Vector2>();
            Action<Vector2> addSample = sample =>
            {
                sampleList.Add(sample);
                activeList.Add(sample);
                grid[sample] = sampleList.Count - 1;
                sampleLimit--;
            };

            addSample(dimensionSize / 2f);

            while (!activeList.IsNullOrEmpty() && sampleLimit > 0)
            {
                int randomActivePointIndex = UnityEngine.Random.Range(0, activeList.Count);
                bool accepted = false;
                for (int i = 0; i < samplingLimit; i++)
                {
                    Vector2 candidate = activeList[randomActivePointIndex] + UnityEngine.Random.insideUnitCircle.normalized * radius;
                    if (grid.Contains(candidate))
                    {
                        bool isValid = true;
                        foreach (var neighbor in grid.GetNeighborsOf(candidate, 1))
                        {
                            if (neighbor != -1 && (candidate - sampleList[neighbor]).sqrMagnitude < radius * radius)
                            {
                                isValid = false;
                                break;
                            }
                        }

                        if (isValid)
                        {
                            addSample(candidate);
                            accepted = true;
                            break;
                        }
                    }
                }

                if (!accepted)
                {
                    activeList.RemoveAt(randomActivePointIndex);
                }
            }

            return sampleList;
        }

        public static List<Vector3> GetSamples3D(float radius, Vector3 dimensionSize, int sampleLimit = 1000, int samplingLimit = 30)
        {
            radius = Mathf.Max(0.1f, radius);

            // Step 0
            Grid3D<int> grid = new Grid3D<int>(dimensionSize, radius / Mathf.Sqrt(2), -1);

            List<Vector3> activeList = new List<Vector3>();
            List<Vector3> sampleList = new List<Vector3>();
            Action<Vector3> addSample = sample =>
            {
                sampleList.Add(sample);
                activeList.Add(sample);
                grid[sample] = sampleList.Count - 1;
                sampleLimit--;
            };

            addSample(dimensionSize / 2f);

            while (!activeList.IsNullOrEmpty() && sampleLimit > 0)
            {
                int randomActivePointIndex = UnityEngine.Random.Range(0, activeList.Count);
                bool accepted = false;
                for (int i = 0; i < samplingLimit; i++)
                {
                    Vector3 candidate = activeList[randomActivePointIndex] + UnityEngine.Random.insideUnitSphere.normalized * radius;
                    if (grid.Contains(candidate))
                    {
                        bool isValid = true;
                        foreach (var neighbor in grid.GetNeighborsOf(candidate, 1))
                        {
                            if (neighbor != -1 && (candidate - sampleList[neighbor]).sqrMagnitude < radius * radius)
                            {
                                isValid = false;
                                break;
                            }
                        }

                        if (isValid)
                        {
                            addSample(candidate);
                            accepted = true;
                            break;
                        }
                    }
                }

                if (!accepted)
                {
                    activeList.RemoveAt(randomActivePointIndex);
                }
            }

            return sampleList;
        }
    }
}