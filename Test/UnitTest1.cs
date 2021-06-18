using NUnit.Framework;
using Quad_Double;
using System;

namespace Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            QuadDouble x = 0.7;
            QuadDouble y = 0.6;
            QuadDouble z = x / y;

        }
        [Test]
        public void Pow()
        {
            QuadDouble x = 1.57;
            var test0 = QuadDouble.Pow(x, 13);
            var test1 = QuadDouble.Pow(x, -13);
            var test2 = QuadDouble.Root(x, 3);
            var test3 = QuadDouble.Root(x, -3);
        }
    }
}