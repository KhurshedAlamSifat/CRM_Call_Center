using System;


namespace Base.Core.Domain.Users
{
    public partial class User : BaseEntity
    {
        public User()
        {
            UserGuid = Guid.NewGuid();
        }

        public Guid UserGuid { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Company { get; set; }
        public string HomeAddress { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}