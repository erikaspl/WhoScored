using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhoScored.CHPP.Files.HattrickFileAccessors
{
    using System.Globalization;

    public class MatchDetails : HattrickFileAccessor
    {
        public MatchDetails(string protectedResourceUrl)
            : base(protectedResourceUrl)
        {
        }

        private const string FILE_PARAMETER_VALUE = "matchdetails";
        private const string VERSION_PARAMETER_VALUE = "2.3";

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
                           MatchIDParameter,
                           MatchEventsParameter                           
                       };
        }


        #region MatchEvents

        private const string MATCH_EVENTS_PARAMETER_NAME = "matchEvents";
        public bool MatchEvents
        {
            set
            {
                this._matchEventsParameter = new RequestInputParameter(MATCH_EVENTS_PARAMETER_NAME, value.ToString(CultureInfo.InvariantCulture));
            }
        }

        private IRequestInputParameter _matchEventsParameter = new RequestInputParameterNullValue();
        protected IRequestInputParameter MatchEventsParameter
        {
            get { return this._matchEventsParameter; }
            set { this._matchEventsParameter = value; }
        }
        #endregion

        #region matchID

        private const string MATCH_ID_PARAMETER_NAME = "matchID";
        public int? MatchID
        {
            set
            {
                if (value.HasValue)
                    this._matchIDParameter = new RequestInputParameter(MATCH_ID_PARAMETER_NAME, value.Value.ToString(CultureInfo.InvariantCulture));
                else
                    this._matchIDParameter = new RequestInputParameterNullValue();

            }
        }

        private IRequestInputParameter _matchIDParameter = new RequestInputParameterNullValue();
        protected IRequestInputParameter MatchIDParameter
        {
            get { return this._matchIDParameter; }
            set { this._matchIDParameter = value; }
        }
        #endregion

    }
}
