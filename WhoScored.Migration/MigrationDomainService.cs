using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
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
            dbService.MapWorldDetails<HattrickDataLeagueListLeague>();
            dbService.SaveWorldDetails(worldDetails.LeagueList.Cast<IWorldDetails>().ToList());
        }

    }
}
