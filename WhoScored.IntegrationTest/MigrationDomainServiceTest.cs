using System.IO;
using System.Linq;
using System.Xml;
using WhoScored.CHPP.Serializer;
using WhoScored.Db.Mongo;
using WhoScored.Migration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WhoScored.Model;

namespace WhoScored.IntegrationTest
{
    using System.Threading;

    using WhoScored.Db;
    using WhoScored.Models;

    /// <summary>
    ///This is a test class for MigrationDomainServiceTest and is intended
    ///to contain all MigrationDomainServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MigrationDomainServiceTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }


        [TestMethod()]
        [DeploymentItem("oAuth.config")]
        public void MigrateWorldDetailsTest_FullMigration()
        {
            var target = new MigrationDomainService(); 
            target.MigrateWorldDetails();
        }

        static string GetXmlString(string strFile)
        {
            // Load the xml file into XmlDocument object.
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(strFile);
            }
            catch (XmlException e)
            {
                Console.WriteLine(e.Message);
            }
            // Now create StringWriter object to get data from xml document.
            StringWriter sw = new StringWriter();
            XmlTextWriter xw = new XmlTextWriter(sw);
            xmlDoc.WriteTo(xw);
            return sw.ToString();
        }

        [TestMethod()]
        [DeploymentItem("./Xml/worlddetails.xml")]
        public void MigrateWorldDetailsTest_FromXmlToDb()
        {
            string strFile = "worlddetails.xml";
            string response = GetXmlString(strFile);

            var worldDetailsInput = HattrickData.Deserialize(response);

            IWhoScoredDbService dbService = new MongoService();
            dbService.SaveWorldDetails(worldDetailsInput.LeagueList.First().League.ToList());

            Thread.Sleep(1000);

            var worldDetailsCount = dbService.GetWorldDetails<HattrickDataLeagueListLeague>().Count;

            dbService.DropWorldDetails();
            Assert.AreEqual(worldDetailsInput.LeagueList.First().League.Count, worldDetailsCount);           
        }



        [TestMethod()]
        [DeploymentItem("./Xml/worlddetails.xml")]
        public void UpdateWorldDetailsTest_FromXmlToDb()
        {
            string strFile = "worlddetails.xml";   
            string response = GetXmlString(strFile);

            var worldDetailsInput = HattrickData.Deserialize(response);

            IWhoScoredDbService dbService = new MongoService();
            dbService.SaveWorldDetails(worldDetailsInput.LeagueList.First().League.ToList());

            Thread.Sleep(1000);

            var worldDetails = dbService.GetWorldDetails<LeagueDetails>();

            int newNumberOfLevels = 100;
            string newEnglishName = "newEnglishName";
            string newLeagueName = "newLeagueName";

            var lithData = worldDetails.Where(w => w.EnglishName == "Lithuania").First();
            lithData.NumberOfLevels = newNumberOfLevels;
            lithData.LeagueName = newLeagueName;
            lithData.EnglishName = newEnglishName;

            dbService.SaveWorldDetails(lithData);

            worldDetails = dbService.GetWorldDetails<LeagueDetails>();
            lithData = worldDetails.Where(w => w.EnglishName == newEnglishName).First();

            Assert.AreEqual(lithData.EnglishName, newEnglishName);
            Assert.AreEqual(lithData.LeagueName, newLeagueName);
            Assert.AreEqual(lithData.NumberOfLevels, newNumberOfLevels);

            dbService.DropWorldDetails();
        }
    }
}
