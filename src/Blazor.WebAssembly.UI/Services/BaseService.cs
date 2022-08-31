using Blazor.WebAssembly.Core.Models;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Blazor.WebAssembly.UI.Services
{
    public class BaseService
    {
        protected async Task<ResponseModel> MapResponse(HttpResponseMessage httpResponse)
        {
            var response = new ResponseModel();
            if(httpResponse.IsSuccessStatusCode)
            {

                response.Data= await httpResponse.Content.ReadAsStringAsync();
                return response;
            }
            JsonSerializerOptions options = new(JsonSerializerDefaults.Web)
            {
               PropertyNameCaseInsensitive=false
            };           
            response.Errors= JsonSerializer.Deserialize<List<ErrorModel>>(await httpResponse.Content.ReadAsStringAsync(), options);
            return response;
        }
    }
}
