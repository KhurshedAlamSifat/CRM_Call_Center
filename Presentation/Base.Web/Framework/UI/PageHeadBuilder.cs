﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Hosting;
using Base.Core;

namespace Base.Web.Framework.UI
{
    /// <summary>
    /// Page head builder
    /// </summary>
    public partial class PageHeadBuilder : IPageHeadBuilder
    {
        #region Fields

        private static readonly object _lock = new object();
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly List<string> _titleParts;
        private readonly List<string> _metaDescriptionParts;
        private readonly List<string> _metaKeywordParts;
        private readonly Dictionary<ResourceLocation, List<ScriptReferenceMeta>> _scriptParts;
        private readonly Dictionary<ResourceLocation, List<string>> _inlineScriptParts;
        private readonly Dictionary<ResourceLocation, List<CssReferenceMeta>> _cssParts;
        private readonly List<string> _canonicalUrlParts;
        private readonly List<string> _headCustomParts;
        private readonly List<string> _pageCssClassParts;
        private string _activeAdminMenuSystemName;
        private string _editPageUrl;

        #endregion

        #region Ctor

        public PageHeadBuilder(
            IActionContextAccessor actionContextAccessor,
            IWebHostEnvironment webHostEnvironment,
            IUrlHelperFactory urlHelperFactory
            )
        {
            _actionContextAccessor = actionContextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _urlHelperFactory = urlHelperFactory;
            _titleParts = new List<string>();
            _metaDescriptionParts = new List<string>();
            _metaKeywordParts = new List<string>();
            _scriptParts = new Dictionary<ResourceLocation, List<ScriptReferenceMeta>>();
            _inlineScriptParts = new Dictionary<ResourceLocation, List<string>>();
            _cssParts = new Dictionary<ResourceLocation, List<CssReferenceMeta>>();
            _canonicalUrlParts = new List<string>();
            _headCustomParts = new List<string>();
            _pageCssClassParts = new List<string>();
        }

        #endregion

        #region Methods
        

        /// <summary>
        /// Add script element
        /// </summary>
        /// <param name="location">A location of the script element</param>
        /// <param name="src">Script path (minified version)</param>
        /// <param name="debugSrc">Script path (full debug version). If empty, then minified version will be used</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this script from bundling</param>
        /// <param name="isAsync">A value indicating whether to add an attribute "async" or not for js files</param>
        public virtual void AddScriptParts(ResourceLocation location, string src, string debugSrc, bool excludeFromBundle, bool isAsync)
        {
            if (!_scriptParts.ContainsKey(location))
                _scriptParts.Add(location, new List<ScriptReferenceMeta>());

            if (string.IsNullOrEmpty(src))
                return;

            if (string.IsNullOrEmpty(debugSrc))
                debugSrc = src;

            _scriptParts[location].Add(new ScriptReferenceMeta
            {
                ExcludeFromBundle = excludeFromBundle,
                IsAsync = isAsync,
                Src = src,
                DebugSrc = debugSrc
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
        public virtual void AppendScriptParts(ResourceLocation location, string src, string debugSrc, bool excludeFromBundle, bool isAsync)
        {
            if (!_scriptParts.ContainsKey(location))
                _scriptParts.Add(location, new List<ScriptReferenceMeta>());

            if (string.IsNullOrEmpty(src))
                return;

            if (string.IsNullOrEmpty(debugSrc))
                debugSrc = src;

            _scriptParts[location].Insert(0, new ScriptReferenceMeta
            {
                ExcludeFromBundle = excludeFromBundle,
                IsAsync = isAsync,
                Src = src,
                DebugSrc = debugSrc
            });
        }
        /// <summary>
        /// Generate all script parts
        /// </summary>
        /// <param name="location">A location of the script element</param>
        /// <param name="bundleFiles">A value indicating whether to bundle script elements</param>
        /// <returns>Generated string</returns>
        public virtual string GenerateScripts(ResourceLocation location)
        {
            if (!_scriptParts.ContainsKey(location) || _scriptParts[location] == null)
                return "";

            if (!_scriptParts.Any())
                return "";

            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);

            var debugModel = _webHostEnvironment.IsDevelopment();
            //bundling is disabled
            var result = new StringBuilder();
            foreach (var item in _scriptParts[location].Distinct())
            {
                var src = debugModel ? item.DebugSrc : item.Src;
                result.AppendFormat("<script {1}src=\"{0}\"></script>", urlHelper.Content(src), item.IsAsync ? "async " : "");
                result.Append(Environment.NewLine);
            }
            return result.ToString();

        }

        /// <summary>
        /// Add inline script element
        /// </summary>
        /// <param name="location">A location of the script element</param>
        /// <param name="script">Script</param>
        public virtual void AddInlineScriptParts(ResourceLocation location, string script)
        {
            if (!_inlineScriptParts.ContainsKey(location))
                _inlineScriptParts.Add(location, new List<string>());

            if (string.IsNullOrEmpty(script))
                return;

            if (_inlineScriptParts[location].Contains(script))
                return;

            _inlineScriptParts[location].Add(script);
        }
        /// <summary>
        /// Append inline script element
        /// </summary>
        /// <param name="location">A location of the script element</param>
        /// <param name="script">Script</param>
        public virtual void AppendInlineScriptParts(ResourceLocation location, string script)
        {
            if (!_inlineScriptParts.ContainsKey(location))
                _inlineScriptParts.Add(location, new List<string>());

            if (string.IsNullOrEmpty(script))
                return;

            if (_inlineScriptParts[location].Contains(script))
                return;

            _inlineScriptParts[location].Insert(0, script);
        }
        /// <summary>
        /// Generate all inline script parts
        /// </summary>
        /// <param name="location">A location of the script element</param>
        /// <returns>Generated string</returns>
        public virtual string GenerateInlineScripts(ResourceLocation location)
        {
            if (!_inlineScriptParts.ContainsKey(location) || _inlineScriptParts[location] == null)
                return "";

            if (!_inlineScriptParts.Any())
                return "";

            var result = new StringBuilder();
            foreach (var item in _inlineScriptParts[location])
            {
                result.Append(item);
                result.Append(Environment.NewLine);
            }
            return result.ToString();
        }

        /// <summary>
        /// Add CSS element
        /// </summary>
        /// <param name="location">A location of the script element</param>
        /// <param name="src">Script path (minified version)</param>
        /// <param name="debugSrc">Script path (full debug version). If empty, then minified version will be used</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this script from bundling</param>
        public virtual void AddCssFileParts(ResourceLocation location, string src, string debugSrc, bool excludeFromBundle = false)
        {
            if (!_cssParts.ContainsKey(location))
                _cssParts.Add(location, new List<CssReferenceMeta>());

            if (string.IsNullOrEmpty(src))
                return;

            if (string.IsNullOrEmpty(debugSrc))
                debugSrc = src;

            _cssParts[location].Add(new CssReferenceMeta
            {
                ExcludeFromBundle = excludeFromBundle,
                Src = src,
                DebugSrc = debugSrc
            });
        }
        /// <summary>
        /// Append CSS element
        /// </summary>
        /// <param name="location">A location of the script element</param>
        /// <param name="src">Script path (minified version)</param>
        /// <param name="debugSrc">Script path (full debug version). If empty, then minified version will be used</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this script from bundling</param>
        public virtual void AppendCssFileParts(ResourceLocation location, string src, string debugSrc, bool excludeFromBundle = false)
        {
            if (!_cssParts.ContainsKey(location))
                _cssParts.Add(location, new List<CssReferenceMeta>());

            if (string.IsNullOrEmpty(src))
                return;

            if (string.IsNullOrEmpty(debugSrc))
                debugSrc = src;

            _cssParts[location].Insert(0, new CssReferenceMeta
            {
                ExcludeFromBundle = excludeFromBundle,
                Src = src,
                DebugSrc = debugSrc
            });
        }
        /// <summary>
        /// Generate all CSS parts
        /// </summary>
        /// <param name="location">A location of the script element</param>
        /// <param name="bundleFiles">A value indicating whether to bundle script elements</param>
        /// <returns>Generated string</returns>
        public virtual string GenerateCssFiles(ResourceLocation location)
        {
            if (!_cssParts.ContainsKey(location) || _cssParts[location] == null)
                return "";

            if (!_cssParts.Any())
                return "";

            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);

            var debugModel = _webHostEnvironment.IsDevelopment();


            //bundling is disabled
            var result = new StringBuilder();
            foreach (var item in _cssParts[location].Distinct())
            {
                var src = debugModel ? item.DebugSrc : item.Src;
                result.AppendFormat("<link href=\"{0}\" rel=\"stylesheet\" type=\"{1}\" />", urlHelper.Content(src), "text/css");
                result.AppendLine();
            }
            return result.ToString();

        }

        /// <summary>
        /// Add canonical URL element to the <![CDATA[<head>]]>
        /// </summary>
        /// <param name="part">Canonical URL part</param>
        public virtual void AddCanonicalUrlParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

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
        /// <returns>Generated string</returns>
        public virtual string GenerateCanonicalUrls()
        {
            var result = new StringBuilder();
            foreach (var canonicalUrl in _canonicalUrlParts)
            {
                result.AppendFormat("<link rel=\"canonical\" href=\"{0}\" />", canonicalUrl);
                result.Append(Environment.NewLine);
            }
            return result.ToString();
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
        /// <returns>Generated string</returns>
        public virtual string GenerateHeadCustom()
        {
            //use only distinct rows
            var distinctParts = _headCustomParts.Distinct().ToList();
            if (!distinctParts.Any())
                return "";

            var result = new StringBuilder();
            foreach (var path in distinctParts)
            {
                result.Append(path);
                result.Append(Environment.NewLine);
            }
            return result.ToString();
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
        /// <returns>Generated string</returns>
        public virtual string GeneratePageCssClasses()
        {
            var result = string.Join(" ", _pageCssClassParts.AsEnumerable().Reverse().ToArray());
            return result;
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
        private class ScriptReferenceMeta : IEquatable<ScriptReferenceMeta>
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
            /// Src for production
            /// </summary>
            public string Src { get; set; }

            /// <summary>
            /// Src for debugging
            /// </summary>
            public string DebugSrc { get; set; }

            /// <summary>
            /// Equals
            /// </summary>
            /// <param name="item">Other item</param>
            /// <returns>Result</returns>
            public bool Equals(ScriptReferenceMeta item)
            {
                if (item == null)
                    return false;
                return Src.Equals(item.Src) && DebugSrc.Equals(item.DebugSrc);
            }
            /// <summary>
            /// Get hash code
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return Src == null ? 0 : Src.GetHashCode();
            }
        }

        /// <summary>
        /// CSS file meta data
        /// </summary>
        private class CssReferenceMeta : IEquatable<CssReferenceMeta>
        {
            public bool ExcludeFromBundle { get; set; }

            /// <summary>
            /// Src for production
            /// </summary>
            public string Src { get; set; }

            /// <summary>
            /// Src for debugging
            /// </summary>
            public string DebugSrc { get; set; }

            /// <summary>
            /// Equals
            /// </summary>
            /// <param name="item">Other item</param>
            /// <returns>Result</returns>
            public bool Equals(CssReferenceMeta item)
            {
                if (item == null)
                    return false;
                return Src.Equals(item.Src) && DebugSrc.Equals(item.DebugSrc);
            }
            /// <summary>
            /// Get hash code
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return Src == null ? 0 : Src.GetHashCode();
            }
        }

        #endregion
    }
}