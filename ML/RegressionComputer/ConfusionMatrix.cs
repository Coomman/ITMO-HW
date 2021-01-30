using System.Linq;

namespace RegressionComputer
{
    internal class ConfusionMatrix
    {
        private readonly int[][] _matrix;

        private readonly int _objectsCount;

        private readonly int[] _rowSum;
        private readonly int[] _columnSum;

        public ConfusionMatrix(int[][] matrix)
        {
            _matrix = matrix;
            _rowSum = new int[matrix.Length];
            _columnSum = new int[matrix.Length];

            for (int i = 0; i < matrix.Length; i++)
            {
                _rowSum[i] = _matrix[i].Sum();
                _columnSum[i] = (int)_matrix.Aggregate<int[], double>(0, (sum, row) => sum + row[i]);
                _objectsCount += _rowSum[i];
            }
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
            => ValueOrZero((double)_matrix[classNum][classNum] / _rowSum[classNum]);
        private double Recall(int classNum)
            => ValueOrZero((double)_matrix[classNum][classNum] / _columnSum[classNum]);

        private double WeighedPrecision()
            => _matrix.Select((_, i) => ValueOrZero((double)_matrix[i][i] * _rowSum[i] / _columnSum[i])).Sum() / _objectsCount;
        private double WeighedRecall()
            => (double)_matrix.Select((_, i) => _matrix[i][i]).Sum() / _objectsCount;

        private static double ValueOrZero(double value)
            => double.IsNaN(value) ? 0 : value;
    }
}
