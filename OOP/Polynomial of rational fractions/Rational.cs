using System;

namespace Lab2
{
    class Rational
    {
        private int _numerator, _denominator;

        public Rational(int numerator, int denominator = 1)
        {
            if (denominator == 0)
                throw new Exception("Zero in Denominator");

            _numerator = numerator;
            _denominator = denominator;
            Reduce();
        }

        private void Reduce()
        {
            int nod = GetNod();
            _numerator /= nod;
            _denominator /= nod;

            if (_denominator > 0)
                return;

            _numerator = -_numerator;
            _denominator = -_denominator;

            int GetNod()
            {
                int num = Math.Abs(_numerator);
                int den = Math.Abs(_denominator);
                while (num != 0 && den != 0)
                {
                    if (num > den)
                        num %= den;
                    else
                        den %= num;
                }

                return num + den;
            }
        }

        public override string ToString()
            => $"{_numerator}/{_denominator}";
        public Rational Clone()
            => new Rational(_numerator, _denominator);

        public static Rational operator +(Rational first, Rational second)
            => new Rational(first._numerator * second._denominator + second._numerator * first._denominator,
                first._denominator * second._denominator);
        public static Rational operator -(Rational first, Rational second)
            => new Rational(first._numerator * second._denominator - second._numerator * first._denominator,
                first._denominator * second._denominator);
        public static Rational operator *(Rational first, Rational second)
            => new Rational(first._numerator * second._numerator, first._denominator * second._denominator);
        public static Rational operator /(Rational first, Rational second)
            => new Rational(first._numerator * second._denominator, first._denominator * second._numerator);
        public static bool operator <(Rational first, Rational second)
            => first._numerator * second._denominator < second._numerator * first._denominator;
        public static bool operator >(Rational first, Rational second)
            => first._numerator * second._denominator > second._numerator * first._denominator;
    }
}