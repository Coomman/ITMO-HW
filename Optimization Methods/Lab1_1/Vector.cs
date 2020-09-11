using System;
using System.Linq;
using System.Collections.Generic;

namespace Lab1_1
{
    public class Vector
    {
        private readonly double[] _coords;

        public Vector(IEnumerable<double> coords)
        {
            _coords = coords.ToArray();
        }

        public static Vector operator +(Vector first, Vector second)
        {
            if (first.Size != second.Size)
                throw new InvalidOperationException("Vectors have different sizes");

            var newCoords = first._coords.Zip(second._coords, (x, y) => x + y);

            return new Vector(newCoords);
        }
        public static Vector operator -(Vector first, Vector second)
        {
            if (first.Size != second.Size)
                throw new InvalidOperationException("Vectors have different sizes");

            var newCoords = first._coords.Zip(second._coords, (x, y) => x - y);

            return new Vector(newCoords);
        }
        public static Vector operator -(Vector vector)
        {
            return new Vector(vector._coords.Select(n => -n));
        }
        public static Vector operator *(Vector vector, double num)
        {
            return new Vector(vector._coords.Select(n => n * num));
        }

        public static Vector GetZeroVector(int size)
        {
            return new Vector(Enumerable.Repeat(0.0, size));
        }

        public int Size => _coords.Length;
        public double Length => Math.Sqrt(_coords.Select(n => n * n).Sum());

        public double this[int i] => _coords[i];

        public Vector Normalized => new Vector(_coords.Select(c => c / Length));

        public override string ToString()
            => $"{{{string.Join("; ", _coords)}}}";
    }
}
