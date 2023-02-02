
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CpDevTools.Webservices.Models.Errors
{
  [DataContract]
  public record ErrorModel
  {
    /// <summary>
    /// Http Status Code
    /// </summary>
    [Required]
    [DataMember(EmitDefaultValue = false)]
    public int StatusCode { get; set; } = 0;
    /// <summary>
    /// Error Id
    /// </summary>
    [Required]
    [DataMember(EmitDefaultValue = false)]
    public string Error { get; set; } = "";
    /// <summary>
    /// Trace Id
    /// </summary>
    [Required]
    [DataMember(EmitDefaultValue = false)]
    public string TraceId { get; set; } = "";
    [DataMember(EmitDefaultValue = false)]
    public string? Message { get; set; } = null;
  }
}
