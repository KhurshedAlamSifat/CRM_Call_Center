using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;


namespace Base.Web.Framework.Extensions
{
    /// <summary>
    /// HTML extensions
    /// </summary>
    public static class HtmlExtensions
    {
        #region Admin area extensions

        /// <summary>
        /// Gets a selected panel name (used in admin area to store selected panel name)
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        /// <returns>Name</returns>
        public static string GetSelectedPanelName(this IHtmlHelper helper)
        {
            //keep this method synchronized with
            //"SaveSelectedPanelName" method of \Area\Admin\Controllers\BaseAdminController.cs
            var tabName = string.Empty;
            const string dataKey = "farmamap.selected-panel-name";

            if (helper.ViewData.ContainsKey(dataKey))
                tabName = helper.ViewData[dataKey].ToString();

            if (helper.ViewContext.TempData.ContainsKey(dataKey))
                tabName = helper.ViewContext.TempData[dataKey].ToString();

            return tabName;
        }

        /// <summary>
        /// Gets a selected tab name (used in admin area to store selected tab name)
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        /// <param name="dataKeyPrefix">Key prefix. Pass null to ignore</param>
        /// <returns>Name</returns>
        public static string GetSelectedTabName(this IHtmlHelper helper, string dataKeyPrefix = null)
        {
            //keep this method synchronized with
            //"SaveSelectedTab" method of \Area\Admin\Controllers\BaseAdminController.cs
            var tabName = string.Empty;
            var dataKey = "nop.selected-tab-name";
            if (!string.IsNullOrEmpty(dataKeyPrefix))
                dataKey += $"-{dataKeyPrefix}";

            if (helper.ViewData.ContainsKey(dataKey))
                tabName = helper.ViewData[dataKey].ToString();

            if (helper.ViewContext.TempData.ContainsKey(dataKey))
                tabName = helper.ViewContext.TempData[dataKey].ToString();

            return tabName;
        }

        #region Form fields

        /// <summary>
        /// Generate hint control
        /// </summary>
        /// <param name="helper">HTML helper</param>
        /// <param name="value">TexHint text</param>
        /// <returns>Result</returns>
        public static IHtmlContent Hint(this IHtmlHelper helper, string value)
        {
            //create tag builder
            var builder = new TagBuilder("div");
            builder.MergeAttribute("title", value);
            builder.MergeAttribute("class", "ico-help");
            builder.MergeAttribute("data-toggle", "tooltip");
            var icon = new StringBuilder();
            icon.Append("<i class='fa fa-question-circle'></i>");
            builder.InnerHtml.AppendHtml(icon.ToString());
            //render tag
            return new HtmlString(builder.ToHtmlString());
        }

        #endregion

        #endregion

        #region Common extensions

        /// <summary>
        /// Convert IHtmlContent to string
        /// </summary>
        /// <param name="htmlContent">HTML content</param>
        /// <returns>Result</returns>
        public static string RenderHtmlContent(this IHtmlContent htmlContent)
        {
            using var writer = new StringWriter();
            htmlContent.WriteTo(writer, HtmlEncoder.Default);
            var htmlOutput = writer.ToString();
            return htmlOutput;
        }

        /// <summary>
        /// Convert IHtmlContent to string
        /// </summary>
        /// <param name="tag">Tag</param>
        /// <returns>String</returns>
        public static string ToHtmlString(this IHtmlContent tag)
        {
            using var writer = new StringWriter();
            tag.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }

        #endregion
    }
}