using Base.Core.Infrastructure;

namespace Base.Services.Common
{
    public static class RegexPattern
    {
        public static string Phone = @"^(?:\+880|880|00880|0)?(1)([3-9])(\d{8})$";
        public static string Email = @"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,17})$";
        public static string Integer = @"[0-9]$";
        public static string IntergerSevenDigit = @"^[0-9]{7}$";
    }
}
