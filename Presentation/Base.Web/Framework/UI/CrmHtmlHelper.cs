using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Base.Core;
using Base.Core.Configuration;

namespace Base.Web.Framework.UI
{
    /// <summary>
    /// Represents the HTML helper implementation
    /// </summary>
    public partial class CrmHtmlHelper : ICrmHtmlHelper
    {
        #region Fields

        private readonly HtmlEncoder _htmlEncoder;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IWebHostEnvironment _webHostEnvironment;
        
        private readonly Dictionary<ResourceLocation, List<ScriptReferenceMeta>> _scriptParts = new();
        private readonly Dictionary<ResourceLocation, List<string>> _inlineScriptParts = new();
        private readonly List<CssReferenceMeta> _cssParts = new();

        private readonly List<string> _canonicalUrlParts = new();
        private readonly List<string> _headCustomParts = new();
        private readonly List<string> _metaDescriptionParts = new();
        private readonly List<string> _metaKeywordParts = new();
        private readonly List<string> _pageCssClassParts = new();
        private readonly List<string> _titleParts = new();

        private string _activeAdminMenuSystemName;
        private string _editPageUrl;

        #endregion

        #region Ctor

        public CrmHtmlHelper(
            IActionContextAccessor actionContextAccessor,
            HtmlEncoder htmlEncoder,
            IUrlHelperFactory urlHelperFactory,
            IWebHostEnvironment webHostEnvironment)
        {
            _actionContextAccessor = actionContextAccessor;
            _htmlEncoder = htmlEncoder;
            _urlHelperFactory = urlHelperFactory;
            _webHostEnvironment = webHostEnvironment;
        }

        #endregion


        #region Methods

        /// <summary>
        /// Add title element to the <![CDATA[<head>]]>
        /// </summary>
        /// <param name="part">Title part</param>
        public virtual void AddTitleParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _titleParts.Add(part);
        }

        /// <summary>
        /// Append title element to the <![CDATA[<head>]]>
        /// </summary>
        /// <param name="part">Title part</param>
        public virtual void AppendTitleParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _titleParts.Insert(0, part);
        }
        /// <summary>
        /// Add script element
        /// </summary>
        /// <param name="location">A location of the script element</param>
        /// <param name="src">Script path (minified version)</param>
        /// <param name="debugSrc">Script path (full debug version). If empty, then minified version will be used</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this script from bundling</param>
        /// <param name="isAsync">A value indicating whether to add an attribute "async" or not for js files</param>
        public virtual void AddScriptParts(ResourceLocation location, string src, string debugSrc = "", bool excludeFromBundle = false, bool isAsync = false)
        {
            if (!_scriptParts.ContainsKey(location))
                _scriptParts.Add(location, new List<ScriptReferenceMeta>());

            if (string.IsNullOrEmpty(src))
                return;

            if (!string.IsNullOrEmpty(debugSrc) && _webHostEnvironment.IsDevelopment())
                src = debugSrc;

            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);

            _scriptParts[location].Add(new ScriptReferenceMeta
            {
                ExcludeFromBundle = excludeFromBundle,
                IsAsync = isAsync,
                IsLocal = urlHelper.IsLocalUrl(src),
                Src = urlHelper.Content(src)
            });
        }

        /// <summary>
        /// Append script element
        /// </summary>
        /// <param name="location">A location of the script element</param>
        /// <param name="src">Script path (minified version)</param>
        /// <param name="debugSrc">Script path (full debug version). If empty, then minified version will be used</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this script from bundling</param>
        /// <param name="isAsync">A value indicating whether to add an attribute "async" or not for js files</param>
        public virtual void AppendScriptParts(ResourceLocation location, string src, string debugSrc = "", bool excludeFromBundle = false, bool isAsync = false)
        {
            if (!_scriptParts.ContainsKey(location))
                _scriptParts.Add(location, new List<ScriptReferenceMeta>());

            if (string.IsNullOrEmpty(src))
                return;

            if (!string.IsNullOrEmpty(debugSrc) && _webHostEnvironment.IsDevelopment())
                src = debugSrc;

            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);

            _scriptParts[location].Insert(0, new ScriptReferenceMeta
            {
                ExcludeFromBundle = excludeFromBundle,
                IsAsync = isAsync,
                IsLocal = urlHelper.IsLocalUrl(src),
                Src = urlHelper.Content(src)
            });
        }

        
        /// <summary>
        /// Add canonical URL element to the <![CDATA[<head>]]>
        /// </summary>
        /// <param name="part">Canonical URL part</param>
        /// <param name="withQueryString">Whether to use canonical URLs with query string parameters</param>
        public virtual void AddCanonicalUrlParts(string part, bool withQueryString = false)
        {
            if (string.IsNullOrEmpty(part))
                return;

            if (withQueryString)
            {
                //add ordered query string parameters
                var queryParameters = _actionContextAccessor.ActionContext.HttpContext.Request.Query.OrderBy(parameter => parameter.Key)
                    .ToDictionary(parameter => parameter.Key, parameter => parameter.Value.ToString());
                part = QueryHelpers.AddQueryString(part, queryParameters);
            }

            _canonicalUrlParts.Add(part);
        }

        /// <summary>
        /// Append canonical URL element to the <![CDATA[<head>]]>
        /// </summary>
        /// <param name="part">Canonical URL part</param>
        public virtual void AppendCanonicalUrlParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _canonicalUrlParts.Insert(0, part);
        }

        /// <summary>
        /// Generate all canonical URL parts
        /// </summary>
        /// <returns>Generated HTML string</returns>
        public virtual IHtmlContent GenerateCanonicalUrls()
        {
            var result = new StringBuilder();
            foreach (var canonicalUrl in _canonicalUrlParts)
            {
                result.AppendFormat("<link rel=\"canonical\" href=\"{0}\" />", canonicalUrl);
                result.Append(Environment.NewLine);
            }
            return new HtmlString(result.ToString());
        }

        /// <summary>
        /// Add any custom element to the <![CDATA[<head>]]> element
        /// </summary>
        /// <param name="part">The entire element. For example, <![CDATA[<meta name="msvalidate.01" content="123121231231313123123" />]]></param>
        public virtual void AddHeadCustomParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _headCustomParts.Add(part);
        }

        /// <summary>
        /// Append any custom element to the <![CDATA[<head>]]> element
        /// </summary>
        /// <param name="part">The entire element. For example, <![CDATA[<meta name="msvalidate.01" content="123121231231313123123" />]]></param>
        public virtual void AppendHeadCustomParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _headCustomParts.Insert(0, part);
        }

        /// <summary>
        /// Generate all custom elements
        /// </summary>
        /// <returns>Generated HTML string</returns>
        public virtual IHtmlContent GenerateHeadCustom()
        {
            //use only distinct rows
            var distinctParts = _headCustomParts.Distinct().ToList();
            if (!distinctParts.Any())
                return HtmlString.Empty;

            var result = new StringBuilder();
            foreach (var path in distinctParts)
            {
                result.Append(path);
                result.Append(Environment.NewLine);
            }
            return new HtmlString(result.ToString());
        }

        /// <summary>
        /// Add CSS class to the <![CDATA[<head>]]> element
        /// </summary>
        /// <param name="part">CSS class</param>
        public virtual void AddPageCssClassParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _pageCssClassParts.Add(part);
        }

        /// <summary>
        /// Append CSS class to the <![CDATA[<head>]]> element
        /// </summary>
        /// <param name="part">CSS class</param>
        public virtual void AppendPageCssClassParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _pageCssClassParts.Insert(0, part);
        }

        /// <summary>
        /// Generate all title parts
        /// </summary>
        /// <param name="part">CSS class</param>
        /// <returns>Generated string</returns>
        public virtual string GeneratePageCssClasses(string part = "")
        {
            AppendPageCssClassParts(part);

            var result = string.Join(" ", _pageCssClassParts.AsEnumerable().Reverse().ToArray());

            if (string.IsNullOrEmpty(result))
                return string.Empty;

            return _htmlEncoder.Encode(result);
        }

        /// <summary>
        /// Specify "edit page" URL
        /// </summary>
        /// <param name="url">URL</param>
        public virtual void AddEditPageUrl(string url)
        {
            _editPageUrl = url;
        }

        /// <summary>
        /// Get "edit page" URL
        /// </summary>
        /// <returns>URL</returns>
        public virtual string GetEditPageUrl()
        {
            return _editPageUrl;
        }

        /// <summary>
        /// Specify system name of admin menu item that should be selected (expanded)
        /// </summary>
        /// <param name="systemName">System name</param>
        public virtual void SetActiveMenuItemSystemName(string systemName)
        {
            _activeAdminMenuSystemName = systemName;
        }

        /// <summary>
        /// Get system name of admin menu item that should be selected (expanded)
        /// </summary>
        /// <returns>System name</returns>
        public virtual string GetActiveMenuItemSystemName()
        {
            return _activeAdminMenuSystemName;
        }

        #endregion

        #region Nested classes

        /// <summary>
        /// JS file meta data
        /// </summary>
        private record ScriptReferenceMeta
        {
            /// <summary>
            /// A value indicating whether to exclude the script from bundling
            /// </summary>
            public bool ExcludeFromBundle { get; set; }

            /// <summary>
            /// A value indicating whether to load the script asynchronously
            /// </summary>
            public bool IsAsync { get; set; }

            /// <summary>
            /// A value indicating whether the Src is local
            /// </summary>
            public bool IsLocal { get; set; }

            /// <summary>
            /// Src for production
            /// </summary>
            public string Src { get; set; }
        }

        /// <summary>
        /// CSS file meta data
        /// </summary>
        private record CssReferenceMeta
        {
            /// <summary>
            /// A value indicating whether to exclude the script from bundling
            /// </summary>
            public bool ExcludeFromBundle { get; set; }

            /// <summary>
            /// Src for production
            /// </summary>
            public string Src { get; set; }

            /// <summary>
            /// A value indicating whether the Src is local
            /// </summary>
            public bool IsLocal { get; set; }

        }

        #endregion
    }
}