using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhoScored.Model
{
    public interface IWorldDetails
    {
        int LeagueId { get; set; }
        string LeagueName { get; set; }
        string EnglishName { get; set; }
        int NumbOfLevels { get; set; }
    }
}
