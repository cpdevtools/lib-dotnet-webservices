


using Microsoft.AspNetCore.Http;




namespace CpDevTools.Webservices.Util
{
  public static class HttpUtil
  {
    private static readonly string[] jsonTypes = new string[] { "application/json", "text/json", "text/plain", "*/*" };
    public static bool AcceptsJson(HttpRequest req)
    {
      return req.Headers.Accept.Any(a => jsonTypes.Contains(a));
    }

  }

}
