using System.Web;

namespace WhoScored.CHPP.Files
{
    public class RequestInputParameter : IRequestInputParameter
    {
        private readonly string _parameterName;
        private readonly string _parameterValue;

        public RequestInputParameter(string parameterName, string parameterValue)
        {
            _parameterName = HttpUtility.UrlEncode(parameterName);
            _parameterValue = HttpUtility.UrlEncode(parameterValue);
        }

        public string GetRequestParameterQueryString()
        {
            return string.Format("{0}={1}", _parameterName, _parameterValue);
        }

        public bool HasValue
        {
            get { return !string.IsNullOrEmpty(_parameterValue); }
        }
    }
}