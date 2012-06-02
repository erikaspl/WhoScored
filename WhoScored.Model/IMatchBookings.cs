namespace WhoScored.Model
{
    public interface IMatchBookings
    {
        string BookingPlayerID { get; set; }

        string BookingPlayerName { get; set; }

        string BookingTeamID { get; set; }

        string BookingType { get; set; }

        string BookingMinute { get; set; }
    }
}