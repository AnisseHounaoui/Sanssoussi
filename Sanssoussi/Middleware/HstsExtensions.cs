
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.HttpsEnforcement
{
    // Extension methods for the HSTS middleware.

    public static class HstsExtensions
    {
        // Adds middleware for using HSTS, which adds the Strict-Transport-Security header.
        public static IApplicationBuilder UseHsts(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseHsts(new HstsOptions());
        }

       // Adds middleware for add HSTS, which adds the Strict-Transport-Security header.     
        public static IApplicationBuilder UseHsts(this IApplicationBuilder app, HstsOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<HstsMiddleware>(Options.Create(options));
        }
    }
}