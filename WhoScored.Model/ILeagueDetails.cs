using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhoScored.Model
{
    public interface ILeagueDetails
    {
        int LeagueID { get; set; }
        int LeagueLevel { get; set; }
        string LeagueLevelUnitName { get; set; }
        int LeagueLevelUnitID { get; set; }
    }
}
