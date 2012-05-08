using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhoScored.Models
{
    using WhoScored.Model;

    public class LeagueDetails : IWorldDetails 
    {
        public int LeagueID { get; set; }

        public string LeagueName { get; set; }

        public string EnglishName { get; set; }

        public int NumberOfLevels { get; set; }

        public bool LeagueInWhoScored { get; set; }
    }
}