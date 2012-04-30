namespace WhoScored.CHPP.Files
{
    public interface IRequestInputParameter
    {
        string GetRequestParameterQueryString();
        bool HasValue { get; }
    }
}
