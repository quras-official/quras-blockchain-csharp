using System;
using System.Linq;
using System.Collections.Generic;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Quras;
using Quras.Core.RingCT.Types;
using Quras.Core.RingCT.Impls;
using Quras.Cryptography.ECC;

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
