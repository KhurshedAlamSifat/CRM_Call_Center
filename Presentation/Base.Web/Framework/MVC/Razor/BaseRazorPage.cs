using Base.Web.Framework.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Base.Web.Framework.MVC.Razor
{
    public abstract class BaseRazorPage<TModel> : Microsoft.AspNetCore.Mvc.Razor.RazorPage<TModel>
    {
        private Localizer _localizer;
        

    }

    /// <summary>
    /// Web view page
    /// </summary>
    public abstract class BaseRazorPage : BaseRazorPage<dynamic>
    {
    }
}
