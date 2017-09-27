using System;
using System.Web;

namespace WebSecurity.Modules
{
    public class AddHttpStrictTransportSecurityHeader : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += OnPreSendRequestHeaders;
        }

        public void Dispose() { }

        void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            var context = GetContext(sender);
            if (context != null && IsSecure(context))
                context.Response.Headers.Add("Strict-Transport-Security", Properties.Settings.Default.StrictTransportSecurity);
        }

        HttpContext GetContext(object sender)
        {
            var app = sender as HttpApplication;
            if (app != null && app.Context != null)
                return app.Context;

            return null;
        }

        static bool IsSecure(HttpContext context)
        {
            // custom headers that indicate that even though the request is over http, the site is behind
            // a secure firewall or load balancer with ssl offloading.

            // Microsoft (TMG etc)
            var frontEndHttps = context.Request.Headers["Front-End-Https"] ?? "off";

            // Others
            var forwardedProto = context.Request.Headers["X-Forwarded-Proto"] ?? "http";

            // Return true for native secure requests or where the headers above are the correct value
            return context.Request.IsSecureConnection ||
                    frontEndHttps.Equals("on", StringComparison.InvariantCultureIgnoreCase) ||
                    forwardedProto.Equals("https", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
