using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhoScored.Model;

namespace WhoScored.Models
{
    public class LeagueDetails : IWorldDetails 
    {
        public int LeagueID { get; set; }

        public string LeagueName { get; set; }

        public string EnglishName { get; set; }

        public int NumberOfLevels { get; set; }

        public int SeasonOffset { get; set; }

        public bool LeagueInWhoScored { get; set; }

    }
}