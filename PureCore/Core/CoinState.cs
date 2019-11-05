using System;

namespace Pure.Core
{
    [Flags]
    public enum CoinState : byte
    {
        Unconfirmed = 0,

        Confirmed = 1 << 0, //1
        Spent = 1 << 1, //2
        //Vote = 1 << 2,
        Claimed = 1 << 3,   //8
        Locked = 1 << 4,    //16
        Frozen = 1 << 5,    //32
        WatchOnly = 1 << 6, //64
    }
}
