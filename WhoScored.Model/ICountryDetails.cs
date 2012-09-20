using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhoScored.Model
{
    public interface ICountryDetails
    {
        int LeagueID { get; set; }
        string LeagueName { get; set; }
        string EnglishName { get; set; }
        int NumberOfLevels { get; set; }
        int SeasonOffset { get; set; }

        bool LeagueInWhoScored { get; set; }
        List<int> SeriesIdList { get; set; }
    }
}
