using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhoScored.Model;

namespace WhoScored.CHPP.MatchDetails.Serializer
{
    public partial class HattrickDataMatch : IMatch
    {
        public IMatchTeam MatchHomeTeam
        {
            get { return HomeTeam.First(); }
            set {}
        }

        public IMatchTeam MatchAwayTeam
        {
            get { return AwayTeam.First(); }
            set { }
        }

        public IMatchArena MatchArena
        {
            get { return Arena.First(); }
            set { }
        }

        public List<IMatchScorers> MatchScorers
        {
            get { return Scorers.Cast<IMatchScorers>().ToList(); }
            set { }
        }

        public List<IMatchBookings> MatchBookings
        {
            get { return Bookings.Cast<IMatchBookings>().ToList(); }
            set { }
        }

        public List<IMatchInjuries> MatchInjuries
        {
            get { return Injuries.Cast<IMatchInjuries>().ToList(); }
            set { }
        }

        public List<IMatchEventList> MatchEventList
        {
            get { return EventList.Cast<IMatchEventList>().ToList(); }
            set { }
        }
    }

    public partial class HattrickDataMatchArena : IMatchArena
    {
        
    }

    public partial class HattrickDataMatchAwayTeam : IMatchTeam
    {
        public string TeamID
        {
            get{ return awayTeamIDField; }
            set{ awayTeamIDField = value; }
        }

        public string TeamName
        {
            get { return awayTeamNameField; }
            set { awayTeamNameField = value; }
        }

        public string Goals
        {
            get { return awayGoalsField; }
            set { awayGoalsField = value; }
        }
    }

    public partial class HattrickDataMatchHomeTeam : IMatchTeam
    {
        public string TeamID
        {
            get { return homeTeamIDField; }
            set { homeTeamIDField = value; }
        }

        public string TeamName
        {
            get { return HomeTeamName; }
            set { HomeTeamName = value; }
        }

        public string Goals
        {
            get { return HomeGoals; }
            set { HomeGoals = value; }
        }
    }

    public partial class HattrickDataMatchBookingsBooking : IMatchBookings
    {
        
    }

    public partial class HattrickDataMatchEventListEvent : IMatchEventList
    {
         
    }

    public partial class HattrickDataMatchInjuriesInjury : IMatchInjuries
    {
        
    }

    public partial class HattrickDataMatchScorersGoal : IMatchScorers
    {
        
    }
}
