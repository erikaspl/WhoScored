using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhoScored.CHPP.WorldDetails.Serializer;

namespace WhoScored.CHPP.Serializer.UnitTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class WorldDetails
    {
        [TestMethod]
        [DeploymentItem("./Xml/worlddetails.xml")]
        public void SerializeWorldDetails()
        {
            string strFile = "worlddetails.xml";
            string response = XmlUtil.GetXmlString(strFile);

            var worldDetailsInput = HattrickData.Deserialize(response);

            Assert.IsTrue(worldDetailsInput.LeagueList.First().League.ToList().Count > 0);
        }
    }
}
