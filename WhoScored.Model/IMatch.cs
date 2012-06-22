using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhoScored.Model
{
    public interface IMatch
    {
        string MatchSeason { get; set; }

        string MatchID { get; set; }

        string MatchType { get; set; }

        string MatchDate { get; set; }

        string FinishedDate { get; set; }

        IMatchTeam MatchHomeTeam { get; set; }

        IMatchTeam MatchAwayTeam { get; set; }

        IMatchArena MatchArena { get; set; }

        List<IMatchScorers> MatchScorers { get; set; }

        List<IMatchBookings> MatchBookings { get; set; }

        List<IMatchInjuries> MatchInjuries { get; set; }

        string PossessionFirstHalfHome { get; set; }

        string PossessionFirstHalfAway { get; set; }

        string PossessionSecondHalfHome { get; set; }

        string PossessionSecondHalfAway { get; set; }

        List<IMatchEventList> MatchEventList { get; set; }
    }
}
