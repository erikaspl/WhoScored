using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhoScored.Model;

namespace WhoScored.Db
{
    public interface IWhoScoredRepository
    {
        void SaveWorldDetails<T>(List<T> worldDetails) where T : class, IWorldDetails;

        void SaveWorldDetails<T>(T worldDetail) where T : class, IWorldDetails;

        List<T> GetWorldDetails<T>() where T : class, IWorldDetails;

        T GetWorldDetails<T>(int countryId) where T : class, IWorldDetails;

        void DropWorldDetails();

        void SaveSettings<T>(T settings) where T : class, ISettings;

        T GetSettings<T>() where T : class, ISettings;

        void SaveSeriesDetails<T>(List<T> leagueDetails) where T : class, ILeagueDetails;

        void SaveSeriesDetails<T>(T leagueDetail) where T : class, ILeagueDetails;

        List<T> GetSeriesDetails<T>() where T : class, ILeagueDetails;

        List<T> GetSeriesDetails<T>(string countryId) where T : class, ILeagueDetails;

        void DropSeriesDetails();

        void SaveSeriesFixtures<T, Y>(List<T> seriesFixtures) 
            where T : class, ISeriesFixtures
            where Y : class, IMatchSummary;

        void SaveSeriesFixtures<T, Y>(T seriesFixture)
            where T : class, ISeriesFixtures
            where Y : class, IMatchSummary;

        List<T> GetSeriesFixturesSummary<T, Y>()
            where T : class, ISeriesFixtures
            where Y : class, IMatchSummary;

        T GetSeriesFixturesSummary<T, Y>(int leagueId, int season)
            where T : class, ISeriesFixtures
            where Y : class, IMatchSummary;

        void DropSeriesFixtures();
    }
}
