using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Services.Authentication
{
    public static partial class AuthenticationDefaults
    {        
        public static string AuthenticationScheme => "Authentication";
        public static string ClaimsIssuer => "crmCallCentre";
        public static PathString LoginPath => new PathString("/login");        
        public static PathString LogoutPath => new PathString("/logout");        
        public static PathString AccessDeniedPath => new PathString("/page-not-found");        
        public static string ReturnUrlParameter => string.Empty;        
        public static string ExternalAuthenticationErrorsSessionKey => "base.externalauth.errors"; //NOMBRE DEL SISTEMA 
    }
}
