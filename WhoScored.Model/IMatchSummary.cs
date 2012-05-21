using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhoScored.Model
{
    public interface IMatchSummary
    {
        int MatchID { get; set; }
        int MatchRound { get; set; }
        int HomeTeamID { get; set; }
        string HomeTeamName { get; set; }
        int AwayTeamID { get; set; }
        string AwayTeamName { get; set; }
        DateTime MatchDate { get; set; }
        int HomeGoals { get; set; }
        int AwayGoals { get; set; }
    }
}
