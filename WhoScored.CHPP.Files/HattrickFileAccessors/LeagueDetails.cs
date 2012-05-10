using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace WhoScored.CHPP.Files.HattrickFileAccessors
{
    public class LeagueDetails : HattrickFileAccessor
    {
        private const string FILE_PARAMETER_VALUE = "leaguedetails";
        private const string VERSION_PARAMETER_VALUE = "1.4";

        public LeagueDetails(string protectedResourceUrl) : base(protectedResourceUrl)
        {

        }

        protected override string FileParameterValue
        {
            get { return FILE_PARAMETER_VALUE; }
        }

        protected override string VersionParameterValue
        {
            get { return VERSION_PARAMETER_VALUE; }
        }

        protected override List<IRequestInputParameter> GetFileSpecificParameters()
        {
            return new List<IRequestInputParameter>()
                       {
                           LeagueLevelUnitIDParameter
                       };
        }


        private const string LEAGUE_LEVEL_UNIT_ID_PARAMETER_NAME = "leagueLevelUnitID";

        #region LeagueLevelUnitID
        public int? LeagueLevelUnitID
        {
            set
            {
                if (value.HasValue)
                    _leagueLevelUnitIDParameter = new RequestInputParameter(LEAGUE_LEVEL_UNIT_ID_PARAMETER_NAME, value.Value.ToString(CultureInfo.InvariantCulture));
                else
                    _leagueLevelUnitIDParameter = new RequestInputParameterNullValue();

            }
        }

        private IRequestInputParameter _leagueLevelUnitIDParameter = new RequestInputParameterNullValue();
        protected IRequestInputParameter LeagueLevelUnitIDParameter
        {
            get { return _leagueLevelUnitIDParameter; }
            set { _leagueLevelUnitIDParameter = value; }
        }
        #endregion
    }
}
