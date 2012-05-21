using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhoScored.Model
{
    public interface ISeriesFixtures
    {
        int LeagueLevelUnitID { get; set; }
        string LeagueLevelUnitName { get; set; }
        List<IMatchSummary> Matches { get; set; }
    }
}
