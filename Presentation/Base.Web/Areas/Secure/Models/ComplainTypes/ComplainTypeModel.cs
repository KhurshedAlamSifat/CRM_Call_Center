using Base.Framework.Models;

namespace Base.Web.Areas.Secure.Models.ComplainTypes
{
    public class ComplainTypeModel : BaseCrmEntityModel
    {
        public int Id { get; set; }
        public string ComplainTypeName { get; set; }
    }
}
