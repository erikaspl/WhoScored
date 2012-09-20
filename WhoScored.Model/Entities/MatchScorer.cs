using System.Collections.Generic; 
using System.Text; 
using System;


namespace WhoScored.Model
{
    
    public class MatchScorer {
        public MatchScorer() { }
        public virtual int MatchScorerId { get; set; }
        public virtual Match Match { get; set; }
        public virtual MatchTeam MatchTeam { get; set; }
        public virtual int PlayerId { get; set; }
        public virtual short EventIndex { get; set; }
        public virtual short TeamGoals { get; set; }
        public virtual short ScorerMinute { get; set; }
        public virtual string PlayerName { get; set; }
        public virtual short OppositionGoals { get; set; }


    }
}
