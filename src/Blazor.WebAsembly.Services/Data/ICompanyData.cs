using Blazor.WebAssembly.Core;

namespace Blazor.WebAsembly.Services.Data
{
    public interface ICompanyData
    {
        Task AddAddress(Address address);
        Task<Company> AttachAddressesToCompany(Company company);
        Task CreateCompany(Company company);
        Task DeleteAddress(Guid id);
        Task DeleteCompany(Guid id);
        Task<IEnumerable<Company>> GetAll();
        Task UpdateAddress(Guid id, Address address);
        Task UpdateCompany(Guid id, Company company);
    }
}