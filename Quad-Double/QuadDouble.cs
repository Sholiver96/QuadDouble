using System;

namespace Quad_Double
{
    public struct QuadDouble
    {
        private double x0;
        private double x1;
        private double x2;
        private double x3;

        public QuadDouble(double x0, double x1, double x2, double x3)
        {
            this.x0 = x0;
            this.x1 = x1;
            this.x2 = x2;
            this.x3 = x3;
        }

        public QuadDouble(double x)
        {
            var xSplit = Split(x);
            var xHi = Split(xSplit.aHi);
            var xLo = Split(xSplit.aLo);

            x0 = xHi.aHi;
            x1 = xHi.aLo;
            x2 = xLo.aHi;
            x3 = xLo.aLo;
        }

        /// <summary>
        /// Computes sum and error of summing 2 doubles. Assuming |a| >= |b|
        /// </summary>
        /// <returns></returns>
        private static (double sum, double err) QuickTwoSum(double a, double b)
        {
            double sum = a + b;
            double err = b - (sum - a);
            return (sum, err);
        }

        /// <summary>
        /// Computes sum and error of summing 2 doubles.
        /// </summary>
        /// <returns></returns>
        private static (double sum, double err) TwoSum(double a, double b)
        {
            double sum = a + b;
            double v = sum - a;
            double err = (a - (sum - v)) + (b - v);
            return (sum, err);
        }

        private static (double aHi, double aLo) Split(double a)
        {
            double t = 134217729 * a;   //2^27+1
            double aHi = t - (t - a);
            double aLo = a - aHi;
            return (aHi, aLo);
        }

        private static (double prod, double err) TwoProd(double a, double b)
        {
            double prod = a * b;
            (double aHi, double aLo) = Split(a);
            (double bHi, double bLo) = Split(b);
            double err = ((aHi * bHi - prod) + aHi * bLo + aLo * bHi) + aLo * bLo;
            return (prod, err);
        }

        private static (double r0, double r1, double r2) ThreeSumThree(double x, double y, double z)
        {
            var sum1 = TwoSum(x, y);
            var sum2 = TwoSum(sum1.sum, z);
            var sum3 = TwoSum(sum1.err, sum2.err);

            return (sum2.sum, sum3.sum, sum3.err);
        }

        private static (double r0, double r1) ThreeSumTwo(double x, double y, double z)
        {
            var sum1 = TwoSum(x, y);
            var sum2 = TwoSum(sum1.sum, z);
            var sum3 = sum1.err + sum2.err;

            return (sum2.sum, sum3);
        }

        private static double ThreeSumOne(double x, double y, double z)
        {
            var sum1 = x + y;
            var sum2 = sum1 + z;

            return sum2;
        }

        private static QuadDouble Renormalise(double a0, double a1, double a2, double a3, double a4)
        {
            (double s, double e) c4 = QuickTwoSum(a3, a4); 
            (double s, double e) c3 = QuickTwoSum(a2, c4.s); 
            (double s, double e) c2 = QuickTwoSum(a1, c3.s); 
            (double s, double e) c1 = QuickTwoSum(a0, c2.s);

            double[] t = new double[5]
            {
                c1.s, 
                c1.e, 
                c2.e, 
                c3.e, 
                c4.e
            };

            double[] b = new double[4] { 0.0, 0.0, 0.0, 0.0 };
            int k = 0;
            double s = t[0];
            for (int i = 1; i < 5; i++)
            {
                (double s, double e) sum = QuickTwoSum(s, t[i]);
                b[k] = sum.s;
                s = sum.e;
                k++;
            }
            return new QuadDouble(b[0], b[1], b[2], b[3]);
        }

        public static QuadDouble operator +(QuadDouble a, double b)
        {
            (double s, double e) sum0 = TwoSum(a.x0, b);
            (double s, double e) sum1 = TwoSum(a.x1, sum0.e);
            (double s, double e) sum2 = TwoSum(a.x2, sum1.e);
            (double s, double e) sum3 = TwoSum(a.x3, sum2.e);

            return Renormalise(sum0.s, sum1.s, sum2.s, sum3.s, sum3.e);
        }

        public static QuadDouble operator +(QuadDouble a, QuadDouble b)
        {
            var sum0 = TwoSum(a.x0, b.x0);
            var sum1 = TwoSum(a.x1, b.x1);
            var sum2 = TwoSum(a.x2, b.x2);
            var sum3 = TwoSum(a.x3, b.x3);

            var sum11 = TwoSum(sum1.sum, sum0.err);
            var sum12 = ThreeSumThree(sum2.sum, sum1.err, sum11.err);
            var sum13 = ThreeSumTwo(sum3.sum, sum2.err, sum12.r1);
            var sum14 = ThreeSumOne(sum3.err, sum13.r1, sum12.r2);

            return Renormalise(sum0.sum, sum11.sum, sum12.r0, sum13.r0, sum14);
        }
    }
}
