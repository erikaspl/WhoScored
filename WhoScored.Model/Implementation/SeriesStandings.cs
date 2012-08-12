using System.Collections.Generic;

namespace WhoScored.Model
{
    using System.Linq;

    public class SeriesStandingsTeamEntity : ISeriesStandingsTeam
    {
        public int Position { get; set; }
        public string TeamId { get; set; }
        public string TeamName { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsConceded { get; set; }
        public int GoalDifference { get; set; }
        public int HomePoints { get; set; }
        public int AwayPoints { get; set; }
        public int TotalPoints { get; set; }
        public int Won { get; set; }
        public int Lost { get; set; }
        public int Drawn { get; set; }
        public int Played { get; set; }

        private List<ITeamMatchResult> _results = new List<ITeamMatchResult>();
        public List<ITeamMatchResult> Results
        {
            get { return _results; }
            set { _results = value; }
        }
    }
}
