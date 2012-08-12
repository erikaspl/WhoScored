using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhoScored.Model.Implementation
{
    public class TeamMatchResultEntity : MatchResultEntity, ITeamMatchResult
    {
        public string ResultSymbol { get; set; }
        public int PositionChange { get; set; }
    }
}
