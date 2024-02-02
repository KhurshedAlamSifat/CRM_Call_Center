using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Base.Web.Framework.Localization
{
    public class LocalizedString : HtmlString
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="localized">Valor de Localized</param>
        public LocalizedString(string localized) : base(localized)
        {
            Text = localized;
        }

        /// <summary>
        /// Text
        /// </summary>
        public string Text { get; }
    }
}
