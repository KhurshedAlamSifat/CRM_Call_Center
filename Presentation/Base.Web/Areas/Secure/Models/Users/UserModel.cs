using Base.Framework.Models;

namespace Base.Web.Areas.Secure.Models.Users
{
    public class UserModel : BaseCrmEntityModel
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Company { get; set; }
        public string HomeAddress { get; set; }
        public string Phone { get; set; }
    }
}
