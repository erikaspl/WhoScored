namespace WhoScored.Model
{
    public interface IMatchEventList
    {
        string Minute { get; set; }

        string SubjectPlayerID { get; set; }

        string SubjectTeamID { get; set; }

        string ObjectPlayerID { get; set; }

        string EventTypeID { get; set; }

        string EventVariation { get; set; }

        string EventText { get; set; }
    }
}