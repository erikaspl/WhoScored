using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using WhoScored.Model;
using WhoScored.Model.Implementation;

namespace WhoScored.Db.Postgres.Repositories
{
    using System.Globalization;

    using WhoScored.Model.Repositories;

    using global::NHibernate.Linq;

    public class SeriesRepository : NHRepository<Series>, ISeriesRepository
    {
        public SeriesRepository(ISession session) : base(session)
        {
        }

        public void SaveUpdateSeries(IList<Series> series)
        {
            using (Session.BeginTransaction())
            {
                foreach (var league in series)
                {
                    Session.SaveOrUpdate(league);
                }
                Session.Transaction.Commit();
            }
        }

        public bool HasFixtures(int htSeriesId, short season)
        {
            var fixtureCount =
                Session.CreateQuery(@"select count(f)
                                    from Series s join s.SeriesFixtures f
                                    where s.HtSeriesId = :htSeriesId and f.Season = :season")
                        .SetParameter("htSeriesId", htSeriesId)
                        .SetParameter("season", season)
                        .UniqueResult<long>();

            return fixtureCount > 0;
        }

        public IQueryable<Series> GetAllSeriesForCountry(int htCountryId)
        {
            var requestedCountry =
                (from country in Session.QueryOver<Country>()
                 where country.HtCountryId == htCountryId
                 select country).SingleOrDefault();

            var seriesQuery = (from series in Session.Query<Series>()
                               where series.Country.CountryId == requestedCountry.CountryId
                               select series);
            return seriesQuery;

        }

        public IList<SeriesFixture> GetSeriesFixtureForSeason(int htSeriesId, int season)
        {
             var fixtures = Session.CreateCriteria<SeriesFixture>()
                 .SetFetchMode("HomeTeam", FetchMode.Eager)
                 .SetFetchMode("AwayTeam", FetchMode.Eager)
                .Add(Restrictions.Eq("Season", (short)season))
                .CreateCriteria("Series")
                    .Add(Restrictions.Eq("HtSeriesId", htSeriesId))
                .List<SeriesFixture>();

             return fixtures;
        }

        public IList<IMatchResult> GetSeriesResults(int seriesId, short season)
        {
            var matches = Session.CreateCriteria<Match>()
                .SetFetchMode("MatchHomeTeam", FetchMode.Eager)
                .SetFetchMode("MatchAwayTeam", FetchMode.Eager)
                .SetFetchMode("MatchHomeTeam.Team", FetchMode.Eager)
                .SetFetchMode("MatchAwayTeam.Team", FetchMode.Eager)
                .Add(Restrictions.Eq("MatchSeason", season))
                .CreateCriteria("Series")
                    .Add(Restrictions.Eq("HtSeriesId", seriesId))                    
                .List<Match>();

            var matchResults = new List<IMatchResult>();

            matchResults.AddRange(matches.Select(GetMatchResultsEntity));

            return matchResults;
        }

        public IList<IMatchResult> GetSeriesResults(int seriesId, short season, short matchRound)
        {
            var matches = Session.CreateCriteria<Match>()
                .SetFetchMode("MatchHomeTeam", FetchMode.Eager)
                .SetFetchMode("MatchAwayTeam", FetchMode.Eager)
                .SetFetchMode("MatchHomeTeam.Team", FetchMode.Eager)
                .SetFetchMode("MatchAwayTeam.Team", FetchMode.Eager)
                .Add(Restrictions.Eq("MatchSeason", season))
                .Add(Restrictions.Eq("MatchRound", matchRound))
                .CreateCriteria("Series")
                    .Add(Restrictions.Eq("HtSeriesId", seriesId))
                .List<Match>();

            var matchResults = new List<IMatchResult>();

            matchResults.AddRange(matches.Select(GetMatchResultsEntity));

            return matchResults;
        }

        private IMatchResult GetMatchResultsEntity(Match match)
        {
            return new MatchResultEntity
            {
                MatchId = match.MatchId,
                MatchRound = match.MatchRound,
                HomeTeamID = match.MatchHomeTeam.Team.TeamId,
                HomeTeamName = match.MatchHomeTeam.Team.TeamName,
                HomeTeamGoals = match.MatchHomeTeam.Goals.ToString(CultureInfo.InvariantCulture),

                AwayTeamID = match.MatchAwayTeam.Team.TeamId,
                AwayTeamName = match.MatchAwayTeam.Team.TeamName,
                AwayTeamGoals = match.MatchAwayTeam.Goals.ToString(CultureInfo.InvariantCulture),
            };
        }
    }
}
