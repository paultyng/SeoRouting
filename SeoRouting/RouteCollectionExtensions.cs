using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc;

namespace SeoRouting
{
    public static class RouteCollectionExtensions
    {
        public static void MapSeoRoute(this RouteCollection routes, string name, string url, object defaults, object constraints)
        {
            bool forceTrailingSlash = url.EndsWith("/");

            routes.Add(name, new Route(url, new RouteValueDictionary(defaults), new RouteValueDictionary(constraints), new SeoRouteHandler(new MvcRouteHandler(), forceTrailingSlash)));
        }

        public static void MapSeoRoute(this RouteCollection routes, string name, string url, object defaults)
        {
            MapSeoRoute(routes, name, url, defaults, null);
        }

        public static void MapSeoRoute(this RouteCollection routes, string name, string url)
        {
            MapSeoRoute(routes, name, url, null, null);
        }
    }
}
