using Base.Web.Framework.Models;

namespace Base.Web.Areas.Secure.Models.Technicians
{
    public class TechnicianSearchModel : BaseSearchModel
    {
        public string SearchTechnicianName { get; set; }
        public int SearchTechinicianbyThana { get; set; }
        public int SearchTechnicianbyServiceCenter { get; set; }
        public int SearchTechnicianbyCategory { get; set; }

    }
}
