using System.IO;
using System.Linq;
using System.Xml;
using WhoScored.CHPP.WorldDetails.Serializer;
using WhoScored.Db.Mongo;
using WhoScored.Migration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WhoScored.Model;

namespace WhoScored.IntegrationTest
{
    using System.Threading;

    using WhoScored.CHPP.MatchDetails.Serializer;
    using WhoScored.Controllers;
    using WhoScored.Db;
    using WhoScored.Models;

    using HattrickData = WhoScored.CHPP.WorldDetails.Serializer.HattrickData;

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

        [TestInitialize]
        public void TestInitialize()
        {
            MigrationDomainService.RegisterMigrationClassMap();
            RegisterMappings.RegisterModelClassMap();
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

            IWhoScoredRepository repository = new WhoScoredRepository();
            repository.SaveWorldDetails(worldDetailsInput.LeagueList.First().League.ToList());

            Thread.Sleep(1000);

            var worldDetailsCount = repository.GetWorldDetails<HattrickDataLeagueListLeague>().Count;

            repository.DropWorldDetails();
            Assert.AreEqual(worldDetailsInput.LeagueList.First().League.Count, worldDetailsCount);           
        }


        [TestMethod()]
        [DeploymentItem("./Xml/worlddetails.xml")]
        public void GetWorldDetailTest_LoadOneWordlDetailFromDB()
        {
            string strFile = "worlddetails.xml";
            string response = GetXmlString(strFile);

            var worldDetailsInput = HattrickData.Deserialize(response);

            IWhoScoredRepository repository = new WhoScoredRepository();
            repository.SaveWorldDetails(worldDetailsInput.LeagueList.First().League.ToList());

            Thread.Sleep(1000);

            var countryDetails = repository.GetWorldDetails<HattrickDataLeagueListLeague>(66);

            repository.DropWorldDetails();
            Assert.AreEqual(worldDetailsInput.LeagueList.First().League.Where(l => l.LeagueID == 66).First().LeagueName, countryDetails.LeagueName);
        }





        [TestMethod()]
        [DeploymentItem("./Xml/worlddetails.xml")]
        public void UpdateWorldDetailsTest_FromXmlToDb()
        {
            string strFile = "worlddetails.xml";   
            string response = GetXmlString(strFile);

            var worldDetailsInput = HattrickData.Deserialize(response);

            IWhoScoredRepository repository = new WhoScoredRepository();
            repository.SaveWorldDetails(worldDetailsInput.LeagueList.First().League.ToList());

            Thread.Sleep(1000);

            var worldDetails = repository.GetWorldDetails<CountryDetails>();

            int newNumberOfLevels = 100;
            string newEnglishName = "newEnglishName";
            string newLeagueName = "newLeagueName";

            var lithData = worldDetails.Where(w => w.EnglishName == "Lithuania").First();
            lithData.NumberOfLevels = newNumberOfLevels;
            lithData.LeagueName = newLeagueName;
            lithData.EnglishName = newEnglishName;

            repository.SaveWorldDetails(lithData);

            worldDetails = repository.GetWorldDetails<CountryDetails>();
            lithData = worldDetails.Where(w => w.EnglishName == newEnglishName).First();

            Assert.AreEqual(lithData.EnglishName, newEnglishName);
            Assert.AreEqual(lithData.LeagueName, newLeagueName);
            Assert.AreEqual(lithData.NumberOfLevels, newNumberOfLevels);

            repository.DropWorldDetails();
        }


        [TestMethod()]
        [DeploymentItem("./Xml/leaguedetails.xml")]
        public void MigrateLeagueDetailsTest_FromXmlToDb()
        {
            string strFile = "leaguedetails.xml";
            string response = GetXmlString(strFile);

            var leagueDetailsInput = CHPP.LeagueDetails.Serializer.HattrickData.Deserialize(response);

            IWhoScoredRepository repository = new WhoScoredRepository();
            repository.SaveSeriesDetails(leagueDetailsInput);

            Thread.Sleep(1000);

            var worldDetailsCount = repository.GetSeriesDetails<CHPP.LeagueDetails.Serializer.HattrickData>().Count;

            repository.DropSeriesDetails();
            Assert.AreEqual(1, worldDetailsCount);
        }


        [TestMethod()]
        [DeploymentItem("./Xml/seriesfixtures.xml")]
        public void MigrateSeriesFixturesTest_FromXmlToDb()
        {
            string strFile = "seriesfixtures.xml";
            string response = GetXmlString(strFile);

            var seriesFixturesInput = CHPP.SeriesFixtures.Serializer.HattrickData.Deserialize(response);

            IWhoScoredRepository repository = new WhoScoredRepository();
            var entity = MigrationDomainService.GetSeriesFixtureEntity(seriesFixturesInput);
            repository.SaveSeriesFixtures(entity);

            Thread.Sleep(1000);

            var fixturesCount = repository.GetSeriesFixturesSummary<SeriesFixturesSummaryEntity>().Count;

            repository.DropSeriesFixtures();
            Assert.AreEqual(1, fixturesCount);
        }


        [TestMethod()]
        [DeploymentItem("./Xml/matchdetails.xml")]
        public void MigrateMatchDetailsTest_FromXmlToDb()
        {
            string strFile = "matchdetails.xml";
            string response = GetXmlString(strFile);

            var matchDetailsInput = CHPP.MatchDetails.Serializer.HattrickData.Deserialize(response).Match.First();

            IWhoScoredRepository repository = new WhoScoredRepository();

            repository.SaveMatchDetails(matchDetailsInput);

            Thread.Sleep(1000);

            var matchDetails = repository.GetMatchDetails<MatchDetails>();

            repository.DropSeriesFixtures();
            Assert.AreEqual(1, matchDetails.Count);
        }
    }
}
