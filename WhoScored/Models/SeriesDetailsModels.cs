using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhoScored.Model;

namespace WhoScored.Models
{
    public class SeriesDetails : ILeagueDetails
    {
        public int LeagueID { get; set; }
        public int LeagueLevel { get; set; }
        public string LeagueLevelUnitName { get; set; }
        public int LeagueLevelUnitID { get; set; }
    }


    public class TeamStandings
    {
        public int Position { get; set; }
        public string TeamName { get; set; }
        public int Played { get; set; }
        public int Won { get; set; }
        public int Drawn { get; set; }
        public int Lost { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsConceded { get; set; }
        public int GoalDifference { get; set; }
        public int TotalPoints { get; set; }
        public string Form { get; set; }
    }

}