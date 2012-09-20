using System.Collections.Generic;

namespace WhoScored.Model.Repositories
{
    public interface ICountryRepository : IRepository<Country>
    {
        Country GetCountryByHtId(int htCountryId);
        void SaveUpdateCountries(IList<Country> contries);
    }
}
