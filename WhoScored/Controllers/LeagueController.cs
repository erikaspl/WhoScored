using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhoScored.Db;
using WhoScored.Db.Mongo;
using WhoScored.Models;

namespace WhoScored.Controllers
{
    public class LeagueController : Controller
    {
        readonly IWhoScoredRepository _repository = new WhoScoredRepository();

        public ActionResult Index()
        {
            const string selectedCountry = "Lithuania";
            var worldDetails = _repository.GetWorldDetails<WorldDetails>();
            var settings = _repository.GetSettings<Settings>();
            var currentSeason = GetCurrentSeason(
                settings.GlobalSeason, worldDetails.First(c => c.EnglishName == selectedCountry).SeasonOffset);
            var worldDetailsViewData = new WorldDetailsModel { WorldDetails = worldDetails, Settings = settings, 
                SelectedCountry = selectedCountry, CurrentSeason = currentSeason};

            return View(worldDetailsViewData);
        }

        private int GetCurrentSeason(int globalSeason, int contrySeasonOffset)
        {
            return globalSeason + contrySeasonOffset;
        }

        public ActionResult TeamStandings(jQueryDataTableParamModel param)
        {
            var seriesStandings = _repository.GetSeriesStandings(param.SeriesId, param.Season);

            var result = from c in seriesStandings
                         select new[]
                                 {
                                     Convert.ToString(c.TeamId), c.TeamName,
                                     c.Played.ToString(), c.Won.ToString(), c.Drawn.ToString(), c.Lost.ToString(),
                                     c.GoalsScored.ToString(), c.GoalsConceded.ToString(), c.GoalDifference.ToString(),
                                     c.TotalPoints.ToString()
                                 };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = seriesStandings.Count,
                iTotalDisplayRecords = seriesStandings.Count, 
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }

    }
}
