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
            QuadDouble quad1 = new QuadDouble(10);
            for(double i = 0.000000000000000000000000000000000000000000000000000000000000000000000000000000000000001; i < 1.1; i *= 10)
            {
                quad1 += new QuadDouble(i);
            }
        }
    }
}