using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WhoScored.CHPP.Serializer.UnitTest
{
    using WhoScored.CHPP.MatchDetails.Serializer;

    [TestClass]
    public class MatchDetails
    {
        [TestMethod]
        [DeploymentItem("./Xml/matchdetails.xml")]
        public void SerializeMatchDetails()
        {
            string strFile = "matchdetails.xml";
            string response = XmlUtil.GetXmlString(strFile);

            var matchDetails = HattrickData.Deserialize(response);

            Assert.IsTrue(matchDetails.Match[0].EventList.Count > 0);
        }
    }
}
