using System.Security.Cryptography;
using PureCore.Wallets.AnonymousKey.Key;

using Pure;

namespace Pure.Core.Anonoymous
{
    public class JSInput
    {
        public Note note;
        public SpendingKey key;
        public byte[] witness;
        public UInt256 AssetID;

        public JSInput()
        {
            byte[] random_byte256 = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random_byte256);
            }
            key = new SpendingKey(new UInt256(random_byte256));

            note = new Note(key.address().a_pk, new Fixed8(0), UInt256.Random(), UInt256.Random(), UInt256.Random());

            witness = new byte[0];
            AssetID = new UInt256();
        }

        public UInt256 Nullifier()
        {
            return note.Nullifier(key);
        }
    }
}
