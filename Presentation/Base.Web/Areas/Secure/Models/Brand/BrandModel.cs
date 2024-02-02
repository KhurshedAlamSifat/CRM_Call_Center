using Base.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Base.Web.Areas.Secure.Models.Brand
{
    public class BrandModel : BaseCrmEntityModel
    {
        public BrandModel()
        {
            BusinessUnits = new List<SelectListItem>();
        }
        public string BrandName { get; set; }
        public string BusinessUnitName { get; set; }
        public int BusinessUnit_Id { get; set; }
        public List<SelectListItem> BusinessUnits { get; set; }

    }
}
