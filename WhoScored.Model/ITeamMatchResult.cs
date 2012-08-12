namespace WhoScored.Model
{
    public interface ITeamMatchResult : IMatchResult
    {
        string ResultSymbol { get; set; }
        int PositionChange { get; set; }
    }
}