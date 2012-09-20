using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhoScored.Db.Postgres;
using WhoScored.Model;
using NHibernate.Criterion;

namespace WhoScored.IntegrationTest
{
    using WhoScored.Migration;

    using Xunit;

    public class NHibernateMigrationTest
    {
        [Fact]
        public void MigrateWorldDetailsTestFullMigration()
        {
            var sessionFactory = SessionFactory.CreateSessionFactory(true);
            var target = new MigrateToNhibernateDomainService(SessionManager.CurrentSession);
            target.MigrateWorldDetails();

            using (var session = sessionFactory.OpenSession())
            {
                var countries = session.QueryOver<Country>()
                    .List<Country>();

                Assert.True(countries.Count > 0);
            }
        }

        [Fact]
        public void MigrateSeriesDetailsTest()
        {
            const int htCountryId = 66;
            var country = TestEntities.CreateCountry(htCountryId, "EnglishName", "countryName");
            var supportedIds = TestEntities.CreateSupportedIdList();

            for (int i = 0; i < 3; i++)
            {
                country.AddSeriesId(supportedIds[i]);                
            }
            

            var sessionFactory = SessionFactory.CreateSessionFactory(true);
            using (var session = sessionFactory.OpenSession())
            {
                session.SaveOrUpdate(country);
                session.Flush();
            }
            
            var service = new MigrateToNhibernateDomainService(SessionManager.CurrentSession);
            service.MigrateLeagueDetails(htCountryId);

            using (var session = sessionFactory.OpenSession())
            {
                var leagues = session.CreateCriteria<Series>()
                    .Add(Restrictions.Eq("Country.CountryId", country.CountryId))
                    .List<Series>();

                Assert.True(leagues.Count > 0);
            }
        }

        [Fact]
        public void MigrateFixturesTest()
        {
            const int seriesId = 29747;
            const int htCountryId = 66;
            var country = TestEntities.CreateCountry(htCountryId, "EnglishName", "countryName");
            var series = TestEntities.CreateSeries(seriesId, country, "A Lyga");
            var suportedSeriesIds = TestEntities.CreateSupportedIdList();
            country.AddSeriesIdRange(suportedSeriesIds);
            var sessionFactory = SessionFactory.CreateSessionFactory(true);
            using (var session = sessionFactory.OpenSession())
            {
                session.SaveOrUpdate(country);
                session.SaveOrUpdate(series);
                session.Flush();
            }

            var service = new MigrateToNhibernateDomainService(SessionManager.CurrentSession);
            service.MigrateFixtures(seriesId, 30);
            service.MigrateFixtures(seriesId, 29);

            using (var session = sessionFactory.OpenSession())
            {
                var fixtures = session.CreateCriteria<SeriesFixture>()
                    .Add(Restrictions.Eq("Series.Id", series.Id))
                    .Add(Restrictions.Eq("Season", (Int16)30))
                    .List<SeriesFixture>();
                Assert.True(fixtures.Count == 56);
            }
        }

        [Fact]
        public void MigrateMatchDetailsTest()
        {
            const int seriesId = 29750;
            const int htCountryId = 66;
            const int htFirstMatchId = 383708238;
            const int htSecondMatchId = 383708239;
            const short season = 31;
            const short matchRound = 12;
            var country = TestEntities.CreateCountry(htCountryId, "EnglishName", "countryName");
            var series = TestEntities.CreateSeries(seriesId, country, "A Lyga");
            var supportedIds = TestEntities.CreateSupportedIdList();
            country.AddSeriesIdRange(supportedIds);
            var sessionFactory = SessionFactory.CreateSessionFactory(true);
            using (var session = sessionFactory.OpenSession())
            {
                session.SaveOrUpdate(country);
                session.SaveOrUpdate(series);
                session.Flush();
            }
            var service = new MigrateToNhibernateDomainService(SessionManager.CurrentSession);
            service.MigrateMatchDetails(htFirstMatchId, matchRound, season, seriesId);
            service.MigrateMatchDetails(htSecondMatchId, matchRound, season, seriesId);

        }
    }
}
