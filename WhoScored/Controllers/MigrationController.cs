using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using NHibernate;
using WhoScored.Db.Postgres;
using WhoScored.Db.Postgres.Repositories;
using WhoScored.Migration;
using WhoScored.Model;
using WhoScored.Model.Repositories;
using WhoScored.Models;
using Settings = WhoScored.Model.Settings;


namespace WhoScored.Controllers
{
    public class MigrationController : WhoScoredControllerBase
    {
        private const int CURRENT_SEASON = 49;

        private readonly ISession _session;

        private readonly CountryRepository _countryRepository;
        private readonly SettingsRepository _settingsRepository;
        private readonly SeriesRepository _seriesRepository;

        public MigrationController(ISession session)
        {
            _session = session;
            _countryRepository = new CountryRepository(_session);
            _settingsRepository = new SettingsRepository(_session);
            _seriesRepository = new SeriesRepository(_session);
        }

        public ActionResult Index()
        {
            var countries = _countryRepository.GetAll().ToList();
            var settings = _settingsRepository.GetAll().First();
            var migrationViewData = new WorldDetailsModel { WorldDetails = GetContryDetailsModel(countries), Settings = settings };

            return View(migrationViewData);
        }

        public ActionResult ResetDatabase()
        {
            _settingsRepository.ResetDatabase();

            var settings = new Model.Settings
                               {
                                   GlobalSeason = CURRENT_SEASON
                               };

            _settingsRepository.Save(settings);

            return Json(new WorldDetailsModel { WorldDetails = new List<ICountryDetails>(), Settings = settings });
        }

        public ActionResult AsyncSeriesSelect(int countryId)
        {
            var leagues = new List<SelectListItem>();
            var seriesFullDetails = _seriesRepository.GetAllSeriesForCountry(countryId);
            var country = _countryRepository.GetCountryByHtId(countryId);

            foreach (var series in country.Series)
            {
                if (seriesFullDetails.Select(s => s.HtSeriesId).Contains(series.HtSeriesId))
                {
                    var item = seriesFullDetails.First(s => s.HtSeriesId == series.HtSeriesId);
                    leagues.Add(new SelectListItem
                                    {
                                        Text = item.LeagueLevelUnitName,
                                        Value = item.HtSeriesId.ToString()
                                    });
                }
                else
                {
                    leagues.Add(new SelectListItem
                                    {
                                        Text = series.HtSeriesId.ToString(),
                                        Value = series.HtSeriesId.ToString()
                                    });
                }
            }

            return Json(leagues.OrderBy(l => l.Value));
        }

        public ActionResult AsyncSeasonSelect(int countryId)
        {
            var seasons = new List<SelectListItem>();

            var worldDetails = _countryRepository.GetCountryByHtId(countryId);
            var settings = _settingsRepository.GetAll().First();

            int numberOfSeasons = settings.GlobalSeason + worldDetails.SeasonOffset;

            for (int i = numberOfSeasons; i >= 1; i--)
            {
                seasons.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString(), Selected = i == numberOfSeasons});
            }

            return Json(seasons);
        }

        public ActionResult ShowFixtures(List<int> seriesId, short season)
        {
            return Json(_seriesRepository.HasFixtures(seriesId.First(), season));
        }

        public void MigrateWorldDetails()
        {
            var migrationService = new MigrateToNhibernateDomainService(_session);
            migrationService.MigrateWorldDetails();
        }

        public void MigrateSeriesDetails(int countryId)
        {
            var migrationService = new MigrateToNhibernateDomainService(_session);
            migrationService.MigrateLeagueDetails(countryId);
        }

        public ActionResult MigrateSeriesFixtures(int seriesId, int season)
        {
            var migrationService = new MigrateToNhibernateDomainService(_session);
            migrationService.MigrateFixtures(seriesId, season);

            return Json(true);
        }

        public ActionResult MigrateMatchDetails(int matchId, int matchRound, int season, int leagueId)
        {
            var migrationService = new MigrateToNhibernateDomainService(_session);
            migrationService.MigrateMatchDetails(matchId, matchRound, season, leagueId);

            return Json(true);
        }

        public ActionResult AjaxHandler(jQueryDataTableParamModel param)
        {
            var seasonSummary = _seriesRepository.GetSeriesFixtureForSeason(param.SeriesId, param.Season);

            IEnumerable<SeriesFixture> filteredMatches;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMatches = seasonSummary.Where(m => m.HomeTeam.TeamName.ToLower().Contains(param.sSearch.ToLower())
                                                                   || m.AwayTeam.TeamName.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredMatches = seasonSummary;
            }

            var displayedMatches = filteredMatches.OrderByDescending(m => m.MatchRound);

            var result = from c in displayedMatches
                         select new[]
                                 {
                                     Convert.ToString(c.HtMatchId), c.MatchDate.ToString("dd/MM/yyyy"),
                                     c.MatchRound.ToString(), c.HomeTeam.TeamName, c.AwayTeam.TeamName, c.IsMatchMigrated.ToString()
                                 };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = seasonSummary.Count,
                iTotalDisplayRecords = seasonSummary.Count,                
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> StartMigrateMatchDetails(int seriesId, int season, int leagueId, string operationId)
        {
            if (!string.IsNullOrEmpty(operationId))
            {
                var seasonSummary =
                    _seriesRepository.GetSeriesFixtureForSeason(seriesId, season);

                if (!_migrationStatus.ContainsKey(operationId))
                {
                    _migrationStatus.Add(operationId, 0);
                }

                var matchDetails = seasonSummary.Where(m => m.IsMatchMigrated == false).ToList();

                await MigrateMatches(matchDetails,
                        season, leagueId, operationId);

                return Json(operationId);
            }
            else
            {
                return Json(false);
            }

        }

        private static readonly Dictionary<string, int> _migrationStatus = new Dictionary<string, int>();
        public async Task MigrateMatches(List<SeriesFixture> matches, int season, int leagueId, string operationId)
        {
            int matchesLeft = matches.Count;
            int totalMatches = matches.Count;

            var migrationService = new MigrateToNhibernateDomainService(_session);
            foreach (var match in matches)
            {
                migrationService.MigrateMatchDetails(match.HtMatchId, match.MatchRound, season, leagueId);

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
