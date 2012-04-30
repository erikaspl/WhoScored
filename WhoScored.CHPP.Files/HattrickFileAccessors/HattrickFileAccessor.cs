using System.Collections.Generic;
using System.Web;

namespace WhoScored.CHPP.Files.HattrickFileAccessors
{
    /// <summary>
    /// This is a base class for all Hattrick File Accessors.
    /// Hattrick File Accessors can provide parameters to construct query strings to request CHPP Files.
    /// </summary>
    public abstract class HattrickFileAccessor
    {
        protected const string FILE_PARAMETER_NAME = "file";
        protected const string VERSION_PARAMETER_NAME = "version";

        protected abstract string FileParameterValue { get; }
        protected abstract string VersionParameterValue { get; }

        protected abstract List<IRequestInputParameter> GetFileSpecificParameters();

        private readonly string _protectedResourceUrl;

        protected string ProtectedResourceUrl
        {
            get { return _protectedResourceUrl; }
        }

        protected HattrickFileAccessor(string protectedResourceUrl)
        {
            _protectedResourceUrl = HttpUtility.UrlPathEncode(protectedResourceUrl);
        }

        /// <summary>
        /// Formats absolute uri using provided resource url and query parameters.
        /// </summary>
        /// <returns>Returns absolute uri as string</returns>
        public string GetHattrickFileAccessorAbsoluteUri()
        {
            var fileInputParameter = new RequestInputParameter(FILE_PARAMETER_NAME, FileParameterValue);
            string htFileAccessor = string.Format("{0}?{1}", _protectedResourceUrl, fileInputParameter.GetRequestParameterQueryString());

            var versionParameterValue = new RequestInputParameter(VERSION_PARAMETER_NAME, VersionParameterValue);
            htFileAccessor += GetQueryString(versionParameterValue);

            foreach (var parameter in GetFileSpecificParameters())
            {
                htFileAccessor += GetQueryString(parameter);
            }

            return htFileAccessor;
        }


        /// <summary>
        /// Creates query string using provided parameter.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>Query string</returns>
        private static string GetQueryString(IRequestInputParameter parameter)
        {
            if (parameter.HasValue)
            {
                 return string.Format("&{0}", parameter.GetRequestParameterQueryString());
            }
            return string.Empty;
        }
    }
}