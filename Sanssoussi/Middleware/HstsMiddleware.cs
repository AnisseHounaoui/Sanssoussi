
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace Microsoft.AspNetCore.HttpsEnforcement
{

    // Enables Http Strict Transport Security (HSTS)
    public class HstsMiddleware
    {
        private const string IncludeSubDomains = "; includeSubDomains";
        private const string Preload = "; preload";

        private readonly RequestDelegate _next;
        private readonly StringValues _nameValueHeaderValue;


        // Initialize the HSTS middleware.
        public HstsMiddleware(RequestDelegate next, IOptions<HstsOptions> options)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _next = next;

            var hstsOptions = options.Value;
            var includeSubdomains = hstsOptions.IncludeSubDomains ? IncludeSubDomains : StringSegment.Empty;
            var preload = hstsOptions.Preload ? Preload : StringSegment.Empty; //set the header name Strict-Transport-Security

            _nameValueHeaderValue = new StringValues($"max-age={hstsOptions.MaxAge}{includeSubdomains}{preload}"); //set the header (max age is 30 days meaning it wil take 30days until we renew the code)
        }

   
        // Invoke the middleware.
        public async Task Invoke(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                if (context.Request.Scheme == "https")
                {
                    context.Response.Headers[HeaderNames.StrictTransportSecurity] = _nameValueHeaderValue;
                }
                return Task.CompletedTask;
            });
            await _next(context);
        }
    }
}