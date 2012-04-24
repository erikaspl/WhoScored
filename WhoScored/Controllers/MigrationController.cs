using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            WhoScoredOAuth oAuth = new WhoScoredOAuth();
            oAuth.WhoScoredConsumer();
        }


    }
}
