using AutoMapper;

using Blazor.WebAssembly.Core;
using Blazor.WebAssembly.Core.Models;
using System.Security.Claims;

namespace Blazor.WebAsembly.Services.AutoMapper
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Company, CompanyModel>().ReverseMap();
            CreateMap<Address, AddressModel>().ReverseMap();
            CreateMap<Claim, ClaimModel>();
        }
    }
}
