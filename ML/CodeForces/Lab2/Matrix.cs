using System;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Lab2_3
{
    public class Matrix
    {
        private readonly double[][] _matrix;

        public Matrix(double[][] matrix)
        {
            _matrix = matrix;
        }

        public int Width => _matrix.First().Length;
        public int Height => _matrix.Length;
        public string Shape => $"({Height}x{Width})";
        public double[] this[int index] => _matrix[index];

        public Matrix Transpose()
        {
            return MatrixOp(Width, Height, 
                (i, j) => _matrix[j][i]);
        }
        public Matrix Perform(Func<double, double> func)
        {
            return MatrixOp(Height, Width,
                (i, j) => func(_matrix[i][j]));
        }

        public static Matrix operator *(Matrix first, Matrix second)
        {
            if (first.Width != second.Height)
                throw new InvalidOperationException($"Dimensions don't match: [{first.Height}, {first.Width}] x [{second.Height}, {second.Width}]");

            var secT = second.Transpose();

            return MatrixOp(first.Height, second.Width,
                (i, j) => first._matrix[i].Zip(secT._matrix[j], (x, y) => x * y).Sum());
        }
        public static Matrix operator *(Matrix matrix, double num)
        {
            return MatrixOp(matrix.Height, matrix.Width, 
                (i, j) => matrix[i][j] * num);
        }
        public static Matrix operator /(Matrix matrix, double num)
        {
            return MatrixOp(matrix.Height, matrix.Width,
                (i, j) => matrix[i][j] / num);
        }
        public static Matrix operator -(Matrix first, Matrix second)
        {
            if (first.Height != second.Height || first.Width != second.Width)
                throw new InvalidOperationException($"Dimensions don't match: [{first.Height}, {first.Width}] - [{second.Height}, {second.Width}]");

            return MatrixOp(first.Height, first.Width,
                (i, j) => first._matrix[i][j] - second._matrix[i][j]);
        }

        private static Matrix MatrixOp(int height, int width, Func<int, int, double> func)
        {
            var matrix = GetMatrixTemplate(height, width);

            var innerScope = Enumerable.Range(0, width).ToList();

            Enumerable.Range(0, height)
                .AsParallel()
                .ForAll(i => innerScope
                    .AsParallel()
                    .ForAll(j => matrix[i][j] = func(i, j)));

            return new Matrix(matrix);
        }
        private static double[][] GetMatrixTemplate(int height, int width)
        {
            var matrix = new double[height][];

            Enumerable.Range(0, height)
                .AsParallel()
                .ForAll(i => matrix[i] = new double[width]);

            return matrix;
        }

        public double SumRows()
        {
            double sum = 0;

            Enumerable.Range(0, Height)
                .AsParallel()
                .ForAll(i => sum += _matrix[i][0]);

            return sum;
        }

        public double Max()
        {
            return Transpose()._matrix[0].Max();
        }

        public double Min()
        {
            return Transpose()._matrix[0].Min();
        }

        public static Matrix NormalDistribution(double center, double radius, int size)
        {
            var rand = new Random();

            return new Matrix(new[]
            {
                Enumerable.Repeat(0, size)
                    .AsParallel()
                    .Select(el => rand.NextDouble() * radius * (rand.Next(0, 1) == 1 ? -1 : 1) + center)
                    .ToArray()
            });
        }

        public static Matrix GetIdentityMatrix(int height, int width)
        {
            if (height != width)
                throw new InvalidOperationException("Can't create non square identity matrix");

            return MatrixOp(height, height, (i, j) => i == j ? 1 : 0);
        }

        public Matrix Inverse()
        {
            var determinant = CalculateDeterminant();
            if (Math.Abs(determinant).CompareTo(0) == 0)
                return null;

            return MatrixOp(Height, Height, (i, j) => ((i + j) % 2 == 1 ? -1 : 1) * CalculateMinor(i, j) / determinant)
                .Transpose();
        }
        private double CalculateDeterminant()
        {
            double result = 0;

            if (Width == 2)
                return _matrix[0][0] * _matrix[1][1] - _matrix[0][1] * _matrix[1][0];

            for (var j = 0; j < Width; j++)
                result += (j % 2 == 1 ? 1 : -1) * _matrix[1][j] * CreateMatrixWithoutColumn(j).CreateMatrixWithoutRow(1).CalculateDeterminant();

            return result;
        }
        private double CalculateMinor(int i, int j)
        {
            return CreateMatrixWithoutColumn(j).CreateMatrixWithoutRow(i).CalculateDeterminant();
        }
        private Matrix CreateMatrixWithoutRow(int row)
        {
            if (row < 0 || row >= Height)
                throw new ArgumentException("Invalid row index");

            return MatrixOp(Height - 1, Width, (i, j) => i < row ? _matrix[i][j] : _matrix[i + 1][j]);
        }
        private Matrix CreateMatrixWithoutColumn(int column)
        {
            if (column < 0 || column >= Width)
                throw new ArgumentException("Invalid column index");
            
            return MatrixOp(Height, Width - 1, (i, j) => j < column ? _matrix[i][j] : _matrix[i][j + 1]);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < Height; i++)
            {
                sb.Append(string.Join("\t", _matrix[i]));
                sb.Append("\n");
            }

            return sb.ToString();

            //return Shape;
        }
    }
}
