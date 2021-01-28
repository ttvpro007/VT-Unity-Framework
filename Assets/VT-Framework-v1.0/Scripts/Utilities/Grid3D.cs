using System.Collections.Generic;
using UnityEngine;

namespace VT.Utilities
{
    public struct Grid3D<T>
    {
        public Grid3D(Vector3 dimensionSize, float cellSize, T defaultValue)
        {
            Cols   = Mathf.Max(Mathf.CeilToInt(dimensionSize.x / cellSize), 1);
            Rows   = Mathf.Max(Mathf.CeilToInt(dimensionSize.y / cellSize), 1);
            Layers = Mathf.Max(Mathf.CeilToInt(dimensionSize.z / cellSize), 1);
            Count = Cols * Rows * Layers;
            CellSize = cellSize;
            DimensionSize = dimensionSize;
            Elements = new List<T>();
            for (int i = 0; i < Count; i++)
            {
                Elements.Add(defaultValue);
            }
        }

        public T this[int i]
        {
            get => Elements[i];
            set => Elements[i] = value;
        }

        public T this[int ix, int iy, int iz]
        {
            get => Elements[ix + iy * Cols + iz * Cols * Rows];
            set => Elements[ix + iy * Cols + iz * Cols * Rows] = value;
        }

        public T this[float xPos, float yPos, float zPos]
        {
            get => this[(int)(xPos / CellSize), (int)(yPos / CellSize), (int)(zPos / CellSize)];
            set => this[(int)(xPos / CellSize), (int)(yPos / CellSize), (int)(zPos / CellSize)] = value;
        }

        public T this[Vector3 position]
        {
            get => this[position.x, position.y, position.z];
            set => this[position.x, position.y, position.z] = value;
        }

        public bool Contains(int index)
        {
            return index >= 0 && index < Elements.Count;
        }

        public bool Contains(int ix, int iy, int iz)
        {
            return Contains(ix + iy * Cols + iz * Cols * Rows);
        }

        public bool Contains(float xPos, float yPos, float zPos)
        {
            return xPos >= 0
                && yPos >= 0
                && zPos >= 0
                && xPos <= DimensionSize.x
                && yPos <= DimensionSize.y
                && zPos <= DimensionSize.z;
        }

        public bool Contains(Vector3 position)
        {
            return Contains(position.x, position.y, position.z);
        }

        public (int, int, int) Index3DAt(float xPos, float yPos, float zPos)
        {
            return ((int)(xPos / CellSize), (int)(yPos / CellSize), (int)(zPos / CellSize));
        }

        public (int, int, int) Index2DAt(Vector3 position)
        {
            return Index3DAt(position.x, position.y, position.z);
        }

        public int IndexAt(Vector3 position)
        {
            return IndexAt(position.x, position.y, position.z);
        }

        public int IndexAt(float xPos, float yPos, float zPos)
        {
            return (int)(xPos / CellSize) + (int)(yPos / CellSize) * Cols + (int)(zPos / CellSize) * Cols * Rows;
        }

        public List<T> GetNeighborsOf(Vector3 position, int range)
        {
            return GetNeighborsOf(IndexAt(position), range);
        }

        public List<T> GetNeighborsOf(int atIndex, int range)
        {
            List<T> neighbors = new List<T>();
            int layerIndex = atIndex / (Cols * Rows);
            // x + y * cols
            int x_Plus_y_Mul_Cols = atIndex - (layerIndex * Cols * Rows);
            int colIndex = x_Plus_y_Mul_Cols / Cols;
            int rowIndex = x_Plus_y_Mul_Cols % Cols;
            for (int ix = -range; ix <= range; ix++)
            {
                for (int iy = -range; iy <= range; iy++)
                {
                    for (int iz = -range; iz <= range; iz++)
                    {
                        int index = rowIndex + ix + (colIndex + iy) * Cols + (layerIndex + iz) * Cols * Rows;
                        if (index >= 0 && index < Count)
                            neighbors.Add(this[index]);
                    }
                }
            }
            return neighbors;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + DimensionSize.GetHashCode();
                hash = hash * 23 + CellSize.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object other)
        {
            if (!(other is Grid3D<T>)) return false;

            return Equals((Grid3D<T>)other);
        }

        public bool Equals(Grid3D<T> other)
        {
            return DimensionSize == other.DimensionSize
                && CellSize == other.CellSize;
        }

        public int Cols;
        public int Rows;
        public int Layers;
        public int Count;
        public float CellSize;
        public Vector3 DimensionSize;
        public List<T> Elements;
    }
}