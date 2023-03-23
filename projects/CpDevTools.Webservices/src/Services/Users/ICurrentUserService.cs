namespace CpDevTools.Webservices.Services.Users
{
  public interface ICurrentUserService<out TUser> where TUser : IUser
  {
    Type UserType { get; }
    TUser? User { get; }
  }

}
