using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhoScored.Model
{
    public interface IMatchArena
    {
        string ArenaID { get; set; }

        string ArenaName { get; set; }

        string WeatherID { get; set; }

        string SoldTotal { get; set; }

        string SoldTerraces { get; set; }

        string SoldBasic { get; set; }

        string SoldRoof { get; set; }

        string SoldVIP { get; set; }
    }
}
