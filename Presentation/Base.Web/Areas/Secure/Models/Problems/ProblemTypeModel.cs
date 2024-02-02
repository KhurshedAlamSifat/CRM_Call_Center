using Base.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Base.Web.Areas.Secure.Models.Problems
{
    public class ProblemTypeModel : BaseCrmEntityModel
    {
        public ProblemTypeModel()
        {
            Categories = new List<SelectListItem>();
        }
        public string ProblemDescription { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<SelectListItem> Categories { get; set; }
    }
}
