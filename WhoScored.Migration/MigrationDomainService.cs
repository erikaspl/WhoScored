using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using WhoScored.CHPP.Files.HattrickFileAccessors;
using WhoScored.CHPP.Serializer;

namespace WhoScored.Migration
{
    public class MigrationDomainService
    {
        private string _protectedResourceUrl;

        public MigrationDomainService()
        {
            _protectedResourceUrl = ConfigurationManager.AppSettings["protectedResourceUrl"];
        }

        public void MigrateWorldDetails()
        {
            var worldDetails = new WorldDetails(_protectedResourceUrl);          

            var request = new WhoScoredRequest();
            string response = request.MakeRequest(worldDetails.GetHattrickFileAccessorAbsoluteUri());

            HattrickData hattrickData = HattrickData.Deserialize(response);
        }
    }
}
