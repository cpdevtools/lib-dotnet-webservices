
using System.Runtime.Serialization;

namespace CpDevTools.Webservices.Models.Errors
{
    [DataContract]
    public record ExceptionErrorModel : ErrorModel
    {
        [DataMember(EmitDefaultValue = false)]
        public string? StackTrace { get; set; }
    }
}