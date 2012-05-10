using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace WhoScored.CHPP.Files.HattrickFileAccessors
{
    /// <summary>
    /// This CHPP File Accessor provides parameters to access league fixtures CHPP file
    /// </summary>
    public class LeagueFixtures : HattrickFileAccessor
    {
        private const string FILE_PARAMETER_VALUE = "leaguefixtures";
        private const string VERSION_PARAMETER_VALUE = "1.2";

        protected override string FileParameterValue
        {
            get { return FILE_PARAMETER_VALUE; }
        }

        protected override string VersionParameterValue
        {
            get { return VERSION_PARAMETER_VALUE; }
        }

        private const string LEAGUE_LEVEL_UNIT_ID_PARAMETER_NAME = "leagueLevelUnitID";
        private const string SEASON_PARAMETER_NAME = "season";

        public LeagueFixtures(string protectedResourceUrl) : base(protectedResourceUrl)
        {
        }

        protected override List<IRequestInputParameter> GetFileSpecificParameters()
        {
            return new List<IRequestInputParameter>
                       {
                           LeagueLevelUnitIDParameter,
                           SeasonParameter
                       };
        }

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

        #region Season
        public int? Season
        {
            set
            {
                if (value.HasValue)
                {
                    _seasonParameter = new RequestInputParameter(SEASON_PARAMETER_NAME, value.Value.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    _seasonParameter = new RequestInputParameterNullValue();
                }
            }
        }

        private IRequestInputParameter _seasonParameter = new RequestInputParameterNullValue();
        protected IRequestInputParameter SeasonParameter
        {
            get { return _seasonParameter; }
            set { _seasonParameter = value; }
        }

        #endregion
    }
}