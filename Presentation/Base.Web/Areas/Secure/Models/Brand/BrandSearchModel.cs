using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Base.Web.Areas.Secure.Models.Brand
{
    /// <summary>
    /// Represents a discount search model
    /// </summary>
    public partial class BrandSearchModel : BaseSearchModel
    {

        #region Properties
        public string BrandName { get; set; }

        #endregion
    }
}
