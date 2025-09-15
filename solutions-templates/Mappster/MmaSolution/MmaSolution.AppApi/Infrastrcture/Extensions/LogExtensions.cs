namespace MmaSolution.AppApi.Infrastrcture.Extensions
{
    public static class LogExtensions
    {
        public static string GetMetricsCurrentResourceName(this HttpContext httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            Endpoint endpoint = httpContext.Features.Get<IEndpointFeature>()?.Endpoint;


            return endpoint?.Metadata.GetMetadata<EndpointNameMetadata>()?.EndpointName;

        }

        public static string GetCorrelationId(this HttpContext httpContext)
        {
            httpContext.Request.Headers.TryGetValue("Cko-Correlation-Id", out StringValues correlationId);
            return correlationId.FirstOrDefault() ?? httpContext.TraceIdentifier;
        }
    }
}
