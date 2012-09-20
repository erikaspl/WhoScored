using System.Collections.Generic;
using System.Linq;

namespace WhoScored.IntegrationTest
{
    using Db.Postgres;
    using Model;
    using Xunit;

    public class MatchRepositoryTest
    {
        [Fact]
        public void MatchSaveTest()
        {
            const int countryId = 66;
            var country = TestEntities.CreateCountry(countryId, "countryEnglishName", "CountryName");
            var series = TestEntities.CreateSeries(1000, country, "A Lyga");
            var sessionFactory = SessionFactory.CreateSessionFactory(true);
            using (var session = sessionFactory.OpenSession())
            {
                session.SaveOrUpdate(country);
                session.SaveOrUpdate(series);
                session.Flush();
            }

            var match1 = TestEntities.CreateMatchFullData(1000, country, 1050, 1051,
                                                          new List<int> {10000, 10001, 10002}, 3000, 1);

            var match2 = TestEntities.CreateMatchFullData(1001, country, 1052, 1053,
                                              new List<int> { 10003, 10004, 10005 }, 3001, 1);

            series.AddMatch(match1);
            series.AddMatch(match2);

            var matchList = new List<Match> { match1, match2 };

            var prodSession = SessionManager.CurrentSession;
            var matchRepository = new MatchRepository(prodSession);

            using (var transaction = prodSession.BeginTransaction())
            {
                matchList.ForEach(matchRepository.SaveUpdate);
                transaction.Commit();
            }
        }

        [Fact]
        public void MatchLoadTest()
        {
            const int countryId = 66;
            const int seriesId = 1000;
            var country = TestEntities.CreateCountry(countryId, "EnglishName", "CountryName");

            List<Series> series = TestEntities.CreateSeries(seriesId, country);
            var sessionFactory = SessionFactory.CreateSessionFactory(true);
            using (var session = sessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    session.SaveOrUpdate(country);
                    foreach (var league in series)
                    {                        
                        session.SaveOrUpdate(league);
                    }
                    session.Transaction.Commit();
                }
            }

            
            var match1 = TestEntities.CreateMatchFullData(1000, country, 1050, 1051,
                                                          new List<int> {10000, 10001, 10002}, 3000, 1);

            var match2 = TestEntities.CreateMatchFullData(1001, country, 1052, 1053,
                                                          new List<int> {10003, 10004, 10005}, 3001, 1);

            series.First(s => s.HtSeriesId == seriesId).AddMatch(match1);
            series.First(s => s.HtSeriesId == seriesId).AddMatch(match2);

            var matchList = new List<Match> {match1, match2};

            using (var session = sessionFactory.OpenSession())
            {
                foreach (var match in matchList)
                {
                    using (session.BeginTransaction())
                    {
                        session.SaveOrUpdate(match);
                        session.Transaction.Commit();
                        session.Clear();
                    }
                }
            }

            var matchRepository = new MatchRepository(SessionManager.CurrentSession);
            var matches = matchRepository.GetAllMatchesForSeries(seriesId).ToList();
            Assert.True(matches.Count == 2);
            var firstMatch = matches.First(m => m.HtMatchId == 1000);
            Assert.True(firstMatch.MatchHomeTeam.Team.TeamId == 1050);
            Assert.True(firstMatch.MatchAwayTeam.Team.TeamId == 1051);
            Assert.True(firstMatch.MatchEvents.Count == 3);
            Assert.True(firstMatch.MatchScorers.Count == 3);
            Assert.True(firstMatch.MatchBookings.Count == 3);
        }

        [Fact]
        public void MatchUpdateTest()
        {
            const int countryId = 66;
            const int seriesId = 1000;
            var country = TestEntities.CreateCountry(countryId, "EnglishName", "CountryName");

            List<Series> series = TestEntities.CreateSeries(seriesId, country);
            var sessionFactory = SessionFactory.CreateSessionFactory(true);
            using (var session = sessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    session.SaveOrUpdate(country);
                    foreach (var league in series)
                    {
                        session.SaveOrUpdate(league);
                    }
                    session.Transaction.Commit();
                }
            }
            var match1 = TestEntities.CreateMatchFullData(1000, country, 1050, 1051,
                                                          new List<int> {10000, 10001, 10002}, 3000, 1);

            var match2 = TestEntities.CreateMatchFullData(1001, country, 1052, 1053,
                                                          new List<int> {10003, 10004, 10005}, 3001, 1);

            series.First(s => s.HtSeriesId == seriesId).AddMatch(match1);
            series.First(s => s.HtSeriesId == seriesId).AddMatch(match2);

            var matchList = new List<Match> {match1, match2};

            using (var session = sessionFactory.OpenSession())
            {
                foreach (var match in matchList)
                {
                    using (session.BeginTransaction())
                    {
                        session.SaveOrUpdate(match);
                        session.Transaction.Commit();
                        session.Clear();
                    }
                }
            }

            var prodSession = SessionManager.CurrentSession;
            var matchRepository = new MatchRepository(prodSession);
            var matches = matchRepository.GetAllMatchesForSeries(seriesId);

            var firstMatch = matches.First(m => m.HtMatchId == 1000);
            var originalRound = firstMatch.MatchRound;

            firstMatch.MatchRound += 10;

            using (var transaction = prodSession.BeginTransaction())
            {
                matchRepository.SaveUpdate(firstMatch);
                transaction.Commit();
            }
            
            var updatedMatch = matchRepository.GetAllMatchesForSeries(seriesId).First(m => m.HtMatchId == 1000);
            Assert.True(updatedMatch.MatchRound != originalRound);

        }    
    }
}
