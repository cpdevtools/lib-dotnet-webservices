using System.Security.Claims;
using System.Security.Principal;

namespace CpDevTools.Webservices.Services.Users
{
  public record DefaultUser : IUser
  {
    public static explicit operator DefaultUser?(ClaimsPrincipal? c) => c != null ? new DefaultUser(c) : null;

    private readonly ClaimsPrincipal _claimsPrincipal;
    public string Id => _claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ?? _claimsPrincipal.FindFirstValue("sub") ?? "";
    public IEnumerable<Claim> Claims => _claimsPrincipal.Claims;
    public IIdentity? Identity => _claimsPrincipal.Identity;
    public bool IsInRole(string role) => _claimsPrincipal.IsInRole(role);

    public DefaultUser(ClaimsPrincipal claimsPrincipal)
    {
      _claimsPrincipal = claimsPrincipal;
    }
  }

}
