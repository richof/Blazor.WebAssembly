using AutoMapper;

using Blazor.WebAssembly.Core;
using Blazor.WebAssembly.Core.ViewModels;

namespace Blazor.WebAsembly.Services.AutoMapper
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Company, CompanyViewModel>().ReverseMap();
            CreateMap<Address, AddressViewModel>().ReverseMap();
        }
    }
}
