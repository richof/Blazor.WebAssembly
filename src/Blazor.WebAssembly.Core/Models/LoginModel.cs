using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.WebAssembly.Core.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "The {0} field is required")]

        public string Email { get; set; }
        [Required(ErrorMessage = "The {0} field is required")]
      

        public string Password { get; set; }
    }
}
