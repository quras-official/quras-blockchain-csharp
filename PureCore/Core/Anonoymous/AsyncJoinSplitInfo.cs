using System;
using System.Collections.Generic;
using System.Text;

namespace Pure.Core.Anonoymous
{
    public class AsyncJoinSplitInfo
    {
        public List<JSInput> vjsin;
        public List<JSOutput> vjsout;
        public List<Note> notes;

        public Fixed8 vpub_old;
        public Fixed8 vpub_new;

        public AsyncJoinSplitInfo()
        {
            vjsin = new List<JSInput>();
            vjsout = new List<JSOutput>();
            notes = new List<Note>();

            vpub_old = new Fixed8();
            vpub_new = new Fixed8();
        }
    }
}
