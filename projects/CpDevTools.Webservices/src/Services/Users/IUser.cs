using System.Security.Claims;
using System.Security.Principal;

namespace CpDevTools.Webservices.Services.Users
{
  public interface IUser : IPrincipal
  {
    string Id { get; }
    IEnumerable<Claim> Claims { get; }
  }

}
