using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhoScored.Model
{
    public interface IMatchResult
    {
        int MatchId { get; set; }
        int MatchRound { get; set; }
        int HomeTeamID { get; set; }
        string HomeTeamName { get; set; }
        string HomeTeamGoals { get; set; }
        int AwayTeamID { get; set; }
        string AwayTeamName { get; set; }
        string AwayTeamGoals { get; set; }
    }
}
