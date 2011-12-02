using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web;
using System.Web.Mvc;

namespace SeoRouting
{
    public class SeoRouteHandler : IRouteHandler
    {
        readonly IRouteHandler _inner;
        readonly bool _forceLowerCase = false;
        readonly bool _forceTrailingSlash = false;

        public SeoRouteHandler(IRouteHandler inner, bool forceTrailingSlash, bool forceLowerCase)
        {
            if (inner == null) throw new ArgumentNullException("inner");

            _inner = inner;
            _forceTrailingSlash = forceTrailingSlash;
            _forceLowerCase = forceLowerCase;
        }

        public SeoRouteHandler(IRouteHandler inner, bool forceTrailingSlash) : this(inner, forceTrailingSlash, false) { }

        public SeoRouteHandler(IRouteHandler inner) : this(inner, false, false) { }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var expectedVirtualPath = requestContext.RouteData.Route.GetVirtualPath(requestContext, requestContext.RouteData.Values);

            if(expectedVirtualPath != null)
            {
                var expected = expectedVirtualPath.VirtualPath;
                var actual = requestContext.HttpContext.Request.PathInfo;

                if (_forceTrailingSlash && !expected.EndsWith("/"))
                {
                    expected += "/";
                }
                else
                {
                    actual = actual.TrimEnd('/');
                }

                if (_forceLowerCase) expected = expected.ToLowerInvariant();

                if (String.Equals(expected, actual, StringComparison.Ordinal))
                {
                    return _inner.GetHttpHandler(requestContext);
                }
                else
                {
                    //url mismatch, return 301 handler
                    return new PermanentRedirectHandler(UrlHelper.GenerateContentUrl(requestContext.HttpContext.Request.AppRelativeCurrentExecutionFilePath + expected, requestContext.HttpContext));
                }
            }

            throw new InvalidOperationException("Route did not return expected virtual path data.");
        }
    }
}
