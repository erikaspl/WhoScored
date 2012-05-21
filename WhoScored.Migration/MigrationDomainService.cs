using System;
using System.Configuration;
using System.Linq;
using WhoScored.CHPP.SeriesFixtures.Serializer;
using WhoScored.CHPP.WorldDetails.Serializer;
using WhoScored.Db.Mongo;
using System.Collections.Generic;
using WhoScored.CHPP.Files.HattrickFileAccessors;
using WhoScored.Model;
using HattrickData = WhoScored.CHPP.SeriesFixtures.Serializer.HattrickData;
using LeagueDetails = WhoScored.CHPP.LeagueDetails.Serializer.HattrickData;
using WorldDetails = WhoScored.CHPP.WorldDetails.Serializer.HattrickData;
namespace WhoScored.Migration
{


    public class MigrationDomainService
    {
        private readonly string _protectedResourceUrl;

        private readonly Dictionary<string, List<int>> _isInWhoScored = new Dictionary<string, List<int>>();

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
            dbService.SaveWorldDetails(worldDetails.LeagueList.First().League.ToList());
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
            var seriesFixturesResult = new List<SeriesFixturesEntity>();

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

                var entity = new SeriesFixturesEntity();

                entity.LeagueLevelUnitID = int.Parse(seriesFixtures.LeagueLevelUnitID);
                entity.LeagueLevelUnitName = seriesFixtures.LeagueLevelUnitName;
                entity.Matches = new List<IMatchSummary>();

                foreach (var hattrickDataMatch in seriesFixtures.Match)
                {
                    entity.Matches.Add(new MatchSummaryEntity
                                           {
                                               AwayGoals = int.Parse(hattrickDataMatch.AwayGoals),
                                               HomeGoals = int.Parse(hattrickDataMatch.HomeGoals),
                                               HomeTeamID = int.Parse(hattrickDataMatch.HomeTeam.First().HomeTeamID),
                                               HomeTeamName = hattrickDataMatch.HomeTeam.First().HomeTeamName,
                                               AwayTeamID = int.Parse(hattrickDataMatch.AwayTeam.First().AwayTeamID),
                                               AwayTeamName = hattrickDataMatch.AwayTeam.First().AwayTeamName,
                                               MatchDate = DateTime.Parse(hattrickDataMatch.MatchDate),
                                               MatchID = int.Parse(hattrickDataMatch.MatchID),
                                               MatchRound = int.Parse(hattrickDataMatch.MatchRound)
                                           });
                }

                var dbService = new WhoScoredRepository();
                dbService.SaveSeriesFixtures(seriesFixturesResult);

            }
        }
    }
}
