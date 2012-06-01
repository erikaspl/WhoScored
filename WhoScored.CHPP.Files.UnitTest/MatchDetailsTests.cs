using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WhoScored.CHPP.Files.UnitTest
{
    using System.Globalization;

    using WhoScored.CHPP.Files.HattrickFileAccessors;

    [TestClass]
    public class MatchDetailsTests
    {
        [TestMethod()]
        public void MatchDetailsAccessor_ProvideNotAllowedChar_ExpectFixedUrl()
        {
            const string protectedUrl = "thisIs  ProtectedUrl";
            var matchDetails = new MatchDetails(protectedUrl);
            string result = matchDetails.GetHattrickFileAccessorAbsoluteUri();

            string expectedUrl = string.Format("{0}?{1}", "thisIs%20%20ProtectedUrl", "file=matchdetails&version=2.3");
            Assert.AreEqual(result, expectedUrl);
        }

        [TestMethod()]
        public void MatchDetailsAccessor_DefaultBehaviout_ExpectDefaultUrl()
        {
            const string protectedUrl = "thisIsProtectedUrl";
            var matchDetails = new MatchDetails(protectedUrl);
            string result = matchDetails.GetHattrickFileAccessorAbsoluteUri();

            string expectedUrl = string.Format("{0}?{1}", protectedUrl, "file=matchdetails&version=2.3");
            Assert.AreEqual(result, expectedUrl);
        }

        [TestMethod()]
        public void MatchDetailsAccessor_ProvideMatchId_ExpectLegueIdInUrl()
        {
            const string protectedUrl = "thisIsProtectedUrl";
            var matchDetails = new MatchDetails(protectedUrl);
            const int matchId = 1234;
            matchDetails.MatchID = matchId;
            string result = matchDetails.GetHattrickFileAccessorAbsoluteUri();

            string expectedUrl = string.Format("{0}?{1}&{2}={3}", protectedUrl, "file=matchdetails&version=2.3", "matchID", matchId.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(result, expectedUrl);
        }

        [TestMethod()]
        public void MatchDetailsAccessor_SetMatchIdToNull_ExpectNoLegueIdInUrl()
        {
            const string protectedUrl = "thisIsProtectedUrl";
            var matchDetails = new MatchDetails(protectedUrl);

            matchDetails.MatchID = null;
            string result = matchDetails.GetHattrickFileAccessorAbsoluteUri();

            string expectedUrl = string.Format("{0}?{1}", protectedUrl, "file=matchdetails&version=2.3");
            Assert.AreEqual(result, expectedUrl);
        }

        [TestMethod()]
        public void MatchDetailsAccessor_ProvideMatchEvents_ExpectSeasonInUrl()
        {
            const string protectedUrl = "thisIsProtectedUrl";
            var matchDetails = new MatchDetails(protectedUrl);
            const bool matchEvents = true;
            matchDetails.MatchEvents = matchEvents;
            string result = matchDetails.GetHattrickFileAccessorAbsoluteUri();

            string expectedUrl = string.Format("{0}?{1}&{2}={3}", protectedUrl, "file=matchdetails&version=2.3", "matchEvents", matchEvents.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(result, expectedUrl);
        }

        [TestMethod()]
        public void MatchDetailsAccessor_ProvideSeasonAndLeagueId_ExpectSeasonAndLeagueIdInUrl()
        {
            const string protectedUrl = "thisIsProtectedUrl";
            var matchDetails = new MatchDetails(protectedUrl);
            const bool matchEvents = true;
            const int matchId = 9876;
            matchDetails.MatchEvents = matchEvents;
            matchDetails.MatchID = matchId;
            string result = matchDetails.GetHattrickFileAccessorAbsoluteUri();

            string expectedUrl = string.Format("{0}?{1}&{2}={3}&{4}={5}", protectedUrl, "file=matchdetails&version=2.3",
                "matchID", matchId.ToString(CultureInfo.InvariantCulture),
                "matchEvents", matchEvents.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(result, expectedUrl);
        }
    }
}
