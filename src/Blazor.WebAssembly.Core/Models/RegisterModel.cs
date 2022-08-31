using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.WebAssembly.Core.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "The {0} field is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The {0} field is required")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "The {0} field is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "The {0} field is required")]
        [Compare("Password", ErrorMessage ="{0} and {1} must be the same")]
        public string ConfirmPassword { get; set; }
    }
}
