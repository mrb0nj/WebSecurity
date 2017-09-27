using System;
using System.Web;

namespace WebSecurity.Modules
{
    public class RemoveHeaders : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += OnPreSendRequestHeaders;
        }

        public void Dispose() { }

        void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;

            if (app != null && app.Context != null)
            {
                var headers = Properties.Settings.Default.RemoveHeaders.Split(',');
                foreach (var header in headers)
                {
                    app.Response.Headers.Remove(header);
                }
            }
        }
    }
}
