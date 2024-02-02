using System;
using System.Text.RegularExpressions;

namespace Base.Services.Common
{
    public static class CommonFormatter
    {
        public static bool IsValidPhoneNumber(this string phoneNumber)
        {
            try
            {
                return Regex.Match(phoneNumber, RegexPattern.Phone).Success;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidEmail(this string email)
        {
            try
            {
                return Regex.Match(email, RegexPattern.Email).Success;
            }
            catch
            {
                return false;
            }
        }

        public static string ToFormattedPhoneNumber(this string phone)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(phone) && Regex.Match(phone, RegexPattern.Phone).Success)
                {
                    switch (phone.Length)
                    {
                        case 10:
                            return "+880" + phone;
                        case 11:
                            return "+88" + phone;
                        case 13:
                            return "+" + phone;
                        case 15:
                            return "+" + phone.Substring(2, 13);
                        case 14:
                            return phone;
                        default:
                            return "";
                    }
                }
                return "";
            }
            catch
            {
                return "";
            }
        }
    }
}
