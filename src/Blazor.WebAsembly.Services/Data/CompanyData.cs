using Blazor.WebAssembly.Core;
using System.Linq;
using System.Net;

namespace Blazor.WebAsembly.Services.Data
{
    public class CompanyData : ICompanyData
    {
        private readonly IDataAccess _dataAccess;

        public CompanyData(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }
        public async Task<IEnumerable<Company>> GetAll()
        {
            string sql = "Select Id,TradeName, Logo from Companies";
            var companies = await _dataAccess.GetAll<Company, dynamic>(sql, new { });
            return companies;
        }
        public async Task<Company> AttachAddressesToCompany(Company company)
        {
            string sql = "Select Id,StreetName,Number,CompanyId from Addresses where CompanyId=@companyId";
            var addresses = await _dataAccess.GetAll<Address, dynamic>(sql, new { companyId = company.Id });
             addresses?.ForEach(a=>company.AddAddress(a));
            return company;
        }
        public async Task AddAddress(Address address)
        {

            string sql = "Insert into Addresses(Id,StreetName,Number,CompanyId) values(@Id,@StreetName,@Number,@CompanyId)";
            await _dataAccess.CreateUpdate(sql, address);
        }
        public async Task UpdateAddress(Guid id,Address address)
        {

            string sql = "Update Addresses set StreetName=@StreetName,Number=@Number Where Id=@Id";
            await _dataAccess.CreateUpdate(sql, new {Id=id,StreetName=address.StreetName,Number=address.Number });
        }
        public async Task CreateCompany(Company company)
        {
            string sql = "Insert Into Companies (Id,TradeName,Logo) Values(@Id,@TradeName,@Logo)";
            await _dataAccess.CreateUpdate(sql, company);
        }
        public async Task UpdateCompany(Guid id, Company company)
        {
            string sql = "Update Companies set TradeName=@TradeName,Logo=@Logo where Id=@Id";
            await _dataAccess.CreateUpdate(sql, new {Id=id,TradeName=company.TradeName,Logo=company.Logo });
        }
        public async Task DeleteAddress(Guid id)
        {
            string sql = "Delete from Addresses where Id=@Id";
            await _dataAccess.CreateUpdate(sql, new { Id = id});
        }
        public async Task DeleteCompany(Guid id)
        {
            string addressesSql = "Select * from Addresses where CompanyId=@Id";
            var addresses = await _dataAccess.GetAll<Address,dynamic>(addressesSql, new { Id = id });
            foreach (var address in addresses)
            {
                await DeleteAddress(address.Id);

            }
            string sql = "Delete from Companies where Id=@id";
            await _dataAccess.CreateUpdate(sql, new { Id = id });
        }

       
    }
}
