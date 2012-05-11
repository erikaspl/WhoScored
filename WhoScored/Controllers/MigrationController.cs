using System.Collections.Generic;
using System.Web.Mvc;
using WhoScored.Db;
using WhoScored.Db.Mongo;
using WhoScored.Migration;
using WhoScored.Models;


namespace WhoScored.Controllers
{
    public class MigrationController : Controller
    {
        IWhoScoredDbService dbService = new MongoService();
        //
        // GET: /Migration/

        public ActionResult Index()
        {
            var worldDetails = dbService.GetWorldDetails<WorldDetails>();
            var settings = dbService.GetSettings<Settings>();
            var migrationViewData = new MigrationModel { WorldDetails = worldDetails, Settings = settings };

            return View(migrationViewData);
        }

        public void MigrateWorldDetails()
        {
            //var leagueFixtures = new WorldDetails(ConfigurationManager.AppSettings["protectedResourceUrl"]);

            var target = new MigrationDomainService();
            target.MigrateWorldDetails();
        }

        public void MigrateSeriesDetails(int countryId)
        {
            
        }
    }
}
