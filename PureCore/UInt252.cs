using System;
using System.Collections.Generic;
using System.Text;

namespace Pure
{
    public class UInt252
    {
        private UInt256 contents;
        private byte[] mask = { 0x0f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
        public UInt252()
            : this(null)
        {
        }

        public UInt252(UInt256 value)
        {
            if (value != null)
            {
                byte[] byValue = value.ToArray();
                byValue[31] = (byte)(byValue[31] & 0x0F);
                contents = new UInt256(value.ToArray());
            }
            else
            {
                contents = new UInt256();
            }
        }

        public byte[] ToArray()
        {
            return contents.ToArray();
        }
    }
}
