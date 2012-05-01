using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhoScored.CHPP.Serializer
{
    using WhoScored.Model;

    public partial class HattrickDataLeagueListLeague : IWorldDetails
	{
        public int LeagueId
        {
            get
            {
                return Int32.Parse(LeagueID);
            }
            set
            {
                LeagueID = value.ToString();
            }
        }

        public int NumbOfLevels
        {
            get
            {
                return Int32.Parse(NumberOfLevels);
            }
            set
            {
                NumberOfLevels = value.ToString();
            }
        }
	}
}
