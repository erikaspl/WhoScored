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

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        [TestMethod()]
        public void MigrateWorldDetailsTest_FullMigration()
        {
            MigrationDomainService target = new MigrationDomainService(); 
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

            var worldDetails = HattrickData.Deserialize(response);
         
            var dbService = new MongoService();
            dbService.MapWorldDetails<HattrickDataLeagueListLeague>();
            dbService.SaveWorldDetails(worldDetails.LeagueList.First().League.Cast<IWorldDetails>().ToList());
        }
    }
}
