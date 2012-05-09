using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhoScored.Db.Connection;
using WhoScored.Db.Mongo;
using WhoScored.Model;
using WhoScored.Models;

namespace WhoScored.IntegrationTest
{
    [TestClass]
    public class SettingsTests
    {
        [TestMethod]
        public void LoadSettingsTest()
        {
            MongoService.MapSettings<Settings>();
            var database = MongoConnector.GetDatabase();

            const int globalSeasonId = 48;

            var settingsInput = new Settings{GlobalSeason = globalSeasonId};
            
            var collection = database.GetCollection<ISettings>(MongoService.SETTINGS_COLLECTION_NAME);

            collection.Save(settingsInput);

            collection = database.GetCollection<ISettings>(MongoService.SETTINGS_COLLECTION_NAME);

            var settings  = collection.FindAll().ToList().First();

            collection.Drop();

            Assert.IsTrue(settings.GlobalSeason == globalSeasonId);
        }
    }
}
