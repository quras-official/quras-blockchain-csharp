using Pure.Core;
using Pure.IO.Caching;
using Pure.Core.Anonoymous;
using PureCore.Wallets.AnonymousKey.Key;
using System;

namespace Pure.Wallets
{
    public class JSCoin : IEquatable<JSCoin>, ITrackable<JSCoinReference>
    {
        public JSCoinReference Reference;
        public JSTransactionOutput Output;

        private string _address = null;
        public string Address
        {
            get
            {
                if (_address == null)
                {
                    _address = Wallet.ToAnonymousAddress(Output.addr);
                }
                return _address;
            }
        }

        public UInt256 Nullifier(SpendingKey key)
        {
            Note nt = new Note(Output.addr.a_pk, Output.Value, Output.rho, Output.r, Output.AssetId);
            return nt.Nullifier(key);
        }

        JSCoinReference ITrackable<JSCoinReference>.Key => Reference;

        private CoinState state;
        public CoinState State
        {
            get
            {
                return state;
            }
            set
            {
                if (state != value)
                {
                    state = value;
                    ITrackable<JSCoinReference> _this = this;
                    if (_this.TrackState == TrackState.None)
                        _this.TrackState = TrackState.Changed;
                }
            }
        }

        TrackState ITrackable<JSCoinReference>.TrackState { get; set; }

        public bool Equals(JSCoin other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;
            return Reference.Equals(other.Reference);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as JSCoin);
        }

        public override int GetHashCode()
        {
            return Reference.GetHashCode();
        }
    }
}
