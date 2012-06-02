namespace WhoScored.Model
{
    public interface IMatchInjuries
    {
        string InjuryPlayerID { get; set; }

        string InjuryPlayerName { get; set; }

        string InjuryTeamID { get; set; }

        string InjuryType { get; set; }

        string InjuryMinute { get; set; }
    }
}