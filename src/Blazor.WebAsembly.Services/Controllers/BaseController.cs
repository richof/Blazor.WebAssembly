using Blazor.WebAssembly.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Blazor.WebAsembly.Services.Controllers
{
    [ApiController]
    public class BaseController:Controller
    {
        private List<ErrorModel> errors=new List<ErrorModel>();
        protected ActionResult CustomResponse(object result=null)
        {
            if(errors.Any())
            {
                return BadRequest(errors);
            }
            return Ok(result);
        }
        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            //var erros = modelState.Values.SelectMany(e => e.Errors);
            var keys = modelState.Keys;
            //foreach (var erro in erros)
            //{
            //    AddError(erro.ErrorMessage);
            //}
            foreach(var key in keys)
            {
                var erros = modelState[key].Errors;
                foreach (var erro in erros)
                {
                    AddError(erro.ErrorMessage, key);
                }
            }
            return CustomResponse();
           
        }
        protected void AddError(string message,string key= null)
        {
            errors.Add(new ErrorModel(key, message));
        }
        //protected void AddError(string key, string message)
        //{
        //    errors.Add(new ErrorModel(key, message));
        //}

    }
    
}
