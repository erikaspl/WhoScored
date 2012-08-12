using System.Collections.Generic;
using WhoScored.Model;

namespace WhoScored.Db
{
    public interface IWhoScoredRepository
    {
        void SaveWorldDetails<T>(List<T> worldDetails) where T : class, IWorldDetails;

        void SaveWorldDetails<T>(T worldDetail) where T : class, IWorldDetails;

        List<T> GetWorldDetails<T>() where T : class, IWorldDetails;

        List<T> GetActiveCountries<T>() where T : class, IWorldDetails;

        T GetWorldDetails<T>(int countryId) where T : class, IWorldDetails;

        void DropWorldDetails();

        void SaveSettings<T>(T settings) where T : class, ISettings;

        T GetSettings<T>() where T : class, ISettings;

        void SaveSeriesDetails<T>(List<T> leagueDetails) where T : class, ILeagueDetails;

        void SaveSeriesDetails<T>(T leagueDetail) where T : class, ILeagueDetails;

        List<T> GetSeriesDetails<T>() where T : class, ILeagueDetails;

        List<T> GetSeriesDetails<T>(string countryId) where T : class, ILeagueDetails;

        void DropSeriesDetails();

        void SaveSeriesFixtures<T>(List<T> seriesFixtures)  where T : class, ISeriesFixtures;

        void SaveSeriesFixtures<T>(T seriesFixture) where T : class, ISeriesFixtures;            

        List<T> GetSeriesFixturesSummary<T>() where T : class, ISeriesFixtures;

        T GetSeriesFixturesSummary<T, Y>(int leagueId, int season) where T : class, ISeriesFixtures where Y : class, IMatch;

        void DropSeriesFixtures();

        void SaveMatchDetails<T>(List<T> matchDetails) where T : class, IMatch;

        void SaveMatchDetails<T>(T matchDetails) where T : class, IMatch;

        List<T> GetMatchDetails<T>() where T : class, IMatch;

        void DropMatchDetails();

        List<ISeriesStandingsTeam> GetSeriesStandingsWithResults(int seriesId, int season, int matchRound);

        List<IMatchResult> GetSeriesResults(int seriesId, int season);

        List<IMatchResult> GetSeriesResults(int seriesId, int season, int matchRound);

    }
}
