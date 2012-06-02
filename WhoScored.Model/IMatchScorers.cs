namespace WhoScored.Model
{
    public interface IMatchScorers
    {
        string ScorerPlayerID { get; set; }

        string ScorerPlayerName { get; set; }

        string ScorerTeamID { get; set; }

        string ScorerHomeGoals { get; set; }

        string ScorerAwayGoals { get; set; }

        string ScorerMinute { get; set; }
    }
}