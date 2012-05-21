using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WhoScored.Db;
using WhoScored.Db.Mongo;
using WhoScored.Migration;
using WhoScored.Models;


namespace WhoScored.Controllers
{
    public class MigrationController : Controller
    {
        IWhoScoredRepository repository = new WhoScoredRepository();
        //
        // GET: /Migration/

        public ActionResult Index()
        {
            var worldDetails = repository.GetWorldDetails<WorldDetails>();
            var settings = repository.GetSettings<Settings>();
            var migrationViewData = new MigrationModel { WorldDetails = worldDetails, Settings = settings };

            return View(migrationViewData);
        }

        public ActionResult AsyncSeriesSelect(string countryId)
        {
            var leagues = new List<SelectListItem>();
            var seriesFullDetails = repository.GetSeriesDetails<SeriesDetails>(countryId);
            var worldDetails = repository.GetWorldDetails<WorldDetails>(int.Parse(countryId));

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

            return Json(leagues);
        }


        public ActionResult AsyncSeasonSelect(string countryId)
        {
            var seasons = new List<SelectListItem>();

            var worldDetails = repository.GetWorldDetails<WorldDetails>(int.Parse(countryId));
            var settings = repository.GetSettings<Settings>();

            int numberOfSeasons = settings.GlobalSeason + worldDetails.SeasonOffset;

            for (int i = numberOfSeasons; i >= 1; i--)
            {
                seasons.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString(), Selected = i == numberOfSeasons});
            }

            return Json(seasons);
        }

        public void MigrateWorldDetails()
        {
            var target = new MigrationDomainService();
            target.MigrateWorldDetails();
        }

        public void MigrateSeriesDetails(List<int> seriesId)
        {
            var target = new MigrationDomainService();
            target.MigrateLeagueDetails(seriesId);
        }
    }
}
