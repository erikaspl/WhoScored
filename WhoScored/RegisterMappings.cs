using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhoScored
{
    using WhoScored.Db.Mongo;
    using WhoScored.Model;
    using WhoScored.Models;

    public class RegisterMappings
    {
        public static void RegisterModelClassMap()
        {
            WhoScoredRepository.MapLeagueDetails<SeriesDetails>();

            WhoScoredRepository.MapMatchDetails<MatchDetails, MatchArena, MatchTeam, MatchScorers,
                MatchBookings, MatchInjuries, MatchEventList>();

            WhoScoredRepository.MapSeriesFixtures<SeriesFixturesSummaryEntity, MatchSummaryEntity>();
            WhoScoredRepository.MapSettings<Settings>();
            WhoScoredRepository.MapWorldDetails<WorldDetails>();
        }
    }
}