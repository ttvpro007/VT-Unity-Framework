using System.Collections.Generic;
using UnityEngine;

namespace VT.Utilities
{
    public struct Grid2D<T>
    {
        public Grid2D(Vector2 dimensionSize, float cellSize, T defaultValue)
        {
            Cols = Mathf.Max(Mathf.CeilToInt(dimensionSize.x / cellSize), 1);
            Rows = Mathf.Max(Mathf.CeilToInt(dimensionSize.y / cellSize), 1);
            Count = Cols * Rows;
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

        public T this[int ix, int iy]
        {
            get => Elements[ix + iy * Cols];
            set => Elements[ix + iy * Cols] = value;
        }

        public T this[float xPos, float yPos]
        {
            get => this[(int)(xPos / CellSize), (int)(yPos / CellSize)];
            set => this[(int)(xPos / CellSize), (int)(yPos / CellSize)] = value;
        }

        public T this[Vector2 position]
        {
            get => this[position.x, position.y];
            set => this[position.x, position.y] = value;
        }

        public bool Contains(int index)
        {
            return index >= 0 && index < Elements.Count;
        }

        public bool Contains(int ix, int iy)
        {
            return Contains(ix + iy * Cols);
        }

        public bool Contains(float xPos, float yPos)
        {
            return xPos >= 0
                && yPos >= 0
                && xPos <= DimensionSize.x
                && yPos <= DimensionSize.y;
        }

        public bool Contains(Vector2 position)
        {
            return Contains(position.x, position.y);
        }

        public (int, int) Index2DAt(float xPos, float yPos)
        {
            return ((int)(xPos / CellSize), (int)(yPos / CellSize));
        }

        public (int, int) Index2DAt(Vector2 position)
        {
            return Index2DAt(position.x, position.y);
        }

        public int IndexAt(Vector2 position)
        {
            return IndexAt(position.x, position.y);
        }

        public int IndexAt(float xPos, float yPos)
        {
            return (int)(xPos / CellSize) + (int)(yPos / CellSize) * Cols;
        }

        public List<T> GetNeighborsOf(Vector2 position, int range)
        {
            return GetNeighborsOf(IndexAt(position), range);
        }

        public List<T> GetNeighborsOf(int atIndex, int range)
        {
            List<T> neighbors = new List<T>();
            int colIndex = atIndex / Cols;
            int rowIndex = atIndex % Cols;
            for (int ix = -range; ix <= range; ix++)
            {
                for (int iy = -range; iy <= range; iy++)
                {
                    int index = rowIndex + ix + (colIndex + iy) * Cols;
                    if (index >= 0 && index < Count)
                        neighbors.Add(this[index]);
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
            if (!(other is Grid2D<T>)) return false;

            return Equals((Grid2D<T>)other);
        }

        public bool Equals(Grid2D<T> other)
        {
            return DimensionSize == other.DimensionSize
                && CellSize == other.CellSize;
        }

        public int Cols;
        public int Rows;
        public int Count;
        public float CellSize;
        public Vector2 DimensionSize;
        public List<T> Elements;
    }
}