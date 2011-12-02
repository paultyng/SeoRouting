using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using Moq;
using System.Collections.Specialized;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SeoRouting.Tests
{
    class RedirectionHelper
    {
        static Mock<RequestContext> GetRequestContext(string requestUrl, string routeUrl, IRouteHandler handler)
        {
            var route = new Route(routeUrl, handler);

            var sv = new Mock<NameValueCollection>(MockBehavior.Strict);
            sv.Setup(m => m.Get("IIS_UrlRewriteModule")).Returns((string)null);

            var req = new Mock<HttpRequestBase>(MockBehavior.Strict);
            req.Setup(m => m.AppRelativeCurrentExecutionFilePath).Returns("~/");
            req.Setup(m => m.PathInfo).Returns(requestUrl);
            req.Setup(m => m.ApplicationPath).Returns("/");
            req.Setup(m => m.ServerVariables).Returns(sv.Object);

            var resp = new Mock<HttpResponseBase>(MockBehavior.Strict);
            resp.Setup(m => m.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(url => url);

            var ctx = new Mock<HttpContextBase>(MockBehavior.Strict);
            ctx.Setup(m => m.Request).Returns(req.Object);
            ctx.Setup(m => m.Response).Returns(resp.Object);

            var reqCtx = new Mock<RequestContext>(MockBehavior.Strict);
            reqCtx.Setup(m => m.HttpContext).Returns(ctx.Object);
            reqCtx.Setup(m => m.RouteData).Returns(route.GetRouteData(ctx.Object));

            return reqCtx;
        }

        public static string TestRedirect(string routeUrl, string testUrl, bool trailingSlash, bool forceLowerCase)
        {
            var innerHandler = new Mock<IRouteHandler>(MockBehavior.Strict);
            var seoHandler = new SeoRouteHandler(innerHandler.Object, trailingSlash, forceLowerCase);

            var ctx = GetRequestContext(testUrl, routeUrl, seoHandler);

            var innerHttpHandler = new Mock<IHttpHandler>();

            innerHandler.Setup(m => m.GetHttpHandler(ctx.Object)).Returns(innerHttpHandler.Object).Verifiable();

            var actualHandler = seoHandler.GetHttpHandler(ctx.Object);

            Assert.IsNotNull(actualHandler);

            var redirHandler = actualHandler as PermanentRedirectHandler;

            if (redirHandler != null) return redirHandler.Location;

            return null;
        }
    }
}
