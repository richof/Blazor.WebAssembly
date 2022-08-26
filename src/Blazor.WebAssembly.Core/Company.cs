using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.WebAssembly.Core
{
    public class Company
    {
        public Guid Id { get; set; }
        public string TradeName { get; set; }
        public byte[] Logo { get; set; }
        public virtual List<Address> Addresses { get; set; }
        public Company()
        {
            Id = Guid.NewGuid();
            Addresses = new List<Address>();
        }
        public void AddAddress(Address address)
        {
            Addresses.Add(address);
        }
        
    }
}
