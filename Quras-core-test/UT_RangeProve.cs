using System;
using System.Linq;
using System.Collections.Generic;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Pure;
using Pure.Core.RingCT.Types;
using Pure.Core.RingCT.Impls;
using Pure.Cryptography.ECC;

namespace Quras_core_test
{
    [TestClass]
    public class UT_RangeProve
    {
        [TestMethod]
        public void RangeProveWithAllowCase()
        {
            Fixed8 amount = new Fixed8(100);

            RangeProveType prove = RangeSignature.Generate(amount);
            RangeSignature.Verify(prove.C, prove.rangeSig).Should().Be(true);
        }
    }
}
