using System.Collections.Generic; 
using System.Text; 
using System;


namespace WhoScored.Model
{
    
    public class MatchArena {
        public virtual int MatchArenaId { get; set; }
        public virtual string ArenaName { get; set; }
        public virtual short WeatherId { get; set; }
        public virtual Nullable<short> SoldTotal { get; set; }
        public virtual int HtArenaId { get; set; }
    }
}
