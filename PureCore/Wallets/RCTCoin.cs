using Pure.Core;
using Pure.IO.Caching;

using System;

namespace Pure.Wallets
{
    public class RCTCoin : IEquatable<RCTCoin>, ITrackable<RCTCoinReference>
    {
        public RCTCoinReference Reference;
        public RCTransactionOutput Output;

        RCTCoinReference ITrackable<RCTCoinReference>.Key => Reference;

        TrackState ITrackable<RCTCoinReference>.TrackState { get; set; }

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
                    ITrackable<RCTCoinReference> _this = this;
                    if (_this.TrackState == TrackState.None)
                        _this.TrackState = TrackState.Changed;
                }
            }
        }

        public void SetTrackState(TrackState state)
        {
            ITrackable<RCTCoinReference> _this = this;
            _this.TrackState = state;
        }

        public bool Equals(RCTCoin other)
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
