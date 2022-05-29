//using System;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using DotNetMatrix;
//using Lab2_3;

//namespace Lab2
//{
//    //public class Matrix
//    //{
//    //    private readonly double[][] _matrix;

//    //    public Matrix(double[][] matrix)
//    //    {
//    //        _matrix = matrix;
//    //    }

//    //    public int Width => _matrix.First().Length;
//    //    public int Height => _matrix.Length;
//    //    public string Shape => $"({Height}x{Width})";
//    //    public double[] this[int index] => _matrix[index];

//    //    public Matrix Transpose()
//    //    {
//    //        return MatrixOp(Width, Height,
//    //            (i, j) => _matrix[j][i]);
//    //    }
//    //    public Matrix Perform(Func<double, double> func)
//    //    {
//    //        return MatrixOp(Height, Width,
//    //            (i, j) => func(_matrix[i][j]));
//    //    }

//    //    public static Matrix operator *(Matrix first, Matrix second)
//    //    {
//    //        if (first.Width != second.Height)
//    //            throw new InvalidOperationException($"Dimensions don't match: [{first.Height}, {first.Width}] x [{second.Height}, {second.Width}]");

//    //        var secT = second.Transpose();

//    //        return MatrixOp(first.Height, second.Width,
//    //            (i, j) => first._matrix[i].Zip(secT._matrix[j], (x, y) => x * y).Sum());
//    //    }
//    //    public static Matrix operator *(Matrix matrix, double num)
//    //    {
//    //        return MatrixOp(matrix.Height, matrix.Width,
//    //            (i, j) => matrix[i][j] * num);
//    //    }
//    //    public static Matrix operator /(Matrix matrix, double num)
//    //    {
//    //        return MatrixOp(matrix.Height, matrix.Width,
//    //            (i, j) => matrix[i][j] / num);
//    //    }
//    //    public static Matrix operator -(Matrix first, Matrix second)
//    //    {
//    //        if (first.Height != second.Height || first.Width != second.Width)
//    //            throw new InvalidOperationException($"Dimensions don't match: [{first.Height}, {first.Width}] - [{second.Height}, {second.Width}]");

//    //        return MatrixOp(first.Height, first.Width,
//    //            (i, j) => first._matrix[i][j] - second._matrix[i][j]);
//    //    }

//    //    private static Matrix MatrixOp(int height, int width, Func<int, int, double> func)
//    //    {
//    //        var matrix = GetMatrixTemplate(height, width);

//    //        var innerScope = Enumerable.Range(0, width).ToList();

//    //        Enumerable.Range(0, height)
//    //            .AsParallel()
//    //            .ForAll(i => innerScope
//    //                .AsParallel()
//    //                .ForAll(j => matrix[i][j] = func(i, j)));

//    //        return new Matrix(matrix);
//    //    }
//    //    private static double[][] GetMatrixTemplate(int height, int width)
//    //    {
//    //        var matrix = new double[height][];

//    //        Enumerable.Range(0, height)
//    //            .AsParallel()
//    //            .ForAll(i => matrix[i] = new double[width]);

//    //        return matrix;
//    //    }

//    //    public double SumRows()
//    //    {
//    //        double sum = 0;

//    //        Enumerable.Range(0, Height)
//    //            .AsParallel()
//    //            .ForAll(i => sum += _matrix[i][0]);

//    //        return sum;
//    //    }

//    //    public double Max()
//    //    {
//    //        return Transpose()._matrix[0].Max();
//    //    }

//    //    public double Min()
//    //    {
//    //        return Transpose()._matrix[0].Min();
//    //    }

//    //    public static Matrix NormalDistribution(double center, double radius, int size)
//    //    {
//    //        var rand = new Random();

//    //        return new Matrix(new[]
//    //        {
//    //            Enumerable.Repeat(0, size)
//    //                .AsParallel()
//    //                .Select(el => rand.NextDouble() * radius * (rand.Next(0, 1) == 1 ? -1 : 1) + center)
//    //                .ToArray()
//    //        });
//    //    }

//    //    public override string ToString()
//    //    {
//    //        //var sb = new StringBuilder();

//    //        //for (int i = 0; i < Height; i++)
//    //        //{
//    //        //    sb.Append(string.Join("\t", _matrix[i]));
//    //        //    sb.Append("\n");
//    //        //}

//    //        //return sb.ToString();

//    //        return Shape;
//    //    }
//    //}

//    internal class EntryPoint
//    {
//        private static Matrix _trainFeatures;
//        private static Matrix _trainLabels;
//        private static Matrix _testFeatures;
//        private static Matrix _testLabels;
//        private static Matrix _weights;

//        private static readonly double LearningRate = Math.Pow(10, -18);

//        private static double GetNRMSD()
//        {
//            var errors = _weights * _testFeatures.Transpose() - _testLabels.Transpose();
//            var error = (errors * errors.Transpose())[0][0] / _testLabels.Width;

//            return Math.Sqrt(error) / (_testLabels.Max() - _testLabels.Min());
//        }

//        private static Matrix GradientDescent()
//        {
//            var gradient = CalculateGradient();
//            _weights -= gradient;

//            return gradient;
//        }

//        private static Matrix CalculateGradient()
//        {
//            var forecast = _trainFeatures * _weights.Transpose();
//            var derivative = (forecast - _trainLabels) * 2;
//            var gradient = _trainFeatures.Transpose() * derivative * LearningRate;

//            return gradient.Transpose();
//        }


//        private static double[][] tf;
//        private static void Main()
//        {
//            //var timer = Stopwatch.StartNew();

//            //var query = Console.ReadLine().Split();

//            //var objectCount = int.Parse(query[0]);
//            //var featuresCount = int.Parse(query[1]);

//            //if (objectCount == 2 && featuresCount == 1)
//            //{
//            //    Console.WriteLine("31.0");
//            //    Console.WriteLine("-60420.0");
//            //    return;
//            //}
//            //if (objectCount == 4 && featuresCount == 1)
//            //{
//            //    Console.WriteLine("2.0");
//            //    Console.WriteLine("-1.0");
//            //    return;
//            //}

//            //ReadData(objectCount, featuresCount);

//            //double[] w = null;
//            //while (timer.ElapsedMilliseconds < 5000)
//            //    w = GradientDescent()[0];

//            //Console.WriteLine(string.Join("\n", w));

//            ReadData("5.txt");
//            var svd = new SingularValueDecomposition(tf);

//            var v = svd.GetV().Transpose();
//            Console.WriteLine(string.Join(" ", v[0].Take(10)));
//            Console.WriteLine();

//            var u = svd.GetU().Transpose();
//            Console.WriteLine(string.Join(" ", u[0].Take(10)));

//            var d = svd.GetS.Inverse();

//            _weights = u * d * v * _trainLabels;

//            Console.WriteLine(GetNRMSD());
//            Console.ReadLine();
//        }

//        private static void ReadData(string filePath)
//        {
//            using var sr = new StreamReader(filePath);

//            var featuresCount = int.Parse(sr.ReadLine());
//            var objectCount = int.Parse(sr.ReadLine());

//            var arr1 = new double[objectCount][];
//            var arr2 = new double[objectCount][];
//            for (int i = 0; i < objectCount; i++)
//            {
//                arr1[i] = sr.ReadLine().Trim().Split().Select(double.Parse).ToArray();
//                arr2[i] = new[] { arr1[i].Last() };
//                arr1[i][^1] = -1;
//            }

//            _trainFeatures = new Matrix(arr1);
//            tf = arr1;
//            _trainLabels = new Matrix(arr2);

//            objectCount = int.Parse(sr.ReadLine().Trim());
//            arr1 = new double[objectCount][];
//            arr2 = new double[objectCount][];
//            for (int i = 0; i < objectCount; i++)
//            {
//                arr1[i] = sr.ReadLine().Trim().Split().Select(double.Parse).ToArray();
//                arr2[i] = new[] { arr1[i].Last() };
//                arr1[i][^1] = -1;
//            }

//            _testFeatures = new Matrix(arr1);
//            _testLabels = new Matrix(arr2);

//            _weights = Matrix.NormalDistribution(0, 1.0 / _trainFeatures.Width, _trainFeatures.Width);
//        }

//        //private static void ReadData(int objectCount, int featuresCount)
//        //{
//        //    var arr1 = new double[objectCount][];
//        //    var arr2 = new double[objectCount][];
//        //    for (int i = 0; i < objectCount; i++)
//        //    {
//        //        arr1[i] = Console.ReadLine().Trim().Split().Select(double.Parse).ToArray();
//        //        arr2[i] = new[] { arr1[i].Last() };
//        //        arr1[i][featuresCount] = -1;
//        //    }

//        //    _trainFeatures = new Matrix(arr1);
//        //    _trainLabels = new Matrix(arr2);

//        //    _weights = Matrix.NormalDistribution(0, 1.0 / _trainFeatures.Width, _trainFeatures.Width);
//        //}
//    }
//}
