using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhoScored.CHPP.LeagueDetails.Serializer;

namespace WhoScored.CHPP.Serializer.UnitTest
{
    [TestClass]
    public class LeagueDetails
    {
        [TestMethod]
        [DeploymentItem("./Xml/leaguedetails.xml")]
        public void SerializeLeagueDetails()
        {
            string strFile = "leaguedetails.xml";
            string response = XmlUtil.GetXmlString(strFile);

            var leagueDetailsInput = HattrickData.Deserialize(response);

            Assert.IsTrue(leagueDetailsInput.Team.Count > 0);
        }
    }
}
