
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CpDevTools.Webservices.Models.Errors
{
    [DataContract]
    public record ValidationErrorsModel
    {
        public static List<ValidationErrorsModel> FromModelState(ModelStateDictionary ms, string? key = null)
        {
            return ms
                .Select(s => FromModelState(s.Value!, s.Key))
                .ToList();
        }

        public static ValidationErrorsModel FromModelState(ModelStateEntry ms, string? key = null)
        {
            var err = new ValidationErrorsModel
            {
                Key = String.IsNullOrEmpty(key) ? null : key,
                Errors = ms.Errors.Select(i => i.ErrorMessage).ToList() ?? new()
            };
            return err;
        }


        [DataMember(EmitDefaultValue = false)]
        public string? Key { get; set; }
        [Required]
        [DataMember(EmitDefaultValue = false)]
        public List<string> Errors { get; set; } = new();
    }
}