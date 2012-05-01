using System.Configuration;
using System.Linq;
using WhoScored.Db.Mongo;
using WhoScored.Model;

namespace WhoScored.Migration
{
    using WhoScored.CHPP.Files.HattrickFileAccessors;
    using WhoScored.CHPP.Serializer;

    public class MigrationDomainService
    {
        private readonly string _protectedResourceUrl;

        public MigrationDomainService()
        {
            _protectedResourceUrl = ConfigurationManager.AppSettings["protectedResourceUrl"];
        }

        public void MigrateWorldDetails()
        {
            var worldDetailsRaw = new WorldDetails(_protectedResourceUrl);          

            var request = new WhoScoredRequest();
            string response = request.MakeRequest(worldDetailsRaw.GetHattrickFileAccessorAbsoluteUri());

            var worldDetails = HattrickData.Deserialize(response);

            var dbService = new MongoService();
            dbService.SaveWorldDetails(worldDetails.LeagueList.First().League.Cast<IWorldDetails>().ToList());
        }
    }
}
