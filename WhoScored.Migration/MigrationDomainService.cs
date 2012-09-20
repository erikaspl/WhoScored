using System;
using System.Linq;

using WhoScored.CHPP.WorldDetails.Serializer;
using WhoScored.Db.Mongo;
using System.Collections.Generic;
using WhoScored.CHPP.Files.HattrickFileAccessors;
using WhoScored.Model;

using LeagueDetails = WhoScored.CHPP.LeagueDetails.Serializer.HattrickData;

namespace WhoScored.Migration
{ 
    public class MigrationDomainService : MigrationDomainServiceBase
    {
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

        public override void MigrateWorldDetails()
        {
            var worldDetailsList = this.ReadWorldDetails();

            var dbService = new WhoScoredRepository();
            dbService.SaveWorldDetails(worldDetailsList);
        }

        public void MigrateLeagueDetails(List<int> seriesIdList)
        {
            var seriesDetails = new List<LeagueDetails>();
            foreach (int seriesId in seriesIdList)
            {
                var leagueDetailsRaw = new CHPP.Files.HattrickFileAccessors.
                    LeagueDetails(ProtectedResourceUrl) { LeagueLevelUnitID = seriesId };

                var request = new WhoScoredRequest();
                string response = request.MakeRequest(leagueDetailsRaw.GetHattrickFileAccessorAbsoluteUri());

                seriesDetails.Add(LeagueDetails.Deserialize(response));
            }

            var dbService = new WhoScoredRepository();
            dbService.SaveSeriesDetails(seriesDetails);            
        }

        public override void MigrateLeagueDetails(int countryId)
        {
            throw new NotImplementedException();
        }

        public override void MigrateFixtures(int seriesId, int season)
        {
            var seriesFixturesResult = new List<SeriesFixturesSummaryEntity>();
            seriesFixturesResult = GetSeriesFixtures(season, seriesId);
            var dbService = new WhoScoredRepository();
            dbService.SaveSeriesFixtures(seriesFixturesResult);
        }

        public override void MigrateMatchDetails(int matchId, int matchRound, int season, int leagueId)
        {
            var matchDetailsRaw = new MatchDetails(ProtectedResourceUrl)
                                      {
                                          MatchID = matchId,
                                          MatchEvents = true
                                      };

            var request = new WhoScoredRequest();
            string response = request.MakeRequest(matchDetailsRaw.GetHattrickFileAccessorAbsoluteUri());
            var matchDetails = CHPP.MatchDetails.Serializer.HattrickData.Deserialize(response);

            matchDetails.Match.First().MatchSeason = season.ToString();
            matchDetails.Match.First().LeagueLevelUnitID = leagueId.ToString();
            matchDetails.Match.First().MatchRound = matchRound;

            var dbSevice = new WhoScoredRepository();
            dbSevice.SaveMatchDetails(matchDetails.Match);
        }
    }
}
