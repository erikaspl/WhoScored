using System.Configuration;
using System.Linq;
using WhoScored.CHPP.WorldDetails.Serializer;
using WhoScored.Db.Mongo;
using System.Collections.Generic;
using WhoScored.CHPP.Files.HattrickFileAccessors;

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

            var dbService = new MongoService();
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

            var dbService = new MongoService();
            dbService.SaveLeagueDetails(seriesDetails);
            
        }
    }
}
