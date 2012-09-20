using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WhoScored.Db.Postgres.Repositories;
using WhoScored.Model;
using WhoScored.Models;


namespace WhoScored.Controllers
{
    using NHibernate;

    using Db.Postgres;

    public class SeriesController : WhoScoredControllerBase
    {
        private readonly CountryRepository _countryRepository;
        private readonly SettingsRepository _settingsRepository;
        private readonly SeriesRepository _seriesRepository;

        private const int DEFAULT_MATCH_ROUND = 14;

        public ActionResult Index()
        {
            return View();
        }

        public SeriesController(ISession session)
        {
            _countryRepository = new CountryRepository(session);
            _settingsRepository = new SettingsRepository(session);
            _seriesRepository = new SeriesRepository(session);
        }

        private int GetCurrentSeason(int globalSeason, int contrySeasonOffset)
        {
            return globalSeason + contrySeasonOffset;
        }

        public ActionResult WorldDetails()
        {
            const string selectedCountry = "Lithuania";
            var countries = _countryRepository.GetAll().ToList();

            var settings = _settingsRepository.GetAll().First();
            var currentSeason = GetCurrentSeason(
                settings.GlobalSeason, countries.First(c => c.EnglishName == selectedCountry).SeasonOffset);
            var worldDetailsViewData = new WorldDetailsModel
            {
                WorldDetails = GetContryDetailsModel(countries.OrderBy(w => w.EnglishName).ToList()),
                Settings = settings,
                SelectedCountry = selectedCountry,
                CurrentSeason = currentSeason
            };

            return Json(worldDetailsViewData);
        }

        public ActionResult SeasonAndSeriesListForCountry(int countryId)
        {
            var seriesFullDetails = _seriesRepository.GetAllSeriesForCountry(countryId).ToList();
            var worldDetails = _countryRepository.GetCountryByHtId(countryId);
            var settings = _settingsRepository.GetAll().First();

            var series = GetSeriesForCountry(worldDetails, seriesFullDetails);
            var seasons = GetSeasonsForCountry(worldDetails, settings);

            return Json(new { Series = series, Seasons = seasons });
        }

        private List<SelectListItem> GetSeriesForCountry(Country country, IList<Series> seriesFullDetails)
        {
            var leagues = new List<SelectListItem>();
            foreach (var series in country.SupportedSeriesId)
            {
                if (seriesFullDetails.Select(s => s.HtSeriesId).Contains(series.HtSeriesId))
                {
                    var item = seriesFullDetails.First(s => s.HtSeriesId == series.HtSeriesId);
                    leagues.Add(new SelectListItem
                                    {
                                        Text = item.LeagueLevelUnitName,
                                        Value = item.HtSeriesId.ToString()
                                    });
                }
                else
                {
                    leagues.Add(new SelectListItem
                                    {
                                        Text = series.HtSeriesId.ToString(),
                                        Value = series.HtSeriesId.ToString()
                                    });
                }
            }
            return leagues;
        }

        private List<SelectListItem> GetSeasonsForCountry(Country country, Model.Settings settings)
        {
            var seasons = new List<SelectListItem>();           
            int numberOfSeasons = settings.GlobalSeason + country.SeasonOffset;

            for (int i = numberOfSeasons; i >= 1; i--)
            {
                seasons.Add(new SelectListItem
                                {Text = i.ToString(), Value = i.ToString(), Selected = i == numberOfSeasons});
            }
            return seasons;
        }

        //public ActionResult TeamStandings(int? seriesId, int season, int? matchRound)
        //{
        //    if (!seriesId.HasValue)
        //    {
        //        return Json(new List<int>());
        //    }

        //    if (matchRound.GetValueOrDefault(0) <= 0)
        //    {
        //        matchRound = DEFAULT_MATCH_ROUND;
        //    }

        //    return Json(_repository.GetSeriesStandings(seriesId.Value, season, matchRound.Value));
        //}

        public ActionResult SeriesResults(int seriesId, short season, short matchRound)
        {
            var seriesResults = _seriesRepository.GetSeriesResults(seriesId, season, matchRound);
            return Json(seriesResults);
        }

        public ActionResult AllSeriesResults(int seriesId, short season)
        {
            var seriesResults = _seriesRepository.GetSeriesResults(seriesId, season);
            return Json(seriesResults);
        }
    }
}
