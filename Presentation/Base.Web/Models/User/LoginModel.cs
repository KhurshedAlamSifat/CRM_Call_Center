using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Base.Web.Models.User
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Phone is required")]
        public string Phone { get; set; }
        public string Identificacion { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Display(Name = "Recordar esta sesion")]
        public bool RememberMe { get; set; }

        public string ErrorMessage { get; set; }

    }
}
