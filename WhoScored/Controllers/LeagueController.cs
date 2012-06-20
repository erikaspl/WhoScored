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
            var worldDetailsViewData = new WorldDetailsModel { WorldDetails = worldDetails, Settings = settings, SelectedCountry = selectedCountry };

            return View(worldDetailsViewData);
        }

    }
}
