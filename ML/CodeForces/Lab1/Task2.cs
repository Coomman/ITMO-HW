using System;
using System.Linq;

namespace Lab1
{
    internal class Matrix
    {
        private readonly int[][] _matrix;

        private readonly int _objectsCount;

        private readonly int[] _rowSum;
        private readonly int[] _columnSum;

        public Matrix(int classCount)
        {
            _matrix = new int[classCount][];
            _rowSum = new int[classCount];

            for (int i = 0; i < classCount; i++)
            {
                _matrix[i] = Console.ReadLine().Split().Select(int.Parse).ToArray();
                _rowSum[i] = _matrix[i].Sum();
                _objectsCount += _rowSum[i];
            }

            _columnSum = new int[classCount];
            for (int i = 0; i < classCount; i++)
                _columnSum[i] = (int)_matrix.Aggregate<int[], double>(0, (sum, row) => sum + row[i]);
        }

        public double MacroF1Score()
        {
            var precision = WeighedPrecision();
            var recall = WeighedRecall();

            if ((precision + recall).CompareTo(0) == 0)
                return 0;

            return 2 * precision * recall / (precision + recall);
        }
        public double MicroF1Score()
        {
            return _matrix.Select((_, i) => _rowSum[i] * F1Score(i)).Sum() / _objectsCount;
        }

        private double F1Score(int classNum)
        {
            if (_matrix[classNum][classNum] == 0)
                return 0;

            double precision = Precision(classNum);
            double recall = Recall(classNum);

            if ((precision + recall).CompareTo(0) == 0)
                return 0;

            return 2 * precision * recall / (precision + recall);
        }

        private double Precision(int classNum)
            => ValueOrNull((double)_matrix[classNum][classNum] / _rowSum[classNum]);
        private double Recall(int classNum)
            => ValueOrNull((double)_matrix[classNum][classNum] / _columnSum[classNum]);

        private double WeighedPrecision()
            => _matrix.Select((_, i) => ValueOrNull((double) _matrix[i][i] * _rowSum[i] / _columnSum[i])).Sum() / _objectsCount;
        private double WeighedRecall()
            => (double)_matrix.Select((_, i) => _matrix[i][i]).Sum() / _objectsCount;

        private static double ValueOrNull(double value)
            => double.IsNaN(value) ? 0 : value;
    }

    internal class Task2
    {
        public static void Main_()
        {
            var classCount = int.Parse(Console.ReadLine());

            var matrix = new Matrix(classCount);

            Solve(matrix);

            Console.ReadLine();
        }
        private static void Solve(Matrix matrix)
        {
            Console.WriteLine(matrix.MacroF1Score());
            Console.WriteLine(matrix.MicroF1Score());
        }
    }
}
