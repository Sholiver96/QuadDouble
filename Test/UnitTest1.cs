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
    }
}