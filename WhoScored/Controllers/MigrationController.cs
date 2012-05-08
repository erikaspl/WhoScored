using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhoScored.CHPP.Files.HattrickFileAccessors;

namespace WhoScored.Controllers
{
    using System.IO;
    using System.Xml;

    using WhoScored.Db;
    using WhoScored.Db.Mongo;
    using WhoScored.Migration;
    using WhoScored.Models;

    public class MigrationController : Controller
    {
        IWhoScoredDbService dbService = new MongoService();
        //
        // GET: /Migration/

        public ActionResult Index()
        {
            var worldDetails = dbService.GetWorldDetails<LeagueDetails>();
            return View(worldDetails);
        }

        public void MigrateWorldDetails()
        {
            //var leagueFixtures = new WorldDetails(ConfigurationManager.AppSettings["protectedResourceUrl"]);

            var target = new MigrationDomainService();
            target.MigrateWorldDetails();

        }
    }
}
