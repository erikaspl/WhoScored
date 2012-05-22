using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhoScored.Model
{
    public interface ISeriesFixtures
    {
        Guid Id { get; set; }
        int LeagueLevelUnitID { get; set; }
        int Season { get; set; }
        string LeagueLevelUnitName { get; set; }
        List<IMatchSummary> Matches { get; set; }

    }
}
