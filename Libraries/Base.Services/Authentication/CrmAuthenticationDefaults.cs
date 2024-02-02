using Microsoft.AspNetCore.Http;

namespace Base.Services.Authentication
{
    /// <summary>
    /// Represents default values related to authentication services
    /// </summary>
    public static partial class CrmAuthenticationDefaults
    {
        public static string AuthenticationScheme => "Authentication";
        public static string ReturnUrlParameter => string.Empty;
        public static string ClaimsIssuer => "crm";

    }
}