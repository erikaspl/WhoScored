using System.Configuration;
using System.Linq;
using WhoScored.Db.Mongo;

namespace WhoScored.Migration
{
    using System.Collections.Generic;

    using WhoScored.CHPP.Files.HattrickFileAccessors;
    using WhoScored.CHPP.Serializer;

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
                worldDetails.Where(w => w.EnglishName == country).First().LeagueInWhoScored = true;
            }
        }
    }
}
