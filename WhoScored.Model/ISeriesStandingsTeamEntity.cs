using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhoScored.Model
{
    public interface ISeriesStandingsTeamEntity
    {
        int Position { get; set; }
        string TeamId { get; set; }
        string TeamName { get; set; }
        int GoalsScored { get; set; }
        int GoalsConceded { get; set; }
        int GoalDifference { get; set; }
        int HomePoints { get; set; }
        int AwayPoints { get; set; }
        int TotalPoints { get; set; }
        int Won { get; set; }
        int Lost { get; set; }
        int Drawn { get; set; }
        int Played { get; set; }
        string Form { get; }

        List<ITeamMatchResultEntity> Results { get; set; }
    }
}
