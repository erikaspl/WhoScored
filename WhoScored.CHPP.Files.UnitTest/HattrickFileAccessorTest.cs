using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using WhoScored.CHPP.Files.HattrickFileAccessors;

namespace WhoScored.CHPP.Files.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for HTFileAccessorTest and is intended
    ///to contain all HTFileAccessorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class HattrickFileAccessorTest
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


        internal virtual HattrickFileAccessor CreateHTFileAccessor()
        {
            // TODO: Instantiate an appropriate concrete class.
            HattrickFileAccessor target = null;
            return target;
        }

        [TestMethod()]
        public void GetHattrickFileAccessorTest_ProvideNotAllowedChar_ExpectFixedUrl()
        {
            const string protectedUrl = "thisIs  ProtectedUrl";
            var leagueFixtures = new SeriesFixtures(protectedUrl);
            string result = leagueFixtures.GetHattrickFileAccessorAbsoluteUri();

            string expectedUrl = string.Format("{0}?{1}", "thisIs%20%20ProtectedUrl", "file=leaguefixtures&version=1.2");
            Assert.AreEqual(result, expectedUrl);
        }

        [TestMethod()]
        public void GetHattrickFileAccessorTest_DefaultBehaviout_ExpectDefaultUrl()
        {
            const string protectedUrl = "thisIsProtectedUrl";
            var leagueFixtures = new SeriesFixtures(protectedUrl);
            string result = leagueFixtures.GetHattrickFileAccessorAbsoluteUri();

            string expectedUrl = string.Format("{0}?{1}", protectedUrl, "file=leaguefixtures&version=1.2");
            Assert.AreEqual(result, expectedUrl);
        }

        [TestMethod()]
        public void GetHattrickFileAccessorTest_ProvideLeagueId_ExpectLegueIdInUrl()
        {
            const string protectedUrl = "thisIsProtectedUrl";
            var leagueFixtures = new SeriesFixtures(protectedUrl);
            const int leagueLevelUnitID = 1234;
            leagueFixtures.LeagueLevelUnitID = leagueLevelUnitID;
            string result = leagueFixtures.GetHattrickFileAccessorAbsoluteUri();

            string expectedUrl = string.Format("{0}?{1}&{2}={3}", protectedUrl, "file=leaguefixtures&version=1.2", "leagueLevelUnitID", leagueLevelUnitID.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(result, expectedUrl);
        }

        [TestMethod()]
        public void GetHattrickFileAccessorTest_SetLeagueIdToNull_ExpectNoLegueIdInUrl()
        {
            const string protectedUrl = "thisIsProtectedUrl";
            var leagueFixtures = new SeriesFixtures(protectedUrl);

            leagueFixtures.LeagueLevelUnitID = null;
            string result = leagueFixtures.GetHattrickFileAccessorAbsoluteUri();

            string expectedUrl = string.Format("{0}?{1}", protectedUrl, "file=leaguefixtures&version=1.2");
            Assert.AreEqual(result, expectedUrl);
        }

        [TestMethod()]
        public void GetHattrickFileAccessorTest_ProvideSeason_ExpectSeasonInUrl()
        {
            const string protectedUrl = "thisIsProtectedUrl";
            var leagueFixtures = new SeriesFixtures(protectedUrl);
            const int season = 1234;
            leagueFixtures.Season = season;
            string result = leagueFixtures.GetHattrickFileAccessorAbsoluteUri();

            string expectedUrl = string.Format("{0}?{1}&{2}={3}", protectedUrl, "file=leaguefixtures&version=1.2", "season", season.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(result, expectedUrl);
        }

        [TestMethod()]
        public void GetHattrickFileAccessorTest_SetSeasonToNull_ExpectNoSeasonInUrl()
        {
            const string protectedUrl = "thisIsProtectedUrl";
            var leagueFixtures = new SeriesFixtures(protectedUrl);

            leagueFixtures.Season = null;
            string result = leagueFixtures.GetHattrickFileAccessorAbsoluteUri();

            string expectedUrl = string.Format("{0}?{1}", protectedUrl, "file=leaguefixtures&version=1.2");
            Assert.AreEqual(result, expectedUrl);
        }

        [TestMethod()]
        public void GetHattrickFileAccessorTest_ProvideSeasonAndLeagueId_ExpectSeasonAndLeagueIdInUrl()
        {
            const string protectedUrl = "thisIsProtectedUrl";
            var leagueFixtures = new SeriesFixtures(protectedUrl);
            const int season = 1234;
            const int leagueId = 9876;
            leagueFixtures.Season = season;
            leagueFixtures.LeagueLevelUnitID = leagueId;
            string result = leagueFixtures.GetHattrickFileAccessorAbsoluteUri();

            string expectedUrl = string.Format("{0}?{1}&{2}={3}&{4}={5}", protectedUrl, "file=leaguefixtures&version=1.2",
                "leagueLevelUnitID", leagueId.ToString(CultureInfo.InvariantCulture),
                "season", season.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(result, expectedUrl);
        }
    }
}
