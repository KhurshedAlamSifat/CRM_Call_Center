using Base.Core.Domain.Users;
using System;

namespace Base.Core.Domain.Users
{
    /// <summary>
    /// Represents a customer password
    /// </summary>
    public partial class UserPassword : BaseEntity
    {
        public UserPassword()
        {
            PasswordFormat = PasswordFormat.Clear;
        }

        public int UserId { get; set; }

        public string Password { get; set; }

        public int PasswordFormatId { get; set; }

        public string PasswordSalt { get; set; }

        public DateTime CreatedOnUtc { get; set; }
        /// <summary>
        /// Gets or sets the password format
        /// </summary>
        public PasswordFormat PasswordFormat
        {
            get => (PasswordFormat)PasswordFormatId;
            set => PasswordFormatId = (int)value;
        }
    }
}