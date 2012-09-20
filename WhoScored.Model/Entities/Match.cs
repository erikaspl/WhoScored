using System.Collections.Generic; 
using System.Text; 
using System;


namespace WhoScored.Model
{
    
    public class Match {
        public Match() {
			MatchBookings = new List<MatchBooking>();
			MatchEvents = new List<MatchEvent>();
			MatchInjuries = new List<MatchInjury>();
			MatchScorers = new List<MatchScorer>();
        }
        public virtual int MatchId { get; set; }
        public virtual int HtMatchId { get; set; }
        public virtual MatchArena MatchArena { get; set; }
        public virtual MatchTeam MatchHomeTeam { get; set; }
        public virtual MatchTeam MatchAwayTeam { get; set; }
        public virtual IList<MatchBooking> MatchBookings { get; set; }
        public virtual IList<MatchEvent> MatchEvents { get; set; }
        public virtual IList<MatchInjury> MatchInjuries { get; set; }
        public virtual IList<MatchScorer> MatchScorers { get; set; }
        public virtual Series Series { get; set; }
        public virtual short MatchSeason { get; set; }
        public virtual System.DateTime MatchDate { get; set; }
        public virtual short MatchRound { get; set; }
        public virtual System.Nullable<System.DateTime> FinishedDate { get; set; }
        public virtual short MatchType { get; set; }

        public virtual void SetMatchHomeTeam(MatchTeam matchTeam)
        {
            MatchHomeTeam = matchTeam;
            matchTeam.Matches.Add(this);
        }

        public virtual void SetMatchAwayTeam(MatchTeam matchTeam)
        {
            MatchAwayTeam = matchTeam;
            matchTeam.Matches.Add(this);
        }

        public virtual void AddHomeMatchScorer(MatchScorer scorer)
        {
            scorer.Match = this;
            MatchScorers.Add(scorer);
            MatchHomeTeam.AddMatchScorer(scorer);
        }

        public virtual void AddAwayMatchScorer(MatchScorer scorer)
        {
            scorer.Match = this;
            MatchScorers.Add(scorer);
            MatchAwayTeam.AddMatchScorer(scorer);
        }

        public virtual void AddHomeTeamMatchInjury(MatchInjury injury)
        {
            injury.Match = this;
            MatchInjuries.Add(injury);
            MatchHomeTeam.AddMatchInjury(injury);
        }

        public virtual void AddAwayTeamMatchInjury(MatchInjury injury)
        {
            injury.Match = this;
            MatchInjuries.Add(injury);
            MatchAwayTeam.AddMatchInjury(injury);
        }

        public virtual void AddHomeTeamMatchBooking(MatchBooking booking)
        {
            booking.Match = this;
            MatchBookings.Add(booking);
            MatchHomeTeam.AddMatchBooking(booking);
        }

        public virtual void AddAwayTeamMatchBooking(MatchBooking booking)
        {
            booking.Match = this;
            MatchBookings.Add(booking);
            MatchAwayTeam.AddMatchBooking(booking);
        }

        public virtual void AddMatchEvent(MatchEvent matchEvent)
        {
            matchEvent.Match = this;
            MatchEvents.Add(matchEvent);
        }
    }
}
