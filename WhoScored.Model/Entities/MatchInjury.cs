using System.Collections.Generic; 
using System.Text; 
using System;


namespace WhoScored.Model
{
    
    public class MatchInjury {
        public MatchInjury() { }
        public virtual int MatchInjuryId { get; set; }
        public virtual Match Match { get; set; }
        public virtual MatchTeam MatchTeam { get; set; }
        public virtual int PlayerId { get; set; }
        public virtual short InjuryMinute { get; set; }
        public virtual short EventIndex { get; set; }
        public virtual string PlayerName { get; set; }
        public virtual short InjuryType { get; set; }
    }
}
