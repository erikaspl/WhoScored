using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhoScored.Db;
using WhoScored.Db.Mongo;
using WhoScored.Model;
using WhoScored.Models;

namespace WhoScored.Controllers
{
    public class SeriesController : Controller
    {
        readonly IWhoScoredRepository _repository = new WhoScoredRepository();

        private const int DEFAULT_MATCH_ROUND = 14;

        public ActionResult Index()
        {
            return View();
        }

        private int GetCurrentSeason(int globalSeason, int contrySeasonOffset)
        {
            return globalSeason + contrySeasonOffset;
        }

        public ActionResult WorldDetails()
        {
            const string selectedCountry = "Lithuania";
            var worldDetails = _repository.GetWorldDetails<WorldDetails>();
            
            var settings = _repository.GetSettings<Settings>();
            var currentSeason = GetCurrentSeason(
                settings.GlobalSeason, worldDetails.First(c => c.EnglishName == selectedCountry).SeasonOffset);
            var worldDetailsViewData = new WorldDetailsModel
            {
                WorldDetails = worldDetails.OrderBy(w => w.EnglishName).ToList(),
                Settings = settings,
                SelectedCountry = selectedCountry,
                CurrentSeason = currentSeason
            };

            return Json(worldDetailsViewData);
        }

        public ActionResult SeasonAndSeriesListForCountry(string countryId)
        {
            var seriesFullDetails = _repository.GetSeriesDetails<SeriesDetails>(countryId);
            var worldDetails = _repository.GetWorldDetails<WorldDetails>(int.Parse(countryId));
            var settings = _repository.GetSettings<Settings>();

            var series = GetSeriesForCountry(worldDetails, seriesFullDetails);
            var seasons = GetSeasonsForCountry(worldDetails, settings);

            return Json(new { Series = series, Seasons = seasons });
        }

        private List<SelectListItem> GetSeriesForCountry(WorldDetails worldDetails, List<SeriesDetails> seriesFullDetails)
        {
            var leagues = new List<SelectListItem>();
            foreach (int seriesId in worldDetails.SeriesIdList)
            {
                if (seriesFullDetails.Select(s => s.LeagueLevelUnitID).Contains(seriesId))
                {
                    var item = seriesFullDetails.First(s => s.LeagueLevelUnitID == seriesId);
                    leagues.Add(new SelectListItem
                                    {
                                        Text = item.LeagueLevelUnitName,
                                        Value = item.LeagueLevelUnitID.ToString()
                                    });
                }
                else
                {
                    leagues.Add(new SelectListItem
                                    {
                                        Text = seriesId.ToString(),
                                        Value = seriesId.ToString()
                                    });
                }
            }
            return leagues;
        }

        private List<SelectListItem> GetSeasonsForCountry(WorldDetails worldDetails, Settings settings)
        {
            var seasons = new List<SelectListItem>();           
            int numberOfSeasons = settings.GlobalSeason + worldDetails.SeasonOffset;

            for (int i = numberOfSeasons; i >= 1; i--)
            {
                seasons.Add(new SelectListItem
                                {Text = i.ToString(), Value = i.ToString(), Selected = i == numberOfSeasons});
            }
            return seasons;
        }

        public ActionResult TeamStandings(int? seriesId, int season, int? matchRound)
        {
            if (!seriesId.HasValue)
            {
                return Json(new List<int>());
            }

            if (matchRound.GetValueOrDefault(0) <= 0)
            {
                matchRound = DEFAULT_MATCH_ROUND;
            }

            return Json(_repository.GetSeriesStandingsWithResults(seriesId.Value, season, matchRound.Value));
        }

        public ActionResult SeriesResults(int seriesId, int season, int matchRound)
        {
            var seriesResults = _repository.GetSeriesResults(seriesId, season, matchRound);
            return Json(seriesResults);
        }

        public ActionResult SeriesResults(int seriesId, int season)
        {
            var seriesResults = _repository.GetSeriesResults(seriesId, season);
            return Json(seriesResults.GroupBy(r => r.MatchRound).ToDictionary(r => r.Key.ToString(), v => v.ToList()));
        }
    }
}
