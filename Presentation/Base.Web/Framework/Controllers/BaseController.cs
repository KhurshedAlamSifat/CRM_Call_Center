using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Encodings.Web;
using Base.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace Base.Web.Framework.Controllers
{
    /// <summary>
    /// Base controller
    /// </summary>
    //[PublishModelEvents]
    //[SignOutFromExternalAuthentication]
    //[ValidatePassword]
    //[SaveIpAddress]
    //[SaveLastActivity]
    //[SaveLastVisitedPage]
    public abstract class BaseController : Controller
    {
        #region Notifications

        /// <summary>
        /// Error's JSON data
        /// </summary>
        /// <param name="error">Error text</param>
        /// <returns>Error's JSON data</returns>
        protected JsonResult ErrorJson(string error)
        {
            return Json(new
            {
                error = error
            });
        }

        /// <summary>
        /// Error's JSON data
        /// </summary>
        /// <param name="errors">Error messages</param>
        /// <returns>Error's JSON data</returns>
        protected JsonResult ErrorJson(object errors)
        {
            return Json(new
            {
                error = errors
            });
        }
        /// <summary>
        /// Display "Edit" (manage) link (in public store)
        /// </summary>
        /// <param name="editPageUrl">Edit page URL</param>
        protected virtual void DisplayEditLink(string editPageUrl)
        {
            //var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();

            //pageHeadBuilder.AddEditPageUrl(editPageUrl);
        }

        #endregion

        
        #region Security

        /// <summary>
        /// Access denied view
        /// </summary>
        /// <returns>Access denied view</returns>
        protected virtual IActionResult AccessDeniedView()
        {
            //var webHelper = EngineContext.Current.Resolve<IWebHelper>();
            //return RedirectToAction("AccessDenied", "Security", new { pageUrl = webHelper.GetRawUrl(Request) });

            return null;
        }

        /// <summary>
        /// Access denied JSON data for DataTables
        /// </summary>
        /// <returns>Access denied JSON data</returns>
        protected JsonResult AccessDeniedDataTablesJson()
        {
            return ErrorJson("Admin.AccessDenied.Description");
        }

        #endregion

        #region Panels and tabs

        /// <summary>
        /// Save selected panel name
        /// </summary>
        /// <param name="panelName">Panel name to save</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request. Pass null to ignore</param>
        public virtual void SaveSelectedPanelName(string tabName, bool persistForTheNextRequest = true)
        {
            //keep this method synchronized with
            //"GetSelectedPanelName" method of \Nop.Web.Framework\Extensions\HtmlExtensions.cs
            if (string.IsNullOrEmpty(tabName))
                throw new ArgumentNullException(nameof(tabName));

            const string dataKey = "nop.selected-panel-name";
            if (persistForTheNextRequest)
            {
                TempData[dataKey] = tabName;
            }
            else
            {
                ViewData[dataKey] = tabName;
            }
        }

        /// <summary>
        /// Save selected tab name
        /// </summary>
        /// <param name="tabName">Tab name to save; empty to automatically detect it</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request. Pass null to ignore</param>
        public virtual void SaveSelectedTabName(string tabName = "", bool persistForTheNextRequest = true)
        {
            //default root tab
            SaveSelectedTabName(tabName, "selected-tab-name", null, persistForTheNextRequest);
            //child tabs (usually used for localization)
            //Form is available for POST only
            if (!Request.Method.Equals(WebRequestMethods.Http.Post, StringComparison.InvariantCultureIgnoreCase))
                return;

            foreach (var key in Request.Form.Keys)
                if (key.StartsWith("selected-tab-name-", StringComparison.InvariantCultureIgnoreCase))
                    SaveSelectedTabName(null, key, key.Substring("selected-tab-name-".Length), persistForTheNextRequest);
        }

        /// <summary>
        /// Save selected tab name
        /// </summary>
        /// <param name="tabName">Tab name to save; empty to automatically detect it</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request. Pass null to ignore</param>
        /// <param name="formKey">Form key where selected tab name is stored</param>
        /// <param name="dataKeyPrefix">A prefix for child tab to process</param>
        protected virtual void SaveSelectedTabName(string tabName, string formKey, string dataKeyPrefix, bool persistForTheNextRequest)
        {
            //keep this method synchronized with
            //"GetSelectedTabName" method of \Nop.Web.Framework\Extensions\HtmlExtensions.cs
            if (string.IsNullOrEmpty(tabName))
            {
                tabName = Request.Form[formKey];
            }

            if (string.IsNullOrEmpty(tabName))
                return;

            var dataKey = "nop.selected-tab-name";
            if (!string.IsNullOrEmpty(dataKeyPrefix))
                dataKey += $"-{dataKeyPrefix}";

            if (persistForTheNextRequest)
            {
                TempData[dataKey] = tabName;
            }
            else
            {
                ViewData[dataKey] = tabName;
            }
        }

        #endregion

    }
}