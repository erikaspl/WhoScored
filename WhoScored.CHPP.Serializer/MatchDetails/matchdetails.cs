using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhoScored.Model;
using System.Xml.Serialization;

namespace WhoScored.CHPP.MatchDetails.Serializer
{

    public partial class HattrickDataMatch : IMatch
    {
        [XmlIgnore]
        public string MatchSeason { get; set; }

        [XmlIgnore]
        public IMatchTeam MatchHomeTeam
        {
            get { return HomeTeam.First(); }
            set {}
        }

        [XmlIgnore]
        public IMatchTeam MatchAwayTeam
        {
            get { return AwayTeam.First(); }
            set { }
        }

        [XmlIgnore]
        public IMatchArena MatchArena
        {
            get { return Arena.First(); }
            set { }
        }

        [XmlIgnore]
        public List<IMatchScorers> MatchScorers
        {
            get { return Scorers.Cast<IMatchScorers>().ToList(); }
            set { }
        }

        [XmlIgnore]
        public List<IMatchBookings> MatchBookings
        {
            get { return Bookings.Cast<IMatchBookings>().ToList(); }
            set { }
        }

        [XmlIgnore]
        public List<IMatchInjuries> MatchInjuries
        {
            get { return Injuries.Cast<IMatchInjuries>().ToList(); }
            set { }
        }

        [XmlIgnore]
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
        [XmlIgnore]
        public string TeamID
        {
            get{ return awayTeamIDField; }
            set{ awayTeamIDField = value; }
        }

        [XmlIgnore]
        public string TeamName
        {
            get { return awayTeamNameField; }
            set { awayTeamNameField = value; }
        }

        [XmlIgnore]
        public string Goals
        {
            get { return awayGoalsField; }
            set { awayGoalsField = value; }
        }
    }

    public partial class HattrickDataMatchHomeTeam : IMatchTeam
    {
        [XmlIgnore]
        public string TeamID
        {
            get { return homeTeamIDField; }
            set { homeTeamIDField = value; }
        }

        [XmlIgnore]
        public string TeamName
        {
            get { return HomeTeamName; }
            set { HomeTeamName = value; }
        }

        [XmlIgnore]
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
