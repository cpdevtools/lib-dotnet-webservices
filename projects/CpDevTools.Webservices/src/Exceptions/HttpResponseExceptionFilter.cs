using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Humanizer;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using CpDevTools.Webservices.Exceptions;
using CpDevTools.Webservices.Models.Errors;
using CpDevTools.Webservices.Util;

namespace CpDevTools.Webservices.Exceptions
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        private IWebHostEnvironment _env;
        private ILogger<HttpResponseExceptionFilter> _logger;

        public int Order => int.MaxValue - 10;

        public HttpResponseExceptionFilter(
            IWebHostEnvironment env,
            ILogger<HttpResponseExceptionFilter> logger
        )
        {
            _env = env;
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (HttpUtil.AcceptsJson(context.HttpContext.Request))
            {
                if (context.Exception != null)
                {
                    var exception = context.Exception!;
                    var exceptionType = exception.GetType();

                    ErrorModel error;
                    if (exception is HttpResponseException ex)
                    {
                        HttpErrorModel err = new HttpErrorModel();
                        err.StatusCode = ex.StatusCode;
                        err.Details = ex.Details;
                        error = err;
                    }
                    else
                    {
                        ExceptionErrorModel err = new ExceptionErrorModel();
                        err.StatusCode = StatusCodes.Status500InternalServerError;
                        if (!_env.IsProduction())
                        {
                            err.StackTrace = exception.StackTrace;
                        }
                        error = err;
                    }
                    // Dasherize doesn't work unless Underscore is called first
                    error.Error = exceptionType.Name.Underscore().Dasherize();
                    error.Message = exception.Message;

                    ReturnError(context, error);
                    context.ExceptionHandled = true;
                }
                else if (context.Result is ErrorModel error)
                {
                    ReturnError(context, error);
                }
            }
        }

        private static void ReturnError(ActionExecutedContext context, ErrorModel error)
        {
            context.HttpContext.Response.Headers.AccessControlAllowOrigin = context.HttpContext.Request.Headers.Origin;
            error.TraceId = context.HttpContext.TraceIdentifier;
            context.Result = new ObjectResult(error)
            {
                StatusCode = error.StatusCode
            };
        }

        public void OnActionExecuting(ActionExecutingContext context) { }
    }
}