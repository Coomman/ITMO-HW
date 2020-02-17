using System.Linq;

namespace Lab2
{
    class Polynomial: RList
    {
        public Polynomial() 
        { }
        public Polynomial(RList list)
        {
            AddRange(list);
        }

        public override string ToString()
            => string.Join(" ", Values.Select((s, i) => $"{s}x^{i}"));

        public static Polynomial operator +(Polynomial first, Polynomial second)
        {
            var p = new Polynomial();
            Polynomial smaller = first.Count < second.Count ? first : second;
            Polynomial bigger = smaller == first ? second : first;

            for (int i = 0; i < smaller.Count; i++)
            {
                p.Add(smaller[i] + bigger[i]);
            }

            for (int i = smaller.Count; i < bigger.Count; i++)
            {
                p.Add(bigger[i].Clone());
            }

            return p;
        }
    }
}
