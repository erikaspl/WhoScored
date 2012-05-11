using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhoScored.Model;

namespace WhoScored.CHPP.WorldDetails.Serializer
{
    using System.Xml.Serialization;

    public partial class HattrickDataLeagueListLeague : IWorldDetails
	{
        [XmlIgnore]
        public bool LeagueInWhoScored { get; set; }

        [XmlIgnore]
        public List<int> SeriesIdList { get; set; }
	}
}
