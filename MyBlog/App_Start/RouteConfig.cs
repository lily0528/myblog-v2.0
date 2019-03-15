using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyBlog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            ////need add  [Route("myblog/{namd}")] on the top of function in control
         routes.MapMvcAttributeRoutes();
           // routes.MapRoute(
           //    name: "NewSlug",
           //    url: "{controller}/slug",
           //    defaults: new { controller = "Blog", action = "details", id = UrlParameter.Optional }
           //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
