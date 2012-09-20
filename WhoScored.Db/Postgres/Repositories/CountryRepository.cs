using System.Collections.Generic;
using NHibernate;
using WhoScored.Model;
using WhoScored.Model.Repositories;

namespace WhoScored.Db.Postgres.Repositories
{
    public class CountryRepository : NHRepository<Country>, ICountryRepository
    {
        public CountryRepository(ISession session) : base(session)
        {
        }

        public Country GetCountryByHtId(int htCountryId)
        {
            var countryQuery = (from countries in Session.QueryOver<Country>()
                                where countries.HtCountryId == htCountryId
                                select countries).SingleOrDefault<Country>();

            return countryQuery;
        }

        public void SaveUpdateCountries(IList<Country> contries)
        {
            using (Session.BeginTransaction())
            {
                foreach (var country in contries)
                {
                    if (country.IsNewValue)
                    {
                        Session.Save(country);
                    }
                    else
                    {
                        Session.SaveOrUpdate(country);
                    }
                }
                Session.Transaction.Commit();
            }
        }
    }
}
