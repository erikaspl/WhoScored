namespace WhoScored.Model
{
    public class SeriesStandingsTeamEntity
    {
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
    }
}
