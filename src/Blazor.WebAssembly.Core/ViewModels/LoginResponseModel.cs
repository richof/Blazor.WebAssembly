using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.WebAssembly.Core.ViewModels
{
    public class LoginResponseModel
    {
        public string Token{ get; set; }
        public double ExpiresIn { get; set; }
        public List<ClaimModel> Claims { get; set; } = new List<ClaimModel>();
    }
    public class ClaimModel
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
