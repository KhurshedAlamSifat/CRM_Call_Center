using Base.Web.Framework.Models;

namespace Base.Web.Areas.Secure.Models.Thanas
{
    public class ThanaSearchModel : BaseSearchModel
    {
        public string SearchThanaName { get; set; }
        public int SearchThanabyDistrict { get; set; }
    }
}
