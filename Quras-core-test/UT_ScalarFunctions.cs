using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Pure;
using Pure.Core.RingCT.Impls;

namespace Quras_core_test
{
    [TestClass]
    public class UT_ScalarFunctions
    {
        [TestMethod]
        public void ScalarLCheck()
        {
            string strExpect = "1000000000000000000000000000000014def9dea2f79cd65812631a5cf5d3ed";
            BigInteger expectedL = new BigInteger(strExpect.HexToBytes().Reverse().Concat(new byte[0]).ToArray());
            ScalarFunctions.l.Should().Be(expectedL);
        }
    }
}
