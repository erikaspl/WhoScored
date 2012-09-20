using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;
using WhoScored.Model;
using WhoScored.Models;

namespace WhoScored.Controllers
{
    public abstract class WhoScoredControllerBase : Controller
    {
        protected IList<ICountryDetails> GetContryDetailsModel(IList<Country> countries)
        {
            var contryDetails = new List<ICountryDetails>();
            countries.ForEach(c => contryDetails.Add(GetContryDetailsModel(c)));

            return contryDetails;
        }

        protected ICountryDetails GetContryDetailsModel(Country country)
        {
            return new CountryDetails
                                    {
                                        EnglishName = country.EnglishName,
                                        LeagueID = country.HtCountryId,
                                        LeagueInWhoScored = country.CountryInWhoScored,
                                        LeagueName = country.CountryName,
                                        NumberOfLevels = country.NumberOfLevels,
                                        SeasonOffset = country.SeasonOffset,
                                    };
        }
    }
}