using AutoMapper;
using Blazor.WebAsembly.Services.Data;

using Blazor.WebAssembly.Core;
using Blazor.WebAssembly.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;


namespace Blazor.WebAsembly.Services.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController:Controller
    {
        private readonly IMapper _mapper;
        private readonly ICompanyData _companyData;
        public CompaniesController(IMapper mapper,
                                ICompanyData companyData)
        {
            _mapper = mapper;
            _companyData = companyData;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var companies = await _companyData.GetAll();
            companies.ToList().ForEach(async c => await _companyData.AttachAddressesToCompany(c));
            var result = _mapper.Map<IEnumerable<Company>, IEnumerable<CompanyModel>>(companies );

            return Ok(result);
        }
        [HttpPost("new")]
       public async Task<ActionResult> NewCompany([FromBody]CompanyModel model)
        {
            
                var company = _mapper.Map<Company>(model);
            company.Id = Guid.NewGuid();
            await _companyData.CreateCompany(company);
            return Ok(company);
        }
        [HttpPut("update/{Id}")]
        public async Task<ActionResult> UpdateCompany(Guid id, [FromBody] CompanyModel model)
        {

            var company = _mapper.Map<Company>(model);
            await _companyData.UpdateCompany(id,company);
            return Ok(company);
        }
        [HttpPost("add-address")]
        public async Task<ActionResult> NewAddress([FromBody] AddressModel model)
        {
            var address= _mapper.Map<Address>(model);
            address.Id= Guid.NewGuid();
            await _companyData.AddAddress(address);
            return Ok();
        }
        [HttpPut("edit-address/{id}")]
        public async Task<ActionResult> UpdateAddress(Guid id,[FromBody] AddressModel model)
        {
            var address = _mapper.Map<Address>(model);
            
            await _companyData.UpdateAddress(id,address);
            return Ok();
        }
        [HttpDelete("delete-address/{id}")]
        public async Task DeleteAddress(Guid id)
        {
            await _companyData.DeleteAddress(id);
        }
        [HttpDelete("delete-company/{id}")]
        public async Task DeleteCompany(Guid id)
        {
            await _companyData.DeleteCompany(id);
        }
    }
}
