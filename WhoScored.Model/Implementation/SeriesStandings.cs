using System.Collections.Generic;

namespace WhoScored.Model
{
    using System.Linq;

    public class SeriesStandingsTeamEntity : ISeriesStandingsTeamEntity
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

        public string Form { 
            get
            {
                string form = string.Empty;
                _results.OrderBy(r => r.MatchRound).ToList().ForEach(r => form += r.ResultSymbol);
                return form;
            }
        }
        private List<ITeamMatchResultEntity> _results = new List<ITeamMatchResultEntity>();
        public List<ITeamMatchResultEntity> Results
        {
            get { return _results; }
            set { _results = value; }
        }
    }
}
