
using System.Runtime.Serialization;

namespace CpDevTools.Webservices.Models.Errors
{
    [DataContract]
    public record HttpErrorModel<TDetails> : ErrorModel
    {
        [DataMember(EmitDefaultValue = false)]
        public TDetails? Details { get; set; }
    }

    [DataContract]
    public record HttpErrorModel : HttpErrorModel<object> { }
}