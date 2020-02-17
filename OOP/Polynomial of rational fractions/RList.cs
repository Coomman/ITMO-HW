using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab2
{
    class RList : IEnumerable<Rational>
    {
        protected readonly List<Rational> Values = new List<Rational>();
        private (Rational r, int i) _lessCount, _greaterCount;

        public Rational Min { get; private set; }
        public Rational Max { get; private set; }

        public int Less(Rational r)
        {
            if (Count == 0)
                return 0;

            if (r == _lessCount.r) 
                return _lessCount.i;

            int count = Values.Count(it => it < r);
            _lessCount = (r, count);

            return count;
        }
        public int Greater(Rational r)
        {
            if (Count == 0)
                return 0;

            if (r == _greaterCount.r)
                return _greaterCount.i;

            int count = Values.Count(it => it > r);
            _greaterCount = (r, count);

            return count;
        }

        public Rational this[int index]
        {
            get => Values[index];
            set => Values[index] = value;
        }
        public IEnumerator<Rational> GetEnumerator()
            => Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
            => Values.Count;
        public override string ToString()
            => string.Join(' ', Values);
        public void Print()
        {
            Console.WriteLine($"{this}\n");
        }

        public void Add(Rational r)
        {
            Values.Add(r);

            Min ??= r;
            Min = Min < r ? Min : r;

            Max ??= r;
            Max = Max > r ? Max : r;

            if (_lessCount.r != null && r < _lessCount.r)
                _lessCount.i++;

            if (_greaterCount.r != null && r > _greaterCount.r)
                _greaterCount.i++;
        }
        public void Assert(string path)
        {
            var res = 
                File.ReadAllText(path)
                .Split(' ')
                .Select(s => s.Split('/'));

            foreach (var s in res)
            {
                Add(new Rational(int.Parse(s[0]), int.Parse(s[1])));
            }
        }
        public void AddRange(RList rList)
        {
            foreach(var r in rList)
            {
                Add(r.Clone());
            }
        }
    }
}