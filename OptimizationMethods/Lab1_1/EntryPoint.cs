using System;

namespace OptimizationMethods
{
    internal class EntryPoint
    {
        private static void Main()
        {
            //var task = new Task1_1(Methods.Fibonacci);

            InitialData[] data =
            {
                new InitialData {Func = Math.Sin, From = -Math.PI / 2, To = Math.PI / 2},
                new InitialData {Func = Math.Cos, From = 0, To = Math.PI},
                new InitialData {Func = x => Math.Pow(x - 2, 2), From = -2, To = 20},
                new InitialData {Func = x => Math.Pow(x - 15, 2) + 5, From = 2, To = 200},
                new InitialData {Func = x => Math.Pow(x + 5, 4), From = -10, To = 15},
                new InitialData {Func = Math.Exp, From = 0, To = 10},
                new InitialData {Func = x => x * x + 2 * x - 4, From = -10, To = 20},
                new InitialData {Func = x => x * x * x - x, From = 0, To = 1}
            };

            var logger = new ExcelHelper();
            var task = new Task1_1(logger);

            const int myVar = 3;
            try
            {
                for (int i = 2; i < 11; i++)
                    task.RunAll(data[myVar], Math.Pow(10, -i));

                logger.SaveDoc("result.xls");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
            }

            //TODO: Add Final Result missing properties
        }
    }
}
