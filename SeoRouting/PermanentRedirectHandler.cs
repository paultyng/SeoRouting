using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SeoRouting
{
    sealed class PermanentRedirectHandler : IHttpHandler
    {
        public PermanentRedirectHandler()
        {
        }

        public PermanentRedirectHandler(string location)
        {
            Location = location;
        }

        public string Location { get; set; }

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.StatusCode = 301;
            context.Response.StatusDescription = "Moved Permanently";
            context.Response.AddHeader("Location", Location);
        }
    }
}
