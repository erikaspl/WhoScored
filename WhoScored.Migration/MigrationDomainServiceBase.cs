using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using WhoScored.CHPP.Files.HattrickFileAccessors;

namespace WhoScored.Migration
{
    using CHPP.WorldDetails.Serializer;
    using Model;

    using WorldDetails = CHPP.WorldDetails.Serializer.HattrickData;
    using LeagueDetails = CHPP.LeagueDetails.Serializer.HattrickData;
    using HattrickData = CHPP.SeriesFixtures.Serializer.HattrickData;

    public abstract class MigrationDomainServiceBase
    {
        protected readonly string ProtectedResourceUrl;

        protected readonly Dictionary<string, List<int>> IsInWhoScored = new Dictionary<string, List<int>>();

        protected MigrationDomainServiceBase()
        {
            ProtectedResourceUrl = ConfigurationManager.AppSettings["protectedResourceUrl"];

            IsInWhoScored.Add("Lithuania", GetLithuanianLeagueList());
        }

        private List<int> GetLithuanianLeagueList()
        {
            var leagueList = new List<int>();

            for (int i = 29747; i <= 29767; i++)
                leagueList.Add(i);

            return leagueList;
        }

        protected void SetIsInWhoScored(IEnumerable<HattrickDataLeagueListLeague> worldDetails)
        {
            foreach (var country in IsInWhoScored)
            {
                var league = worldDetails.First(w => w.EnglishName == country.Key);
                league.LeagueInWhoScored = true;
                league.SeriesIdList = country.Value;
            }
        }

        protected List<HattrickDataLeagueListLeague> ReadWorldDetails()
        {
            var worldDetailsRaw = new CHPP.Files.HattrickFileAccessors.WorldDetails(ProtectedResourceUrl);

            var request = new WhoScoredRequest();
            string response = request.MakeRequest(worldDetailsRaw.GetHattrickFileAccessorAbsoluteUri());

            var worldDetails = WorldDetails.Deserialize(response);

            var worldDetailsList = worldDetails.LeagueList.First().League.ToList();

            SetIsInWhoScored(worldDetailsList);
            return worldDetailsList;
        }

        protected LeagueDetails ReadSeriesDetails(int seriesId)
        {
            var leagueDetailsRaw = new CHPP.Files.HattrickFileAccessors.LeagueDetails(ProtectedResourceUrl) { LeagueLevelUnitID = seriesId };

            var request = new WhoScoredRequest();
            string response = request.MakeRequest(leagueDetailsRaw.GetHattrickFileAccessorAbsoluteUri());

            return LeagueDetails.Deserialize(response);           
        }

        protected List<SeriesFixturesSummaryEntity> GetSeriesFixtures(int season, int seriesId)
        {
            var seriesFixturesResult = new List<SeriesFixturesSummaryEntity>();
            var seriesFixturesRaw = new SeriesFixtures(ProtectedResourceUrl) { LeagueLevelUnitID = seriesId, Season = season };
            var request = new WhoScoredRequest();
            string response = request.MakeRequest(seriesFixturesRaw.GetHattrickFileAccessorAbsoluteUri());
            var seriesFixtures = HattrickData.Deserialize(response);

            seriesFixturesResult.Add(GetSeriesFixtureEntity(seriesFixtures));

            return seriesFixturesResult;
        }

        public static SeriesFixturesSummaryEntity GetSeriesFixtureEntity(HattrickData seriesFixtures)
        {
            var entity = new SeriesFixturesSummaryEntity
                             {
                                 LeagueLevelUnitID = int.Parse(seriesFixtures.LeagueLevelUnitID),
                                 LeagueLevelUnitName = seriesFixtures.LeagueLevelUnitName,
                                 Matches = new List<IMatchSummary>(),
                                 Season = int.Parse(seriesFixtures.Season)
                             };


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

        protected CHPP.MatchDetails.Serializer.HattrickData GetMatchDetails(int matchId)
        {
            var matchDetailsRaw = new MatchDetails(ProtectedResourceUrl)
                                      {
                                          MatchID = matchId,
                                          MatchEvents = true
                                      };

            var request = new WhoScoredRequest();
            string response = request.MakeRequest(matchDetailsRaw.GetHattrickFileAccessorAbsoluteUri());
            var matchDetails = CHPP.MatchDetails.Serializer.HattrickData.Deserialize(response);
            return matchDetails;
        }

        public abstract void MigrateLeagueDetails(int countryId);

        public abstract void MigrateFixtures(int seriesId, int season);

        public abstract void MigrateWorldDetails();

        public abstract void MigrateMatchDetails(int matchId, int matchRound, int season, int leagueId);
    }
}
