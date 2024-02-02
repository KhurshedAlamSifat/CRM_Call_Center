using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Base.Web.Areas.Secure.Models.BusinessUnit 
{
    /// <summary>
    /// Represents a discount search model
    /// </summary>
    public partial class BusinessUnitSearchModel : BaseSearchModel
    {
        #region Ctor


        #endregion

        #region Properties


        public string BusinessUnitName { get; set; } 

        #endregion
    }
}