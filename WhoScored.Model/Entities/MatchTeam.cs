using System.Collections.Generic; 
using System.Text; 
using System;


namespace WhoScored.Model
{
    
    public class MatchTeam {
        public MatchTeam()
        {
            MatchBookings = new List<MatchBooking>();
            MatchInjuries = new List<MatchInjury>();
            MatchScorers = new List<MatchScorer>();
            Matches = new List<Match>();
        }
        public virtual int MatchTeamId { get; set; }
        public virtual Team Team { get; set; }
        public virtual IList<MatchBooking> MatchBookings { get; set; }
        public virtual IList<MatchInjury> MatchInjuries { get; set; }
        public virtual IList<MatchScorer> MatchScorers { get; set; }
        public virtual IList<Match> Matches { get; set; }
        public virtual short RatingRightDef { get; set; }
        public virtual short? TacticType { get; set; }
        public virtual string DressURI { get; set; }
        public virtual short? TacticSkill { get; set; }
        public virtual short RatingMidAtt { get; set; }
        public virtual short RatingMidfield { get; set; }
        public virtual short? RatingIndirectSetPiecesDef { get; set; }
        public virtual string Formation { get; set; }
        public virtual short RatingLeftAtt { get; set; }
        public virtual short RatingRightAtt { get; set; }
        public virtual short Goals { get; set; }
        public virtual short PossessionFirstHalf { get; set; }
        public virtual short PossessionSecondHalf { get; set; }
        public virtual short? RatingIndirectSetPiecesAtt { get; set; }
        public virtual short RatingLeftDef { get; set; }
        public virtual short RatingMidDef { get; set; }

        public virtual void AddMatchBooking(MatchBooking booking)
        {
            booking.MatchTeam = this;
            MatchBookings.Add(booking);
        }

        public virtual void AddMatchScorer(MatchScorer scorer)
        {
            scorer.MatchTeam = this;
            MatchScorers.Add(scorer);
        }

        public virtual void AddMatchInjury(MatchInjury injury)
        {
            injury.MatchTeam = this;
            MatchInjuries.Add(injury);
        }

        public virtual void SetTeam(Team team)
        {
            Team = team;
            team.AddMatchTeam(this);
        }
    }
}
