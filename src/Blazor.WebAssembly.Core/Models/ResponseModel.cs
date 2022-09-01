
namespace Blazor.WebAssembly.Core.Models
{
    public class ResponseModel
    {
        public int StatusCode { get; set; }
        public string Data { get; set; }
     
        public List<ErrorModel> Errors { get; set; }
        public ResponseModel()
        {
            Errors = new List<ErrorModel>();
        }

    }
    public class ErrorModel
    {
        public ErrorModel(string property, string message)
        {
            Property = property;
            Message = message;
        }

        public string Property { get; set; }
        public string Message { get; set; }
    }
}
