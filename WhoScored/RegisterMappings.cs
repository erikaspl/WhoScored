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

            WhoScoredRepository.MapMatchDetails<MatchDetails, Models.MatchArena, Models.MatchTeam, MatchScorers,
                MatchBookings, MatchInjuries, MatchEventList>();

            WhoScoredRepository.MapSeriesFixtures<SeriesFixturesSummaryEntity, MatchSummaryEntity>();
            WhoScoredRepository.MapSettings<Models.Settings>();
            WhoScoredRepository.MapWorldDetails<CountryDetails>();
        }
    }
}