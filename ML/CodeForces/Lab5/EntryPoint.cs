 using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab5
{
    public static class Extensions
    {
        public static int ToInt(this string str)
            => int.Parse(str);
    }

    public class Neuron
    {
        public IReadOnlyList<int> Weights { get; }
        public double Bias { get; }

        public Neuron(IReadOnlyList<int> weights, double bias)
        {
            Weights = weights;
            Bias = bias;
        }

        public override string ToString()
        {
            return string.Join(" ", Weights) + $" {Bias}";
        }
    }

    public class LogicComputer
    {
        private readonly int _argCount;
        private readonly int _tableSize;

        private readonly List<int> _zeroes = new List<int>(1024);
        private readonly List<int> _ones = new List<int>(1024);

        public LogicComputer()
        {
            _argCount = ReadInt();
            _tableSize = (int) Math.Pow(2, _argCount);

            for (int i = 0; i < _tableSize; i++)
            {
                var f = ReadInt();

                if (f == 0)
                    _zeroes.Add(i);
                else
                    _ones.Add(i);
            }
        }
        private static int ReadInt()
            => Console.ReadLine().ToInt();

        public void Process()
        {
            if (_zeroes.Count == _tableSize)
            {
                WriteAnswer(new[] {new Neuron(Enumerable.Repeat(0, _argCount).ToArray(), -0.5)});
                return;
            }
            if (_ones.Count == _tableSize)
            {
                WriteAnswer(new[] {new Neuron(Enumerable.Repeat(0, _argCount).ToArray(), 0.5)});
                return;
            }

            var list = _ones.Count < _zeroes.Count ? _ones : _zeroes;

            var layer = new List<Neuron>(list
                .Select(index => Convert
                    .ToString(index, 2)
                    .PadLeft(_argCount, '0')
                    .Select(ch => ch == '1' ? 1 : -1)
                    .ToArray())
                .Select(weights => new Neuron(weights, -weights.Count(w => w == 1) + 0.5)));

            WriteAnswer(layer, true);

            Console.WriteLine(_ones.Count < _zeroes.Count
                ? new Neuron(Enumerable.Repeat(1, layer.Count).ToArray(), -0.5)
                : new Neuron(Enumerable.Repeat(-1, layer.Count).ToArray(), 0.5));
        }

        private static void WriteAnswer(ICollection<Neuron> neurons, bool hasSecondLayer = false)
        {
            Console.WriteLine(hasSecondLayer ? 2 : 1);

            if (hasSecondLayer)
                Console.Write($"{neurons.Count} ");

            Console.WriteLine(1);

            foreach (var neuron in neurons)
                Console.WriteLine(neuron);
        }
    }

    class EntryPoint
    {
        private static void Main()
        {
            var lc = new LogicComputer();
            lc.Process();

            Console.ReadLine();
        }
    }
}
