using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhoScored.Migration.HattrickFileAccessors;

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

            var request = new WhoScoredRequest();
            string response = request.MakeRequest(leagueFixtures.GetHattrickFileAccessorAbsoluteUri());

            using (XmlReader xmlReader = XmlReader.Create(new StringReader(response)))
            {
                xmlReader.ReadToDescendant("League");

                xmlReader.ReadContentAs(typeof(string[]), null);
            }

        }


    }
}
