namespace WhoScored.Migration
{
    public interface IRequestInputParameter
    {
        string GetRequestParameterQueryString();
        bool HasValue { get; }
    }
}
