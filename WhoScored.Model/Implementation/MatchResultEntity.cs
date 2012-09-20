using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhoScored.Model.Implementation
{
    public class MatchResultEntity : IMatchResult
    {
        public int MatchId { get; set; }
        public int MatchRound { get; set; }
        public int HomeTeamID { get; set; }
        public string HomeTeamName { get; set; }
        public string HomeTeamGoals { get; set; }
        public int AwayTeamID { get; set; }
        public string AwayTeamName { get; set; }
        public string AwayTeamGoals { get; set; }
    }
}
