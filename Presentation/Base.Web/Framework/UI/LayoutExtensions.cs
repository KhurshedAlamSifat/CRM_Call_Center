﻿using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Base.Core.Infrastructure;

namespace Base.Web.Framework.UI
{
    /// <summary>
    /// Layout extensions
    /// </summary>
    public static class LayoutExtensions
    {
        /// <summary>
        /// Add script element
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="src">Script path (minified version)</param>
        /// <param name="debugSrc">Script path (full debug version). If empty, then minified version will be used</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this script from bundling</param>
        /// <param name="isAsync">A value indicating whether to add an attribute "async" or not for js files</param>
        public static void AddScriptParts(this IHtmlHelper html, string src, string debugSrc = "",
            bool excludeFromBundle = false, bool isAsync = false)
        {
            AddScriptParts(html, ResourceLocation.Head, src, debugSrc, excludeFromBundle, isAsync);
        }
        /// <summary>
        /// Add script element
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="location">A location of the script element</param>
        /// <param name="src">Script path (minified version)</param>
        /// <param name="debugSrc">Script path (full debug version). If empty, then minified version will be used</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this script from bundling</param>
        /// <param name="isAsync">A value indicating whether to add an attribute "async" or not for js files</param>
        public static void AddScriptParts(this IHtmlHelper html, ResourceLocation location,
            string src, string debugSrc = "", bool excludeFromBundle = false, bool isAsync = false)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.AddScriptParts(location, src, debugSrc, excludeFromBundle, isAsync);
        }
        /// <summary>
        /// Append script element
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="src">Script path (minified version)</param>
        /// <param name="debugSrc">Script path (full debug version). If empty, then minified version will be used</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this script from bundling</param>
        /// <param name="isAsync">A value indicating whether to add an attribute "async" or not for js files</param>
        public static void AppendScriptParts(this IHtmlHelper html, string src, string debugSrc = "",
            bool excludeFromBundle = false, bool isAsync = false)
        {
            AppendScriptParts(html, ResourceLocation.Head, src, debugSrc, excludeFromBundle, isAsync);
        }
        /// <summary>
        /// Append script element
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="location">A location of the script element</param>
        /// <param name="src">Script path (minified version)</param>
        /// <param name="debugSrc">Script path (full debug version). If empty, then minified version will be used</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this script from bundling</param>
        /// <param name="isAsync">A value indicating whether to add an attribute "async" or not for js files</param>
        public static void AppendScriptParts(this IHtmlHelper html, ResourceLocation location,
            string src, string debugSrc = "", bool excludeFromBundle = false, bool isAsync = false)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.AppendScriptParts(location, src, debugSrc, excludeFromBundle, isAsync);
        }
        /// <summary>
        /// Generate all script parts
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="location">A location of the script element</param>
        /// <param name="bundleFiles">A value indicating whether to bundle script elements</param>
        /// <returns>Generated string</returns>
        public static IHtmlContent NopScripts(this IHtmlHelper html, ResourceLocation location)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            return new HtmlString(pageHeadBuilder.GenerateScripts(location));
        }





        /// <summary>
        /// Add inline script element
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="location">A location of the script element</param>
        /// <param name="script">Script</param>
        public static void AddInlineScriptParts(this IHtmlHelper html, ResourceLocation location, string script)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.AddInlineScriptParts(location, script);
        }
        /// <summary>
        /// Append inline script element
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="location">A location of the script element</param>
        /// <param name="script">Script</param>
        public static void AppendInlineScriptParts(this IHtmlHelper html, ResourceLocation location, string script)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.AppendInlineScriptParts(location, script);
        }
        /// <summary>
        /// Generate all inline script parts
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="location">A location of the script element</param>
        /// <returns>Generated string</returns>
        public static IHtmlContent NopInlineScripts(this IHtmlHelper html, ResourceLocation location)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            return new HtmlString(pageHeadBuilder.GenerateInlineScripts(location));
        }

        /// <summary>
        /// Add CSS element
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="src">Script path (minified version)</param>
        /// <param name="debugSrc">Script path (full debug version). If empty, then minified version will be used</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this script from bundling</param>
        public static void AddCssFileParts(this IHtmlHelper html, string src, string debugSrc = "", bool excludeFromBundle = false)
        {
            AddCssFileParts(html, ResourceLocation.Head, src, debugSrc, excludeFromBundle);
        }
        /// <summary>
        /// Add CSS element
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="location">A location of the script element</param>
        /// <param name="src">Script path (minified version)</param>
        /// <param name="debugSrc">Script path (full debug version). If empty, then minified version will be used</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this script from bundling</param>
        public static void AddCssFileParts(this IHtmlHelper html, ResourceLocation location, 
            string src, string debugSrc = "", bool excludeFromBundle = false)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.AddCssFileParts(location, src, debugSrc, excludeFromBundle);
        }
        /// <summary>
        /// Append CSS element
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="src">Script path (minified version)</param>
        /// <param name="debugSrc">Script path (full debug version). If empty, then minified version will be used</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this script from bundling</param>
        public static void AppendCssFileParts(this IHtmlHelper html, string src, string debugSrc = "", bool excludeFromBundle = false)
        {
            AppendCssFileParts(html, ResourceLocation.Head, src, debugSrc, excludeFromBundle);
        }
        /// <summary>
        /// Append CSS element
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="location">A location of the script element</param>
        /// <param name="src">Script path (minified version)</param>
        /// <param name="debugSrc">Script path (full debug version). If empty, then minified version will be used</param>
        /// <param name="excludeFromBundle">A value indicating whether to exclude this script from bundling</param>
        public static void AppendCssFileParts(this IHtmlHelper html, ResourceLocation location, 
            string src, string debugSrc = "", bool excludeFromBundle = false)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.AppendCssFileParts(location, src, debugSrc, excludeFromBundle);
        }
        /// <summary>
        /// Generate all CSS parts
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="location">A location of the script element</param>
        /// <param name="bundleFiles">A value indicating whether to bundle script elements</param>
        /// <returns>Generated string</returns>
        public static IHtmlContent FarmaMapCssFiles(this IHtmlHelper html, ResourceLocation location)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            return new HtmlString(pageHeadBuilder.GenerateCssFiles(location));
        }

        /// <summary>
        /// Add canonical URL element to the <![CDATA[<head>]]>
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="part">Canonical URL part</param>
        /// <param name="withQueryString">Whether to use canonical URLs with query string parameters</param>
        public static void AddCanonicalUrlParts(this IHtmlHelper html, string part, bool withQueryString = false)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();

            if (withQueryString)
            {
                //add ordered query string parameters
                var queryParameters = html.ViewContext.HttpContext.Request.Query.OrderBy(parameter => parameter.Key)
                    .ToDictionary(parameter => parameter.Key, parameter => parameter.Value.ToString());
                part = QueryHelpers.AddQueryString(part, queryParameters);
            }

            pageHeadBuilder.AddCanonicalUrlParts(part);
        }
        /// <summary>
        /// Append canonical URL element to the <![CDATA[<head>]]>
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="part">Canonical URL part</param>
        public static void AppendCanonicalUrlParts(this IHtmlHelper html, string part)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.AppendCanonicalUrlParts(part);
        }
        /// <summary>
        /// Generate all canonical URL parts
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="part">Canonical URL part</param>
        /// <returns>Generated string</returns>
        public static IHtmlContent NopCanonicalUrls(this IHtmlHelper html, string part = "")
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            html.AppendCanonicalUrlParts(part);
            return new HtmlString(pageHeadBuilder.GenerateCanonicalUrls());
        }


        /// <summary>
        /// Add any custom element to the <![CDATA[<head>]]> element
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="part">The entire element. For example, <![CDATA[<meta name="msvalidate.01" content="123121231231313123123" />]]></param>
        public static void AddHeadCustomParts(this IHtmlHelper html, string part)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.AddHeadCustomParts(part);
        }
        /// <summary>
        /// Append any custom element to the <![CDATA[<head>]]> element
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="part">The entire element. For example, <![CDATA[<meta name="msvalidate.01" content="123121231231313123123" />]]></param>
        public static void AppendHeadCustomParts(this IHtmlHelper html, string part)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.AppendHeadCustomParts(part);
        }
        /// <summary>
        /// Generate all custom elements
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <returns>Generated string</returns>
        public static IHtmlContent NopHeadCustom(this IHtmlHelper html)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            return new HtmlString(pageHeadBuilder.GenerateHeadCustom());
        }


        /// <summary>
        /// Add CSS class to the <![CDATA[<head>]]> element
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="part">CSS class</param>
        public static void AddPageCssClassParts(this IHtmlHelper html, string part)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.AddPageCssClassParts(part);
        }
        /// <summary>
        /// Append CSS class to the <![CDATA[<head>]]> element
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="part">CSS class</param>
        public static void AppendPageCssClassParts(this IHtmlHelper html, string part)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.AppendPageCssClassParts(part);
        }
        /// <summary>
        /// Generate all title parts
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="part">CSS class</param>
        /// <param name="includeClassElement">A value indicating whether to include "class" attributes</param>
        /// <returns>Generated string</returns>
        public static IHtmlContent NopPageCssClasses(this IHtmlHelper html, string part = "", bool includeClassElement = true)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            html.AppendPageCssClassParts(part);
            var classes = pageHeadBuilder.GeneratePageCssClasses();

            if (string.IsNullOrEmpty(classes))
                return null;

            var result = includeClassElement ? $"class=\"{classes}\"" : classes;
            return new HtmlString(result);
        }


        /// <summary>
        /// Specify system name of admin menu item that should be selected (expanded)
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="systemName">System name</param>
        public static void SetActiveMenuItemSystemName(this IHtmlHelper html, string systemName)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.SetActiveMenuItemSystemName(systemName);
        }
        /// <summary>
        /// Get system name of admin menu item that should be selected (expanded)
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <returns>System name</returns>
        public static string GetActiveMenuItemSystemName(this IHtmlHelper html)
        {
            var pageHeadBuilder = EngineContext.Current.Resolve<IPageHeadBuilder>();
            return pageHeadBuilder.GetActiveMenuItemSystemName();
        }
    }
}