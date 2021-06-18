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
            QuadDouble quad1 = new QuadDouble(1);
            for(QuadDouble i = 0.5; i.x0 > 0.0000000000000000000000000000000000000000000000000001; i *= 0.7)
            {
                Console.WriteLine($"{i.x0}   {i.x1}   {i.x2}   {i.x3}");
            }
        }
    }
}