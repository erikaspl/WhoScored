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
}