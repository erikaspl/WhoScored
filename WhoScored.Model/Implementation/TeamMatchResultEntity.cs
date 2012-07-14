using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhoScored.Model.Implementation
{
    public class TeamMatchResultEntity : ITeamMatchResultEntity
    {
        public int MatchRound { get; set; }
        public string HomeTeamName { get; set; }
        public string HomeTeamGoals { get; set; }
        public string AwayTeamName { get; set; }
        public string AwayTeamGoals { get; set; }
        public string ResultSymbol { get; set; }
        public int PositionChange { get; set; }
    }
}
