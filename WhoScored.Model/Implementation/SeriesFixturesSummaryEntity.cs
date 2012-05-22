using System;
using System.Collections.Generic;

namespace WhoScored.Model
{
    public class SeriesFixturesSummaryEntity : ISeriesFixtures
    {
        public Guid Id { get; set; }

        public int LeagueLevelUnitID { get; set; }

        public int Season { get; set; }

        public string LeagueLevelUnitName { get; set; }
        public List<IMatchSummary> Matches { get; set; }
    }

    public class MatchSummaryEntity : IMatchSummary
    {
        public int MatchID { get; set; }
        public int MatchRound { get; set; }
        public int HomeTeamID { get; set; }
        public string HomeTeamName { get; set; }
        public int AwayTeamID { get; set; }
        public string AwayTeamName { get; set; }
        public DateTime MatchDate { get; set; }
        public int? HomeGoals { get; set; }
        public int? AwayGoals { get; set; }
    }
}
