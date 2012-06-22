using WhoScored.Model;

namespace WhoScored.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class MatchDetails : IMatch
    {
        public string MatchSeason { get; set; }

        public string MatchID { get; set; }

        public string MatchType { get; set; }

        public string MatchDate { get; set; }

        public string FinishedDate { get; set; }

        public IMatchTeam MatchHomeTeam { get; set; }

        public IMatchTeam MatchAwayTeam { get; set; }

        public IMatchArena MatchArena { get; set; }

        public List<IMatchScorers> MatchScorers { get; set; }

        public List<IMatchBookings> MatchBookings { get; set; }

        public List<IMatchInjuries> MatchInjuries { get; set; }

        public string PossessionFirstHalfHome { get; set; }

        public string PossessionFirstHalfAway { get; set; }

        public string PossessionSecondHalfHome { get; set; }

        public string PossessionSecondHalfAway { get; set; }

        public List<IMatchEventList> MatchEventList { get; set; }
    }

    public class MatchTeam : IMatchTeam
    {
        public string TeamID { get; set; }

        public string TeamName { get; set; }

        public string DressURI { get; set; }

        public string Formation { get; set; }

        public string Goals { get; set; }

        public string TacticType { get; set; }

        public string TacticSkill { get; set; }

        public string RatingMidfield { get; set; }

        public string RatingRightDef { get; set; }

        public string RatingMidDef { get; set; }

        public string RatingLeftDef { get; set; }

        public string RatingRightAtt { get; set; }

        public string RatingMidAtt { get; set; }

        public string RatingLeftAtt { get; set; }

        public string RatingIndirectSetPiecesDef { get; set; }

        public string RatingIndirectSetPiecesAtt { get; set; }
    }

    public class MatchArena : IMatchArena
    {
        public string ArenaID { get; set; }

        public string ArenaName { get; set; }

        public string WeatherID { get; set; }

        public string SoldTotal { get; set; }

        public string SoldTerraces { get; set; }

        public string SoldBasic { get; set; }

        public string SoldRoof { get; set; }

        public string SoldVIP { get; set; }
    }

    public class MatchScorers : IMatchScorers
    {
        public string ScorerPlayerID { get; set; }

        public string ScorerPlayerName { get; set; }

        public string ScorerTeamID { get; set; }

        public string ScorerHomeGoals { get; set; }

        public string ScorerAwayGoals { get; set; }

        public string ScorerMinute { get; set; }
    }

    public class MatchBookings : IMatchBookings
    {
        public string BookingPlayerID { get; set; }

        public string BookingPlayerName { get; set; }

        public string BookingTeamID { get; set; }

        public string BookingType { get; set; }

        public string BookingMinute { get; set; }
    }

    public class MatchInjuries : IMatchInjuries
    {
        public string InjuryPlayerID { get; set; }

        public string InjuryPlayerName { get; set; }

        public string InjuryTeamID { get; set; }

        public string InjuryType { get; set; }

        public string InjuryMinute { get; set; }
    }

    public class MatchEventList : IMatchEventList
    {
        public string Minute { get; set; }

        public string SubjectPlayerID { get; set; }

        public string SubjectTeamID { get; set; }

        public string ObjectPlayerID { get; set; }

        public string EventTypeID { get; set; }

        public string EventVariation { get; set; }

        public string EventText { get; set; }
    }
}