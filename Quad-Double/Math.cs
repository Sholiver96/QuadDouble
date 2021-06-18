using System;
using System.Collections.Generic;
using System.Text;

namespace Quad_Double
{
    public static class Math
    {
        /// <summary>
        /// Computes sum and error of summing 2 doubles. Assuming |a| >= |b|
        /// </summary>
        /// <returns></returns>
        public static (double sum, double err) QuickTwoSum(double a, double b)
        {
            double sum = a + b;
            double err = b - (sum - a);
            return (sum, err);
        }

        /// <summary>
        /// Computes sum and error of summing 2 doubles.
        /// </summary>
        /// <returns></returns>
        public static (double sum, double err) TwoSum(double a, double b)
        {
            double sum = a + b;
            double v = sum - a;
            double err = (a - (sum - v)) + (b - v);
            return (sum, err);
        }

        public static (double aHi, double aLo) Split(double a)
        {
            double t = 134217729 * a;   //2^27+1
            double aHi = t - (t - a);
            double aLo = a - aHi;
            return (aHi, aLo);
        }

        public static (double prod, double err) TwoProd(double a, double b)
        {
            double prod = a * b;
            (double aHi, double aLo) = Split(a);
            (double bHi, double bLo) = Split(b);
            double err = ((aHi * bHi - prod) + aHi * bLo + aLo * bHi) + aLo * bLo;
            return (prod, err);
        }

        public static (double r0, double r1, double r2) ThreeSumThree(double x, double y, double z)
        {
            var sum1 = TwoSum(x, y);
            var sum2 = TwoSum(sum1.sum, z);
            var sum3 = TwoSum(sum1.err, sum2.err);

            return (sum2.sum, sum3.sum, sum3.err);
        }

        public static (double r0, double r1) ThreeSumTwo(double x, double y, double z)
        {
            var sum1 = TwoSum(x, y);
            var sum2 = TwoSum(sum1.sum, z);
            var sum3 = sum1.err + sum2.err;

            return (sum2.sum, sum3);
        }

        public static double ThreeSumOne(double x, double y, double z)
        {
            var sum1 = x + y;
            var sum2 = sum1 + z;

            return sum2;
        }

        public static (double r0, double r1, double r2) SixSumThree(double x, double y, double z, double a, double b, double c)
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

        public static (double r0, double r1) FourSumTwo(double w, double x, double y, double z)
        {
            var sum1 = TwoSum(x, y);
            return (sum1.sum, ThreeSumOne(w, z, sum1.err));
        }

        public static (double r0, double r1) NineSumTwo(double x0, double x1, double x2, double x3, double x4, double x5, double x6, double x7, double x8)
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
    }
}
