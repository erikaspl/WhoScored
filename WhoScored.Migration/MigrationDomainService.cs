using System.Configuration;
using System.Linq;
using WhoScored.CHPP.WorldDetails.Serializer;
using WhoScored.Db.Mongo;
using System.Collections.Generic;
using WhoScored.CHPP.Files.HattrickFileAccessors;

namespace WhoScored.Migration
{


    public class MigrationDomainService
    {
        private readonly string _protectedResourceUrl;

        private List<string> _isInWhoScored = new List<string>();

        public MigrationDomainService()
        {
            _protectedResourceUrl = ConfigurationManager.AppSettings["protectedResourceUrl"];

            _isInWhoScored.Add("Lithuania");
        }

        public void MigrateWorldDetails()
        {
            var worldDetailsRaw = new WorldDetails(_protectedResourceUrl);          

            var request = new WhoScoredRequest();
            string response = request.MakeRequest(worldDetailsRaw.GetHattrickFileAccessorAbsoluteUri());

            var worldDetails = HattrickData.Deserialize(response);

            var worldDetailsList = worldDetails.LeagueList.First().League.ToList();

            SetIsInWhoScored(worldDetailsList);

            var dbService = new MongoService();
            dbService.SaveWorldDetails(worldDetails.LeagueList.First().League.ToList());
        }

        private void SetIsInWhoScored(IEnumerable<HattrickDataLeagueListLeague> worldDetails)
        {
            foreach (var country in _isInWhoScored)
            {
                worldDetails.First(w => w.EnglishName == country).LeagueInWhoScored = true;
            }
        }
    }
}
