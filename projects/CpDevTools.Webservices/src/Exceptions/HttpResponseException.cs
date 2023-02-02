namespace CpDevTools.Webservices.Exceptions
{

    public class HttpResponseException : Exception
    {
        public int StatusCode { get; }
        public object? Details { get; }

        public HttpResponseException(int statusCode, object? details = null) : this(statusCode, null, details) { }

        public HttpResponseException(int statusCode, string? message = null, object? details = null) : base(message)
        {
            StatusCode = statusCode;
            Details = details;
        }


    }
}