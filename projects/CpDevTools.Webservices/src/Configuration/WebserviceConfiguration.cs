

using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

namespace CpDevTools.Webservices.Configuration
{

  public record WebserviceConfiguration
  {
    public static string DefaultKey = "webservice";



    public record ContactConfiguration
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

    public record LicenseConfiguration
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

    public static implicit operator OpenApiInfo(WebserviceConfiguration c)
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

    public string? Version { get; set; } = "0.0.0";
    public string BuildType
    {
      get
      {
#if DEBUG
        return "Debug";
#else
                return "Release";
#endif
      }
    }
    public string? Environment { get; set; } = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    public DateTime? BuiltOn { get; set; }
    public string? CommitSha { get; set; }
    public string? Title { get; set; }
    public string DocumentId
    {
      get
      {
        // => String.Join("", new string[] { Company ?? "" })
        var p = new List<string>();

        if (!String.IsNullOrWhiteSpace(Company))
        {
          p.Add(Company);
        }
        if (!String.IsNullOrWhiteSpace(Title))
        {
          p.Add(Title);
        }

        return String
            .Join("-", p)
            .Replace(' ', '-')
            .Replace('.', '_')
            .ToLower();
      }
    }
    public string? Company { get; set; }
    public string? Description { get; set; }
    public string? RepositoryUrl { get; set; }
    public string? TermsOfService { get; set; }
    public ContactConfiguration? Contact { get; set; }
    public LicenseConfiguration? License { get; set; }

    public WebserviceConfiguration()
    {
      var assembly = Assembly.GetEntryAssembly();

      var product = assembly?.GetCustomAttribute<AssemblyProductAttribute>()?.Product;
      var company = assembly?.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company;
      var description = assembly?.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;
      var title = assembly?.GetCustomAttribute<AssemblyTitleAttribute>()?.Title;
      var version = assembly?.GetCustomAttribute<AssemblyVersionAttribute>()?.Version;
      var informationalVersion = assembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
      var configuration = assembly?.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration;

      var meta = assembly?.GetCustomAttributes<AssemblyMetadataAttribute>();
      var repositoryUrl = meta?.Where(a => a.Key == "RepositoryUrl").Select(a => a.Value).FirstOrDefault();

      Title = product ?? title  ?? "Webservice";
      Company = company;
      RepositoryUrl = repositoryUrl;
      Version = informationalVersion;
    }
  }
}
