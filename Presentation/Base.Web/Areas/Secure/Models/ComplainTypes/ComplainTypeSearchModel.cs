using Base.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Base.Web.Areas.Secure.Models.ComplainTypes
{
    public class ComplainTypeSearchModel : BaseSearchModel
    {

        #region Properties


        public string SearchComplainTypeName { get; set; }

        #endregion
    }
}
