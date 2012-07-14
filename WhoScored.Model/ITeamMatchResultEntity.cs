namespace WhoScored.Model
{
    public interface ITeamMatchResultEntity
    {
        int MatchRound { get; set; }
        string HomeTeamName { get; set; }
        string HomeTeamGoals { get; set; }
        string AwayTeamName { get; set; }
        string AwayTeamGoals { get; set; }
        string ResultSymbol { get; set; }
        int PositionChange { get; set; }
    }
}