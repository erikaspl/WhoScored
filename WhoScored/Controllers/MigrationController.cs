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

    public class MigrationController : Controller
    {
        //
        // GET: /Migration/

        public ActionResult Index()
        {
            return View();
        }

        public void MigrateWorldDetails()
        {
            var leagueFixtures = new WorldDetails(ConfigurationManager.AppSettings["protectedResourceUrl"]);

        }
    }
}
