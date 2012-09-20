using System.Collections.Generic;
using System.Linq;
using WhoScored.Db.Postgres;
using WhoScored.Db.Postgres.Repositories;
using WhoScored.Model;

namespace WhoScored.IntegrationTest
{
    using Xunit;

    public class CountryRepositoryTest
    {
        [Fact]
        public void CountriesCRUDTest()
        {
            var sessionFactory = SessionFactory.CreateSessionFactory(true);
            
            const string country1 = "CountryName1";
            const string country2 = "CountryName2";

            var countries = new List<Country>
                                          { TestEntities.CreateCountry(10000, "EnglishName1", country1),
                                            TestEntities.CreateCountry(10001, "EnglishName2", country2)
                                          };
            var session = SessionManager.CurrentSession;
            var repository = new CountryRepository(session);
            repository.SaveUpdateCountries(countries);

            var fromDb = repository.GetAll();
            Assert.True(fromDb.ToList().Count(c => c.CountryName == country1 || c.CountryName == country2) == 2 );

            var updateCountry = countries.First();

            const string country3 = "CountryName3";
            const string englishName3 = "EnglishName3";
            updateCountry.EnglishName = englishName3;
            updateCountry.CountryName = country3;

            repository.SaveUpdate(updateCountry);

            var updatedCountry = repository.GetById(updateCountry.CountryId);
            Assert.True(updatedCountry.CountryName == updateCountry.CountryName);
            Assert.True(updatedCountry.EnglishName == updateCountry.EnglishName);

            foreach (var country in countries)
            {
                repository.Delete(country);
            }
            session.Flush();

            fromDb = repository.GetAll();
            Assert.True(fromDb.ToList().Count(c => c.CountryName == country1 || c.CountryName == country2) == 0);
        }

        [Fact]
        public void CountryArrayTest()
        {
            const int contryId = 10000;
            var country = TestEntities.CreateCountry(contryId, "EnglishName1", "CountryName1");
            var seriesIdList = new List<int> {1, 2, 3, 4, 5, 6};

            country.AddSeriesIdRange(seriesIdList);
            int listCount = seriesIdList.Count;

            var sessionFactory = SessionFactory.CreateSessionFactory(true);
            using (var session = sessionFactory.OpenSession())
            {
                session.SaveOrUpdate(country);
                session.Flush();
            }

            var repository = new CountryRepository(SessionManager.CurrentSession);
            var fromDb = repository.GetCountryByHtId(contryId);
            Assert.True(fromDb.SupportedSeriesId.Count == listCount);
        }
    }
}
