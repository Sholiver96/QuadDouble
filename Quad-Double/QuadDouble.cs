using System;

namespace Quad_Double
{
    public struct QuadDouble
    {
        public double x0;
        public double x1;
        public double x2;
        public double x3;

        public QuadDouble(double x0, double x1, double x2, double x3)
        {
            this.x0 = x0;
            this.x1 = x1;
            this.x2 = x2;
            this.x3 = x3;
        }

        public QuadDouble(double x)
        {
            x0 = x;
            x1 = 0;
            x2 = 0;
            x3 = 0;
        }
        public static implicit operator QuadDouble(double value) => new QuadDouble(value);

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

        private static (double r0, double r1, double r2) SixSumThree(double x, double y, double z, double a, double b, double c)
        {
            var sum1 = ThreeSumThree(x, y, z);
            var sum2 = ThreeSumThree(a, b, c);

            var sum11 = TwoSum(sum1.r0, sum2.r0);
            var sum12 = TwoSum(sum1.r1, sum2.r1);
            var sum13 = sum1.r2 + sum2.r2;

            var sum21 = TwoSum(sum11.err, sum12.sum);
            var sum22 = ThreeSumOne(sum13, sum12.err, sum21.err);

            return (sum11.sum, sum21.sum, sum22);
        }

        private static (double r0, double r1) FourSumTwo(double w, double x, double y, double z)
        {
            var sum1 = TwoSum(x, y);
            return (sum1.sum, ThreeSumOne(w, z, sum1.err));
        }

        private static (double r0, double r1) NineSumTwo(double x0, double x1, double x2, double x3, double x4, double x5, double x6, double x7, double x8)
        {
            var sum1 = TwoSum(x0, x3);
            var sum2 = TwoSum(x1, x2);
            var sum11 = FourSumTwo(sum1.err, sum1.sum, sum2.err, sum2.sum);


            var sum3 = TwoSum(x4, x7);
            var sum4 = TwoSum(x5, x6);
            var sum12 = FourSumTwo(sum3.err, sum3.sum, sum4.err, sum4.sum);

            var sum21 = FourSumTwo(sum11.r0, sum11.r1, sum12.r0, sum12.r1);

            return ThreeSumTwo(sum21.r1, sum21.r0, x8);
        }

        public static QuadDouble Renormalise(double a0, double a1, double a2, double a3, double a4)
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

        public static QuadDouble operator *(QuadDouble a, double b)
        {
            var prod0 = TwoProd(a.x0, b);
            var prod1 = TwoProd(a.x1, b);
            var prod2 = TwoProd(a.x2, b);
            var prod3 = a.x3 * b;

            var sum11 = TwoSum(prod0.err, prod1.prod);
            var sum12 = ThreeSumThree(prod2.prod, prod1.err, sum11.err);
            var sum13 = ThreeSumTwo(prod3, prod2.err, sum12.r1);
            var sum14 = sum13.r1 + sum12.r2;

            return Renormalise(prod0.prod, sum11.sum, sum12.r0, sum13.r0, sum14);
        }

        public static QuadDouble operator *(QuadDouble a, QuadDouble b)
        {
            var prod00 = TwoProd(a.x0, b.x0);

            var prod01 = TwoProd(a.x0, b.x1);
            var prod10 = TwoProd(a.x1, b.x0);

            var prod02 = TwoProd(a.x0, b.x2);
            var prod11 = TwoProd(a.x1, b.x1);
            var prod20 = TwoProd(a.x2, b.x0);

            var prod03 = TwoProd(a.x0, b.x3);
            var prod12 = TwoProd(a.x1, b.x2);
            var prod21 = TwoProd(a.x2, b.x1);
            var prod30 = TwoProd(a.x3, b.x0);

            var prod13 = TwoProd(a.x1, b.x3);
            var prod22 = TwoProd(a.x2, b.x2);
            var prod31 = TwoProd(a.x3, b.x1);

            var sum1 = ThreeSumThree(prod00.err, prod01.prod, prod10.prod);
            var sum2 = SixSumThree(sum1.r1, prod01.err, prod10.err, prod02.prod, prod11.prod, prod20.prod);
            var sum3 = NineSumTwo(sum1.r2, sum2.r1, prod02.err, prod11.err, prod20.err, prod03.prod, prod12.prod, prod21.prod, prod30.prod);
            var sum4 = NineSumTwo(sum2.r2, sum3.r1, prod03.err, prod12.err, prod21.err, prod30.err, prod13.prod, prod22.prod, prod31.prod);

            return Renormalise(prod00.prod, sum1.r0, sum2.r0, sum3.r0, sum4.r0);
        }
    }
}
