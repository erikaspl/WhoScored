using System;
using System.Configuration;
using System.Linq;

using WhoScored.CHPP.WorldDetails.Serializer;
using WhoScored.Db.Mongo;
using System.Collections.Generic;
using WhoScored.CHPP.Files.HattrickFileAccessors;
using WhoScored.Model;

using LeagueDetails = WhoScored.CHPP.LeagueDetails.Serializer.HattrickData;
using WorldDetails = WhoScored.CHPP.WorldDetails.Serializer.HattrickData;
namespace WhoScored.Migration
{ 
    using HattrickData = WhoScored.CHPP.SeriesFixtures.Serializer.HattrickData;
    using HattrickDataMatch = WhoScored.CHPP.SeriesFixtures.Serializer.HattrickDataMatch;

    public class MigrationDomainService
    {
        private readonly string _protectedResourceUrl;

        private readonly Dictionary<string, List<int>> _isInWhoScored = new Dictionary<string, List<int>>();

        public static void RegisterMigrationClassMap()
        {
            WhoScoredRepository.MapWorldDetails<HattrickDataLeagueListLeague>();

            WhoScoredRepository.MapLeagueDetails<CHPP.LeagueDetails.Serializer.HattrickData>();

            WhoScoredRepository.MapMatchDetails<CHPP.MatchDetails.Serializer.HattrickDataMatch, CHPP.MatchDetails.Serializer.HattrickDataMatchArena, 
                CHPP.MatchDetails.Serializer.HattrickDataMatchHomeTeam, CHPP.MatchDetails.Serializer.HattrickDataMatchScorersGoal,
                CHPP.MatchDetails.Serializer.HattrickDataMatchBookingsBooking, CHPP.MatchDetails.Serializer.HattrickDataMatchInjuriesInjury, 
                CHPP.MatchDetails.Serializer.HattrickDataMatchEventListEvent>();

            WhoScoredRepository.MapSeriesFixtures<SeriesFixturesSummaryEntity, MatchSummaryEntity>();
        }

        public MigrationDomainService()
        {
            _protectedResourceUrl = ConfigurationManager.AppSettings["protectedResourceUrl"];

            _isInWhoScored.Add("Lithuania", GetLithuanianLeagueList());
        }

        private List<int> GetLithuanianLeagueList()
        {
            var leagueList = new List<int>();

            for (int i = 29747; i <= 29767; i++)
                leagueList.Add(i);

            return leagueList;
        }

        public void MigrateWorldDetails()
        {
            var worldDetailsRaw = new CHPP.Files.HattrickFileAccessors.WorldDetails(_protectedResourceUrl);

            var request = new WhoScoredRequest();
            string response = request.MakeRequest(worldDetailsRaw.GetHattrickFileAccessorAbsoluteUri());

            var worldDetails = WorldDetails.Deserialize(response);

            var worldDetailsList = worldDetails.LeagueList.First().League.ToList();

            SetIsInWhoScored(worldDetailsList);

            var dbService = new WhoScoredRepository();
            dbService.SaveWorldDetails(worldDetailsList);
        }

        private void SetIsInWhoScored(IEnumerable<HattrickDataLeagueListLeague> worldDetails)
        {
            foreach (var country in _isInWhoScored)
            {
                var league = worldDetails.First(w => w.EnglishName == country.Key);
                league.LeagueInWhoScored = true;
                league.SeriesIdList = country.Value;
            }
        }

        public void MigrateLeagueDetails(List<int> seriesIdList)
        {
            var seriesDetails = new List<LeagueDetails>();
            foreach (int seriesId in seriesIdList)
            {
                var leagueDetailsRaw = new CHPP.Files.HattrickFileAccessors.
                    LeagueDetails(_protectedResourceUrl) { LeagueLevelUnitID = seriesId };

                var request = new WhoScoredRequest();
                string response = request.MakeRequest(leagueDetailsRaw.GetHattrickFileAccessorAbsoluteUri());

                seriesDetails.Add(LeagueDetails.Deserialize(response));
            }

            var dbService = new WhoScoredRepository();
            dbService.SaveSeriesDetails(seriesDetails);            
        }

        public void MigrateFixtures(List<int> seriesIdList, int season)
        {
            var seriesFixturesResult = new List<SeriesFixturesSummaryEntity>();

            foreach (var seriesId in seriesIdList)
            {
                var seriesFixturesRaw = new SeriesFixtures(_protectedResourceUrl)
                                            {
                                                LeagueLevelUnitID = seriesId,
                                                Season = season
                                            };
                var request = new WhoScoredRequest();
                string response = request.MakeRequest(seriesFixturesRaw.GetHattrickFileAccessorAbsoluteUri());
                var seriesFixtures = HattrickData.Deserialize(response);

                seriesFixturesResult.Add(GetSeriesFixtureEntity(seriesFixtures));
                var dbService = new WhoScoredRepository();
                dbService.SaveSeriesFixtures(seriesFixturesResult);

            }
        }

        public static SeriesFixturesSummaryEntity GetSeriesFixtureEntity(HattrickData seriesFixtures)
        {
            var entity = new SeriesFixturesSummaryEntity();

            entity.LeagueLevelUnitID = int.Parse(seriesFixtures.LeagueLevelUnitID);
            entity.LeagueLevelUnitName = seriesFixtures.LeagueLevelUnitName;
            entity.Matches = new List<IMatchSummary>();
            entity.Season = int.Parse(seriesFixtures.Season);

            entity.Id = int.Parse(string.Format("{0}{1}", entity.LeagueLevelUnitID, entity.Season));

            foreach (var hattrickDataMatch in seriesFixtures.Match)
            {
                entity.Matches.Add(new MatchSummaryEntity
                    {
                        AwayGoals = !string.IsNullOrEmpty(hattrickDataMatch.AwayGoals)
                                                  ? (int?)int.Parse(hattrickDataMatch.AwayGoals)
                                                  : null,
                        HomeGoals = !string.IsNullOrEmpty(hattrickDataMatch.HomeGoals)
                                                  ? (int?)int.Parse(hattrickDataMatch.HomeGoals)
                                                  : null,
                        HomeTeamID = int.Parse(hattrickDataMatch.HomeTeam.First().HomeTeamID),
                        HomeTeamName = hattrickDataMatch.HomeTeam.First().HomeTeamName,
                        AwayTeamID = int.Parse(hattrickDataMatch.AwayTeam.First().AwayTeamID),
                        AwayTeamName = hattrickDataMatch.AwayTeam.First().AwayTeamName,
                        MatchDate = DateTime.Parse(hattrickDataMatch.MatchDate),
                        MatchID = int.Parse(hattrickDataMatch.MatchID),
                        MatchRound = int.Parse(hattrickDataMatch.MatchRound)
                    });
            }
            return entity;
        }
    }
}
