using System;
using Lab1_1.DTO;
using NPOI.SS.Formula.Functions;

namespace Lab1_1
{
    internal class EntryPoint
    {
        private static readonly InitialData[] Data2 =
        {
            new InitialData {Func = Math.Sin, From = -Math.PI / 2, To = Math.PI / 2},
            new InitialData {Func = Math.Cos, From = 0, To = Math.PI},
            new InitialData {Func = x => Math.Pow(x - 2, 2), From = -2, To = 20},
            new InitialData {Func = x => Math.Pow(x - 15, 2) + 5, From = 2, To = 200},
            new InitialData {Func = x => Math.Pow(x + 5, 4), From = -10, To = 15},
            new InitialData {Func = Math.Exp, From = 0, To = 10},
            new InitialData {Func = x => x * x + 2 * x - 4, From = -10, To = 20},
            new InitialData {Func = x => x * x * x - x, From = 0, To = 1},
            new InitialData {Func = x => x * x, From = -10, To = 10}
        };

        private static readonly InitialData[] Data =
        {
            new InitialData {From = -0.5, To = 0.5, Func = x => -5 * Math.Pow(x, 5) + 4 * Math.Pow(x, 4) - 12 * Math.Pow(x, 3) + 11 * Math.Pow(x, 2) - 2 * x + 1}, // 0.89763
            new InitialData {From = 6, To = 9.9, Func = x => Math.Pow(Math.Log10(x - 2), 2) + Math.Pow(Math.Log10(10 - x), 2) - Math.Pow(x, 0.2)}, // -0.84603
            new InitialData {From = 0, To = Math.PI * 2, Func = x => -3 * x * Math.Sin(x * 0.75) + Math.Exp(-2 * x)}, // -7.27463
            new InitialData {From = 0, To = 1, Func = x => Math.Exp(3 * x) + 5 * Math.Exp(-2 * x)}, // 5.14834
            new InitialData {From = 0.5, To = 2.5, Func = x => 0.2 * x * Math.Log10(x) + Math.Pow(x - 2.3, 2)}, // 0.160177
        };

        private static void Main()
        {
            //CheckMultiSegmentWithMinimum();

            try
            {
                foreach (var data in Data)
                    CheckSegmentWithMinimum(data);

                var myVar = 4;
                CheckMinimum(myVar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
            }

            Console.ReadLine();
        }

        private static void CheckSegmentWithMinimum(InitialData data)
        {
            var task = new MinimumFinder(Methods.Brent);

            //var segment = task.GetSegmentWithMinimum(data.Func);
            //Console.WriteLine($"Segment with minimum: {segment}");
            var test = task.GetMinimum(data);
            Console.WriteLine($"Ans: {test.Res} Iterations: {test.IterationCount}");

            //data.From = segment.From;
            //data.To = segment.To;
            //Console.WriteLine(task.GetMinimum(data).Res);
            Console.WriteLine();
        }
        private static void CheckMinimum(int myVar)
        {
            var logger = new ExcelHelper();
            var task = new MinimumFinder(logger);

            for (int i = 2; i < 8; i++)
                task.RunAll(Data[myVar], Math.Pow(10, -i));

            logger.ProcessChart();
            logger.SaveDoc($"result{myVar + 1}.xlsx");
        }
        private static void CheckMultiSegmentWithMinimum()
        {
            var task = new MultiMinimumFinder(Methods.Fibonacci);

            static double Func(Vector x) => Math.Pow(x[0] - 4, 2) + Math.Pow(x[1] - 8, 2);

            //static double Func(Vector x) => Math.Cos(x[0]);

            var segment = task.GetMultiSegmentWithMinimum(Func, new Vector(new[] {-1.0, -2.0}));

            var result = task.GetMinimum(new MultiInitialData {From = segment.From, To = segment.To, Func = Func});

            Console.WriteLine(result.segment);
        }
    }
}
