using System;
using System.Numerics;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Pure;
using Pure.Core.RingCT.Impls;

namespace Quras_core_test
{
    [TestClass]
    public class UT_Fixed8
    {
        [TestMethod]
        public void TestBigInteger()
        {
            BigInteger aa = -1000;
            BigInteger bb = 100;
            BigInteger cc = bb + aa;

            byte[] dd = aa.ToByteArray();

            Fixed8 vPub = Fixed8.One * -100;
            byte [] ee = vPub.ToBinaryFormat().ToBinary();
        }

        [TestMethod]
        public void Basic_equality()
        {
            var a = Fixed8.FromDecimal(1.23456789m);
            var b = Fixed8.Parse("1.23456789");
            a.Should().Be(b);
        }

        [TestMethod]
        public void Can_parse_exponent_notation()
        {
            Fixed8 expected = Fixed8.FromDecimal(1.23m);
            Fixed8 actual = Fixed8.Parse("1.23E-0");
            actual.Should().Be(expected);
        }

        [TestMethod]
        public void Can_multiply_with_presicion()
        {
            decimal a = 123456789m;
            decimal b = 1.23456789m;
            decimal expected = a * b;
            Fixed8 af = Fixed8.FromDecimal(123456789m);
            Fixed8 bf = Fixed8.FromDecimal(1.23456789m);
            Fixed8 actual = af * bf;
            ((decimal)actual).Should().Be(expected);
        }

        [TestMethod]
        public void Can_multiply_without_overflow()
        {
            decimal a = Math.Round(((decimal)Fixed8.MaxValue - 1m) / 2m, 8);
            decimal b = 2m;
            decimal expected = a * b;
            Fixed8 af = Fixed8.FromDecimal(a);
            Fixed8 bf = Fixed8.FromDecimal(b);
            Fixed8 actual = af * bf;
            ((decimal)actual).Should().Be(expected);
        }
    }
}
