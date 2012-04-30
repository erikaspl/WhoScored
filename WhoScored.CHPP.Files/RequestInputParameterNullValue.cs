namespace WhoScored.CHPP.Files
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