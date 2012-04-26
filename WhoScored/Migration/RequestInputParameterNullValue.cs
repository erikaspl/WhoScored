using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhoScored.Migration
{
    public class RequestInputParameterNullValue : IRequestInputParameter
    {

        public string GetRequestParameterQueryString()
        {
            return string.Empty;
        }

        #region IRequestInputParameter Members


        public bool HasValue
        {
            get { return false; }
        }

        #endregion
    }
}