using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WhoScored.Db;
using WhoScored.Db.Mongo;
using WhoScored.Migration;
using WhoScored.Model;
using WhoScored.Models;


namespace WhoScored.Controllers
{
    public class MigrationController : Controller
    {
        readonly IWhoScoredRepository _repository = new WhoScoredRepository();

        public ActionResult Index()
        {
            var worldDetails = _repository.GetWorldDetails<WorldDetails>();
            var settings = _repository.GetSettings<Settings>();
            var migrationViewData = new WorldDetailsModel { WorldDetails = worldDetails, Settings = settings };

            return View(migrationViewData);
        }

        public ActionResult AsyncSeriesSelect(string countryId)
        {
            var leagues = new List<SelectListItem>();
            var seriesFullDetails = _repository.GetSeriesDetails<SeriesDetails>(countryId);
            var worldDetails = _repository.GetWorldDetails<WorldDetails>(int.Parse(countryId));

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

            var worldDetails = _repository.GetWorldDetails<WorldDetails>(int.Parse(countryId));
            var settings = _repository.GetSettings<Settings>();

            int numberOfSeasons = settings.GlobalSeason + worldDetails.SeasonOffset;

            for (int i = numberOfSeasons; i >= 1; i--)
            {
                seasons.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString(), Selected = i == numberOfSeasons});
            }

            return Json(seasons);
        }

        public ActionResult ShowFixtures(List<int> seriesId, int season)
        {
            var seasonSummary = _repository.GetSeriesFixturesSummary<SeriesFixturesSummaryEntity, MatchDetails>(seriesId.First(), season);

            if (seasonSummary != null)
            {
                return Json(true);
            }

            return Json(false);
        }

        public void MigrateWorldDetails()
        {
            var migrationService = new MigrationDomainService();
            migrationService.MigrateWorldDetails();
        }

        public void MigrateSeriesDetails(List<int> seriesId)
        {
            var migrationService = new MigrationDomainService();
            migrationService.MigrateLeagueDetails(seriesId);
        }

        public ActionResult MigrateSeriesFixtures(int seriesId, int season)
        {
            var migrationService = new MigrationDomainService();
            migrationService.MigrateFixtures(new List<int>{seriesId}, season);

            return Json(true);
        }

        public ActionResult MigrateMatchDetails(int matchId)
        {
            var migrationService = new MigrationDomainService();
            migrationService.MigrateMatchDetails(matchId);

            return Json(true);
        }

        public ActionResult AjaxHandler(jQueryDataTableParamModel param)
        {
            var seasonSummary = _repository.GetSeriesFixturesSummary<SeriesFixturesSummaryEntity, MatchDetails>(param.SeriesId, param.Season);

            IEnumerable<IMatchSummary> filteredMatches;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMatches = seasonSummary.Matches.Where(m => m.HomeTeamName.ToLower().Contains(param.sSearch.ToLower())
                                                                   || m.AwayTeamName.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredMatches = seasonSummary.Matches;
            }

            var displayedMatches = filteredMatches.OrderByDescending(m => m.MatchRound);

            var result = from c in displayedMatches
                         select new[]
                                 {
                                     Convert.ToString(c.MatchID), c.MatchDate.ToString("dd/MM/yyyy"),
                                     c.MatchRound.ToString(), c.HomeTeamName, c.AwayTeamName, c.IsMatchMigrated.ToString()
                                 };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = seasonSummary.Matches.Count,
                iTotalDisplayRecords = seasonSummary.Matches.Count,                
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> StartMigrateMatchDetails(int seriesId, int season, string operationId)
        {
            if (!string.IsNullOrEmpty(operationId))
            {
                var seasonSummary =
                    _repository.GetSeriesFixturesSummary<SeriesFixturesSummaryEntity, MatchDetails>(seriesId, season);

                if (!_migrationStatus.ContainsKey(operationId))
                {
                    _migrationStatus.Add(operationId, 0);
                }

                await
                    MigrateMatches(
                        seasonSummary.Matches.Where(m => m.IsMatchMigrated == false).Select(m => m.MatchID).ToList(),
                        operationId);

                return Json(operationId);
            }
            else
            {
                return Json(false);
            }

        }

        private static readonly Dictionary<string, int> _migrationStatus = new Dictionary<string, int>();
        public async Task MigrateMatches(List<int> matches, string operationId)
        {
            int matchesLeft = matches.Count;
            int totalMatches = matches.Count;

            var migrationService = new MigrationDomainService();
            foreach (var matchId in matches)
            {
                migrationService.MigrateMatchDetails(matchId);

                matchesLeft--;
                _migrationStatus[operationId] = 100 - Convert.ToInt32(Math.Round(matchesLeft / (decimal)totalMatches * 100, 0));
            }            
        }

        public ActionResult GetMigrationStatus(string operationId)
        {
            int status = 0; //initial status complete
            if (!string.IsNullOrEmpty(operationId) && _migrationStatus.ContainsKey(operationId))
            {
                status = _migrationStatus[operationId];
            }
            return Json(status);
        }

        public void CompleteMigrateMatchDetails(string operationId)
        {
            if (!string.IsNullOrEmpty(operationId) && _migrationStatus.ContainsKey(operationId))
            {
                _migrationStatus.Remove(operationId);
            }
        }

    }
}
