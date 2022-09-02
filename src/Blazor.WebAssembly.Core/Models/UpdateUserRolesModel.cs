using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.WebAssembly.Core.Models
{
    public class UpdateUserRolesModel
    {
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
        public UpdateUserRolesModel()
        {
            Roles = new List<string>();
        }
        public void AddRole(string newRole)
        {
            Roles.Add(newRole);
        }
    }
}
