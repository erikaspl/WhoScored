using System.Collections;
using System.Collections.Generic;
using System.Web;

namespace WhoScored.Migration.HattrickFileAccessors
{
    public abstract class HattrickFileAccessor
    {
        protected const string FILE_PARAMETER_NAME = "file";
        protected const string VERSION_PARAMETER_NAME = "version";

        private readonly string _protectedResourceUrl;

        protected string ProtectedResourceUrl
        {
            get { return _protectedResourceUrl; }
        }

        protected abstract string FileParameterValue { get; }

        protected abstract string VersionParameterValue { get; }

        protected abstract List<IRequestInputParameter> GetFileSpecificParameters();

        protected HattrickFileAccessor(string protectedResourceUrl)
        {
            _protectedResourceUrl = HttpUtility.UrlPathEncode(protectedResourceUrl);
        }

        public string GetHattrickFileAccessor()
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