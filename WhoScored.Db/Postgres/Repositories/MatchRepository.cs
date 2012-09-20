using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using WhoScored.Db.Postgres.Repositories;
using WhoScored.Model;
using WhoScored.Model.Repositories;

namespace WhoScored.Db.Postgres
{
    using global::NHibernate;

    public class MatchRepository : NHRepository<Match>, IMatchRepository
    {
        public MatchRepository(ISession session) : base(session)
        {
            
        }

        public IQueryable<Match> GetAllMatchesForSeries(int htSeriesId)
        {
            var requestedSeries =
                (from series in Session.QueryOver<Series>()
                 where series.HtSeriesId == htSeriesId
                 select series).SingleOrDefault();

            var matchQuery = (from matches in Session.Query<Match>()
                              where matches.Series.Id == requestedSeries.Id
                              select matches);
            return matchQuery;
        }
    }
}
