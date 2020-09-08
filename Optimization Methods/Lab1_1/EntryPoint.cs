using System;
using Lab1_1.DTO;

namespace Lab1_1
{
    internal class EntryPoint
    {
        private static readonly InitialData[] Data =
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

        private static void Main()
        {
            CheckMultiSegmentWithMinimum();

            try
            {
                foreach (var data in Data)
                    CheckSegmentWithMinimum(data);

                const int myVar = 3;
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
            var task = new MinimumFinder(Methods.Dichotomy);

            var segment = task.GetSegmentWithMinimum(data.Func);
            Console.WriteLine($"Segment with minimum: {segment}");

            Console.WriteLine(task.GetMinimum(data).Res);

            data.From = segment.From;
            data.To = segment.To;
            Console.WriteLine(task.GetMinimum(data).Res);
            Console.WriteLine();
        }
        private static void CheckMinimum(int myVar)
        {
            var logger = new ExcelHelper();
            var task = new MinimumFinder(logger);

            for (int i = 2; i < 11; i++)
                task.RunAll(Data[myVar], Math.Pow(10, -i));

            logger.ProcessChart();
            logger.SaveDoc("result.xlsx");
        }
        private static void CheckMultiSegmentWithMinimum()
        {
            var task = new MultiMinimumFinder();

            static double Func(Vector x) => Math.Pow(x[0] - 4, 2) + Math.Pow(x[1] - 8, 2);

            //static double Func(Vector x) => Math.Cos(x[0]);

            var res = task.GetMultiSegmentWithMinimum(Func, new Vector(new[] {-1.0, -2.0}));

            Console.WriteLine(res);
        }
    }
}
