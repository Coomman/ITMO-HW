using System;
using System.Linq;
using System.Collections.Generic;

namespace RegressionComputer.Models
{
    public class Vector
    {
        private readonly IList<double> _coords;

        public Vector(IList<double> coords)
        {
            _coords = coords;
        }

        public double ManhattanDistance(Vector other)
            => _coords.Zip(other._coords, (x, y) => Math.Abs(x - y)).Sum();
        public double EuclideanDistance(Vector other)
            => Math.Sqrt(_coords.Zip(other._coords, (x, y) => Math.Pow(x - y, 2)).Sum());
        public double ChebyshevDistance(Vector other)
            => _coords.Zip(other._coords, (x, y) => Math.Abs(x - y)).Max();

        public bool Equals(Vector other)
            => !_coords.Where((coord, i) => coord.CompareTo(other._coords[i]) != 0).Any();
        public static Vector GetNullVector(int size)
            => new Vector(Enumerable.Repeat(0.0, size).ToList());

        public int ArgMax => _coords.ArgMax();
        public int Size => _coords.Count;

        public static Vector operator +(Vector first, Vector second)
            => new Vector(first._coords.Zip(second._coords, (x, y) => x + y).ToList());
        public static Vector operator *(Vector vec, double num)
            => new Vector(vec._coords.Select(coord => coord * num).ToList());
        public static Vector operator /(Vector vec, double num)
            => new Vector(vec._coords.Select(coord => coord / num).ToList());

        public double this[int index] => _coords[index];
    }
}
