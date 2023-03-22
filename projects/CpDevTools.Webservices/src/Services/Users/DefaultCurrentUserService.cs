using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CpDevTools.Webservices.Services.Users
{
  public class DefaultCurrentUserService : ICurrentUserService<DefaultUser>
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private ClaimsPrincipal? HttpUser => _httpContextAccessor.HttpContext?.User;

    public DefaultCurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }
    public Type UserType => typeof(DefaultUser);

    public DefaultUser? User => (DefaultUser?)HttpUser;
  }

}
