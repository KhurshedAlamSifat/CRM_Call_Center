using Base.Core.Domain.Users;

namespace Base.Services.Users
{
    /// <summary>
    /// Customer registration request
    /// </summary>
    public partial class UserRegistrationRequest
    {
        public UserRegistrationRequest(User user, string email,
            string password, string name,
            string phone, string address)
        {
            User = user;
            Email = email;
            Password = password;
            Name = name;
            Phone = phone;
            Address = address;
        }

        public User User { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
