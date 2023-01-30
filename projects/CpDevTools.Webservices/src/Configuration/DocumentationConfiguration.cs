

using System.Reflection;
using Microsoft.OpenApi.Models;

namespace CpDevTools.Webservices.Configuration
{

    public class DocumentationConfiguration
    {
        public class ContactConfiguration
        {
            public string? Name { get; set; }
            public string? Url { get; set; }
            public string? Email { get; set; }

            public static implicit operator OpenApiContact(ContactConfiguration c)
            {
                return new OpenApiContact
                {
                    Name = c.Name,
                    Email = c.Email,
                    Url = String.IsNullOrWhiteSpace(c.Url) ? null : new Uri(c.Url),
                };
            }
        }

        public class LicenseConfiguration
        {
            public static implicit operator OpenApiLicense(LicenseConfiguration c)
            {
                return new OpenApiLicense
                {
                    Name = c.Name,
                    Url = String.IsNullOrWhiteSpace(c.Url) ? null : new Uri(c.Url),
                };
            }
            public string? Name { get; set; }
            public string? Url { get; set; }
        }

        public static implicit operator OpenApiInfo(DocumentationConfiguration c)
        {
            return new OpenApiInfo
            {
                Title = c.Title,
                Description = c.Description,
                Version = c.Version,
                TermsOfService = String.IsNullOrWhiteSpace(c.TermsOfService) ? null : new(c.TermsOfService),
                Contact = c.Contact ?? new(),
                License = c.License ?? new(),
            };
        }

        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Version { get; set; }
        public string? TermsOfService { get; set; }
        public ContactConfiguration? Contact { get; set; }
        public LicenseConfiguration? License { get; set; }

        public DocumentationConfiguration()
        {
            var name = Name;
            if (String.IsNullOrWhiteSpace(name))
            {
                name = Assembly.GetEntryAssembly()?.GetName().Name ?? "Webservice";
                Name = name.ToLower();
            }

            if (String.IsNullOrWhiteSpace(Title))
            {
                Title = name + " Documentation";
            }

            if (String.IsNullOrWhiteSpace(Version))
            {
                Version = "0.0.0";
            }
        }
    }
}