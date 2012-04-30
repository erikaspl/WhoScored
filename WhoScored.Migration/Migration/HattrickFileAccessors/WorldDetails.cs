using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhoScored.Migration.HattrickFileAccessors
{
    using System.Globalization;

    using WhoScored.Migration.HattrickFileAccessors;

    public class WorldDetails : HattrickFileAccessor
    {
        public WorldDetails(string protectedResourceUrl)
            : base(protectedResourceUrl)
        {
        }

        private const string FILE_PARAMETER_VALUE = "worlddetails";
        private const string VERSION_PARAMETER_VALUE = "1.4";

        protected override string FileParameterValue
        {
            get
            {
                return FILE_PARAMETER_VALUE;
            }
        }

        protected override string VersionParameterValue
        {
            get
            {
                return VERSION_PARAMETER_VALUE;
            }
        }

        protected override List<IRequestInputParameter> GetFileSpecificParameters()
        {
            return new List<IRequestInputParameter>
                       {
                           CountryIdParameter,
                           LeagueParameter
                       };
        }

        #region Country
        private const string COUNTRY_ID = "countryID ";
        public int? CountryId
        {
            set
            {
                if (value.HasValue)
                    this._countryIdParameter = new RequestInputParameter(COUNTRY_ID, value.Value.ToString(CultureInfo.InvariantCulture));
                else
                    this._countryIdParameter = new RequestInputParameterNullValue();

            }
        }

        private IRequestInputParameter _countryIdParameter = new RequestInputParameterNullValue();
        protected IRequestInputParameter CountryIdParameter
        {
            get { return this._countryIdParameter; }
            set { this._countryIdParameter = value; }
        }
        #endregion

        #region League

        private const string LEAGUE_PARAMETER_NAME = "leagueID";
        public int? League
        {
            set
            {
                if (value.HasValue)
                {
                    this._leagueParameter = new RequestInputParameter(LEAGUE_PARAMETER_NAME, value.Value.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    this._leagueParameter = new RequestInputParameterNullValue();
                }
            }
        }

        private IRequestInputParameter _leagueParameter = new RequestInputParameterNullValue();
        protected IRequestInputParameter LeagueParameter
        {
            get { return this._leagueParameter; }
            set { this._leagueParameter = value; }
        }

        #endregion
    }
}
