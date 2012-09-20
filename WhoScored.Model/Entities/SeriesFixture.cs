using System.Collections.Generic; 
using System.Text; 
using System; 


namespace WhoScored.Model {
    
    public class SeriesFixture {
        public SeriesFixture() { }
        public virtual int MatchId { get; set; }
        public virtual int HtMatchId { get; set; }
        public virtual Team HomeTeam { get; set; }
        public virtual Team AwayTeam { get; set; }
        public virtual Series Series { get; set; }
        public virtual System.Nullable<short> AwayGoals { get; set; }
        public virtual System.DateTime MatchDate { get; set; }
        public virtual System.Nullable<short> HomeGoals { get; set; }
        public virtual short Season { get; set; }
        public virtual int MatchRound { get; set; }
        public virtual bool IsMatchMigrated
        {
            get { return MigratedMatchCount > 0; }
            set{}
        }
        public virtual int MigratedMatchCount { get; set; }
    }
}
