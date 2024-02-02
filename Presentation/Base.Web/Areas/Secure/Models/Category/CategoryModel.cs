using Base.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Base.Web.Areas.Secure.Models.Category
{
    public class CategoryModel : BaseCrmEntityModel
    {
        public CategoryModel()
        {
            Brands = new List<SelectListItem>();
        }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public int Brand_Id { get; set; }
        public List<SelectListItem> Brands { get; set; }

    }
}
