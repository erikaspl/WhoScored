using System.Collections.Generic;
using System.Linq;

using WhoScored.Db.Postgres;
using WhoScored.Model;

namespace WhoScored.IntegrationTest
{
    using Db.Postgres.Repositories;

    using Xunit;

    public class SeriesRepositoryTest
    {
        [Fact]
        public void SeriesLoadTest()
        {
            var country = TestEntities.CreateCountry(66, "EnglishName", "CountryName");

            List<Series> series = TestEntities.CreateSeries(1000, country);
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

            var repository = new SeriesRepository(SessionManager.CurrentSession);
            var countrySeries = repository.GetAllSeriesForCountry(66);

            Assert.True(countrySeries.Count() == 2);
            Assert.True(countrySeries.First().SeriesFixtures.Count == 1);
            Assert.True(countrySeries.First().SeriesFixtures.First().HomeTeam.TeamName == "TeamName1");
            Assert.True(countrySeries.First().SeriesFixtures.First().AwayTeam.TeamName == "TeamName2");
        }

        [Fact]
        public void SeriesSaveTest()
        {
            var country = TestEntities.CreateCountry(66, "EnglishName", "CountryName");
            var sessionFactory = SessionFactory.CreateSessionFactory(true);
            using (var session = sessionFactory.OpenSession())
            {
                session.SaveOrUpdate(country);
                session.Flush();
            }

            List<Series> series = TestEntities.CreateSeries(1000, country);
            var prodSession = SessionManager.CurrentSession;
            var repository = new SeriesRepository(prodSession);
            repository.SaveUpdateSeries(series);

            var countrySeries = repository.GetAllSeriesForCountry(66);
            Assert.True(countrySeries.Count() == 2);
            Assert.True(countrySeries.First().SeriesFixtures.Count == 1);
            Assert.True(countrySeries.First().SeriesFixtures.First().HomeTeam.TeamName == "TeamName1");
            Assert.True(countrySeries.First().SeriesFixtures.First().AwayTeam.TeamName == "TeamName2");
        }

        [Fact]
        public void SeriesUpdateTest()
        {
            var country = TestEntities.CreateCountry(66, "EnglishName", "CountryName");
            List<Series> series = TestEntities.CreateSeries(1000, country);
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

            var repository = new SeriesRepository(SessionManager.CurrentSession);
            var countrySeries = repository.GetAllSeriesForCountry(66).ToList();

            const string newValue = "newName";
            countrySeries.First(s => s.HtSeriesId == 1000).LeagueLevelUnitName = newValue;
            repository.SaveUpdateSeries(countrySeries);

            var newSeries = repository.GetAllSeriesForCountry(66);
            Assert.True(newSeries.First(s => s.HtSeriesId == 1000).LeagueLevelUnitName == newValue);

        }

        [Fact]
        public void GetSeriesFixtureForSeasonTest()
        {
            const int season = 30;
            const int htSeriesId = 1000;
            var country = TestEntities.CreateCountry(66, "EnglishName", "CountryName");
            List<Series> series = TestEntities.CreateSeries(htSeriesId, country);
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


            var repository = new SeriesRepository(SessionManager.CurrentSession);
            var fixtures = repository.GetSeriesFixtureForSeason(htSeriesId, season);

            Assert.True(fixtures.Count == 1);
        }

        [Fact]
        public void GetSeriesResultsTest()
        {
            const int season = 30;
            const int htSeriesId = 1000;
            var country = TestEntities.CreateCountry(66, "EnglishName", "CountryName");
            var series = TestEntities.CreateSeries(htSeriesId, country, "SeriesName");
            var matches = new List<Match>
                              {
                                  TestEntities.CreateMatchFullData(1000, country, 1050, 1051, new List<int>(), 1500, 1 ),
                                  TestEntities.CreateMatchFullData(1001, country, 1052, 1053, new List<int>(), 1500, 1 ),
                                  TestEntities.CreateMatchFullData(1002, country, 1054, 1055, new List<int>(), 1500, 2 )
                              };
            matches.ForEach(series.AddMatch);

            var sessionFactory = SessionFactory.CreateSessionFactory(true);
            using (var session = sessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    session.SaveOrUpdate(country);
                    session.SaveOrUpdate(series);
                    session.Transaction.Commit();
                }
            }

            var seriesRepository = new SeriesRepository(SessionManager.CurrentSession);

            var matchesFromDb = seriesRepository.GetSeriesResults(htSeriesId, season);

            Assert.True(matchesFromDb.Count == 3);
        }

        [Fact]
        public void GetSeriesResultsByMatchRoundTest()
        {
            const int season = 30;
            const int htSeriesId = 1000;
            var country = TestEntities.CreateCountry(66, "EnglishName", "CountryName");
            var series = TestEntities.CreateSeries(htSeriesId, country, "SeriesName");
            var matches = new List<Match>
                              {
                                  TestEntities.CreateMatchFullData(1000, country, 1050, 1051, new List<int>(), 1500, 1 ),
                                  TestEntities.CreateMatchFullData(1001, country, 1052, 1053, new List<int>(), 1500, 1 ),
                                  TestEntities.CreateMatchFullData(1002, country, 1054, 1055, new List<int>(), 1500, 2 )
                              };
            matches.ForEach(series.AddMatch);

            var sessionFactory = SessionFactory.CreateSessionFactory(true);
            using (var session = sessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    session.SaveOrUpdate(country);
                    session.SaveOrUpdate(series);
                    session.Transaction.Commit();
                }
            }

            var seriesRepository = new SeriesRepository(SessionManager.CurrentSession);

            var matchesFromDb = seriesRepository.GetSeriesResults(htSeriesId, season, 1);

            Assert.True(matchesFromDb.Count == 2);
        }

        [Fact]
        public void HasFixturesTestDetectFixture()
        {
            const int season = 30;
            const int htSeriesId = 1000;
            var country = TestEntities.CreateCountry(66, "EnglishName", "CountryName");
            List<Series> series = TestEntities.CreateSeries(htSeriesId, country);
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


            var repository = new SeriesRepository(SessionManager.CurrentSession);
            var hasFixtures = repository.HasFixtures(htSeriesId, season);
            Assert.True(hasFixtures);
        }

        [Fact]
        public void HasFixturesTestFixturesNotFound()
        {
            const int season = 30;
            const int htSeriesId = 1000;
            var country = TestEntities.CreateCountry(66, "EnglishName", "CountryName");
            List<Series> series = TestEntities.CreateSeries(htSeriesId, country);
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


            var repository = new SeriesRepository(SessionManager.CurrentSession);
            var hasFixtures = repository.HasFixtures(1002, season);
            Assert.False(hasFixtures);
        }
    }
}
