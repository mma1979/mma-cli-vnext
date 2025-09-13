using Microsoft.AspNetCore.Http;
using MmaSolution.AppApi.Infrastrcture.Extensions;
using Serilog.Context;

using System.Threading.Tasks;

namespace MmaSolution.AppApi.Infrastrcture.Middlewares
{
    public class RequestLogContextMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLogContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            using (LogContext.PushProperty("CorrelationId", context.GetCorrelationId()))
            {
                return _next.Invoke(context);
            }
        }
    }
}
