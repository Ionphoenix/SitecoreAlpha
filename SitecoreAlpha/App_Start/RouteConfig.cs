using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SitecoreAlpha
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "MemberSearch", action = "Display", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "MemberSearch",
               url: "{controller}/{action}",
               defaults: new { controller = "MemberSearch", action = "Display", id = UrlParameter.Optional }
           );
        }
    }
}
