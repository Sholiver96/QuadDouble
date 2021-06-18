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

        

        public static QuadDouble Renormalise(double a0, double a1, double a2, double a3, double a4)
        {
            (double s, double e) c4 = Math.QuickTwoSum(a3, a4); 
            (double s, double e) c3 = Math.QuickTwoSum(a2, c4.s); 
            (double s, double e) c2 = Math.QuickTwoSum(a1, c3.s); 
            (double s, double e) c1 = Math.QuickTwoSum(a0, c2.s);

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
                (double s, double e) sum = Math.QuickTwoSum(s, t[i]);
                b[k] = sum.s;
                s = sum.e;
                k++;
            }
            return new QuadDouble(b[0], b[1], b[2], b[3]);
        }

        public static QuadDouble operator +(QuadDouble a, double b)
        {
            (double s, double e) sum0 = Math.TwoSum(a.x0, b);
            (double s, double e) sum1 = Math.TwoSum(a.x1, sum0.e);
            (double s, double e) sum2 = Math.TwoSum(a.x2, sum1.e);
            (double s, double e) sum3 = Math.TwoSum(a.x3, sum2.e);

            return Renormalise(sum0.s, sum1.s, sum2.s, sum3.s, sum3.e);
        }

        public static QuadDouble operator +(QuadDouble a, QuadDouble b)
        {
            var sum0 = Math.TwoSum(a.x0, b.x0);
            var sum1 = Math.TwoSum(a.x1, b.x1);
            var sum2 = Math.TwoSum(a.x2, b.x2);
            var sum3 = Math.TwoSum(a.x3, b.x3);

            var sum11 = Math.TwoSum(sum1.sum, sum0.err);
            var sum12 = Math.ThreeSumThree(sum2.sum, sum1.err, sum11.err);
            var sum13 = Math.ThreeSumTwo(sum3.sum, sum2.err, sum12.r1);
            var sum14 = Math.ThreeSumOne(sum3.err, sum13.r1, sum12.r2);

            return Renormalise(sum0.sum, sum11.sum, sum12.r0, sum13.r0, sum14);
        }

        public static QuadDouble operator -(QuadDouble a, double b)
        {
            return a + (0 - b);
        }

        public static QuadDouble operator -(QuadDouble a, QuadDouble b)
        {
            return a + new QuadDouble(-b.x0, -b.x1, -b.x2, -b.x3);
        }

        public static QuadDouble operator *(QuadDouble a, double b)
        {
            var prod0 = Math.TwoProd(a.x0, b);
            var prod1 = Math.TwoProd(a.x1, b);
            var prod2 = Math.TwoProd(a.x2, b);
            var prod3 = a.x3 * b;

            var sum11 = Math.TwoSum(prod0.err, prod1.prod);
            var sum12 = Math.ThreeSumThree(prod2.prod, prod1.err, sum11.err);
            var sum13 = Math.ThreeSumTwo(prod3, prod2.err, sum12.r1);
            var sum14 = sum13.r1 + sum12.r2;

            return Renormalise(prod0.prod, sum11.sum, sum12.r0, sum13.r0, sum14);
        }

        public static QuadDouble operator *(QuadDouble a, QuadDouble b)
        {
            var prod00 = Math.TwoProd(a.x0, b.x0);

            var prod01 = Math.TwoProd(a.x0, b.x1);
            var prod10 = Math.TwoProd(a.x1, b.x0);

            var prod02 = Math.TwoProd(a.x0, b.x2);
            var prod11 = Math.TwoProd(a.x1, b.x1);
            var prod20 = Math.TwoProd(a.x2, b.x0);

            var prod03 = Math.TwoProd(a.x0, b.x3);
            var prod12 = Math.TwoProd(a.x1, b.x2);
            var prod21 = Math.TwoProd(a.x2, b.x1);
            var prod30 = Math.TwoProd(a.x3, b.x0);

            var prod13 = Math.TwoProd(a.x1, b.x3);
            var prod22 = Math.TwoProd(a.x2, b.x2);
            var prod31 = Math.TwoProd(a.x3, b.x1);

            var sum1 = Math.ThreeSumThree(prod00.err, prod01.prod, prod10.prod);
            var sum2 = Math.SixSumThree(sum1.r1, prod01.err, prod10.err, prod02.prod, prod11.prod, prod20.prod);
            var sum3 = Math.NineSumTwo(sum1.r2, sum2.r1, prod02.err, prod11.err, prod20.err, prod03.prod, prod12.prod, prod21.prod, prod30.prod);
            var sum4 = Math.NineSumTwo(sum2.r2, sum3.r1, prod03.err, prod12.err, prod21.err, prod30.err, prod13.prod, prod22.prod, prod31.prod);

            return Renormalise(prod00.prod, sum1.r0, sum2.r0, sum3.r0, sum4.r0);
        }

        public static QuadDouble operator /(QuadDouble a, double b)
        {
            return a * (1 / b);
        }

        public static QuadDouble operator/(QuadDouble a, QuadDouble b)
        {
            double q0 = a.x0 / b.x0;
            QuadDouble r0 = a - (q0 * b);

            double q1 = r0.x0 / b.x0;
            QuadDouble r1 = r0 - (q1 * b);

            double q2 = r1.x0 / b.x0;
            QuadDouble r2 = r1 - (q2 * b);

            double q3 = r2.x0 / b.x0;
            QuadDouble r3 = r2 - (q3 * b);

            double q4 = r3.x0 / b.x0;

            return Renormalise(q0, q1, q2, q3, q4);
        }
    }
}
