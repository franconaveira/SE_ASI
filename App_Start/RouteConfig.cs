using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SE_ASI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Registrar_Denuncia",
                url: "Registrar",
                defaults: new { controller = "Registrar", action = "Registrar" }
              );

            routes.MapRoute(
                 name: "Confirmar",
                 url: "Confirmar",
                 defaults: new { controller = "Confirmar", action = "Confirmar" }
              );
            routes.MapRoute(
                 name: "Confirmar_Judicial",
                 url: "Confirmar_Judicial",
                 defaults: new { controller = "Confirmar_Judicial", action = "Confirmar_Judicial" }
  );
        }
    }
}
