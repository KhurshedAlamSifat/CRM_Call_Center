using Microsoft.AspNetCore.Http;

namespace Base.Core
{
    public partial interface IWebHelper
    {
        string GetUrlReferrer();
        string GetCurrentIpAddress();
        bool IsCurrentConnectionSecured();
        
        T QueryString<T>(string name);
        void RestartAppDomain();
        bool IsRequestBeingRedirected { get; }
        string GetCurrentRequestProtocol();
        bool IsLocalRequest(HttpRequest req);
        string GetRawUrl(HttpRequest request);
        bool IsAjaxRequest(HttpRequest request);

        string GetThisPageUrl(bool includeQueryString, bool? useSsl = null, bool lowercaseUrl = false);

        string ModifyQueryString(string url, string key, params string[] values);
    }
}
