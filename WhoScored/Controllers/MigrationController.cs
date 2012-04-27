using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhoScored.Migration.HattrickFileAccessors;

namespace WhoScored.Controllers
{
    public class MigrationController : Controller
    {
        //
        // GET: /Migration/

        public ActionResult Index()
        {
            return View();
        }

        public void Migrate()
        {
            var leagueFixtures = new LeagueFixtures(ConfigurationManager.AppSettings["protectedResourceUrl"]);

            leagueFixtures.Season = 30;

            var request = new WhoScoredRequest();
            string response = request.MakeRequest(leagueFixtures.GetHattrickFileAccessorAbsoluteUri());
        }


    }
}
