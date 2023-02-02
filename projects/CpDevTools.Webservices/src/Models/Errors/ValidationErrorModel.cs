
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace CpDevTools.Webservices.Models.Errors
{

    [DataContract]
    public record ValidationErrorModel : HttpErrorModel<List<ValidationErrorsModel>>
    {
        public ValidationErrorModel() : base()
        {
            StatusCode = StatusCodes.Status400BadRequest;
            Error = "validation-errors";
            Message = "One or more validation errors occurred.";
        }
    }
}