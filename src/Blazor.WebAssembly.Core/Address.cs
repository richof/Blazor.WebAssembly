namespace Blazor.WebAssembly.Core
{
    public class Address
    {
        public Guid Id { get; set; }
        public string StreetName { get; set; }
        public string Number { get; set; }
        public Guid CompanyId { get; set; }
        //
        public Company Company { get; set; }
        public Address()
        {
            Id = Guid.NewGuid();
        }
    }
}
