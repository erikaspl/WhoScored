using System.Collections.Generic;
using System.Linq;

namespace WhoScored.Model.Repositories
{
    public interface ISeriesRepository
    {
        void SaveUpdateSeries(IList<Series> series);

        bool HasFixtures(int htSeriesId, short season);

        IQueryable<Series> GetAllSeriesForCountry(int htCountryId);

        IList<SeriesFixture> GetSeriesFixtureForSeason(int htSeriesId, int season);

        IList<IMatchResult> GetSeriesResults(int seriesId, short season);

        IList<IMatchResult> GetSeriesResults(int seriesId, short season, short matchRound);

    }
}
