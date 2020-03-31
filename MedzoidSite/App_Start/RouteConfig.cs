using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MedzoidSite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
           name: "Default",
           url: "{action}/{id}",
           // defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
           new { controller = "Home", action = "Index", id = UrlParameter.Optional },
           new[] { "Medzoid.Controllers" }
           );

         



            routes.MapRoute(
                name: "blog",
                url: "blog/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new [] { "Medzoid.Controllers" }
            );

            //routes.MapRoute(
            //    name: "Allopathic",
            //    url: "{controller}/{action}/{id}",
            //    new { controller = "Home", action = "Allopathic", id = UrlParameter.Optional },
            //    new[] { "Medzoid.Controllers" }
            //);
            // routes.MapRoute(
            //    name: "Ayurvedic",
            //    url: "{controller}/{action}/{id}",
            //    new { controller = "Home", action = "Ayurvedic", id = UrlParameter.Optional },
            //    new[] { "Medzoid.Controllers" }
            //);
            //routes.MapRoute(
            //    name: "Homeopathic",
            //    url: "{controller}/{action}/{id}",
            //    new { controller = "Home", action = "Homeopathic", id = UrlParameter.Optional },
            //    new[] { "Medzoid.Controllers" }
            //);
            // routes.MapRoute(
            //    name: "Unani",
            //    url: "pharmacy/{action}/{id}",
            //    new { controller = "Home", action = "Unani", id = UrlParameter.Optional },
            //    new[] { "Medzoid.Controllers" }
            //);
            //routes.MapRoute(
            //    name: "Drugs",
            //    url: "pharmacy/{action}/{id}",
            //    new { controller = "Home", action = "Drugs", id = UrlParameter.Optional },
            //    new[] { "Medzoid.Controllers" }
            //);
            
            routes.MapRoute(
                name: "BlogPost",
                url: "BlogPost/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "Medzoid.Controllers" }
            );




            routes.MapRoute(
                name: "Dashboard",
                url: "Admin/Dashboard/{action}/{id}",
                new { controller = "Admin", action = "unclaimeddoctor", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "unclaimeddoctor",
                url: "Admin/unclaimeddoctor/{action}/{id}",
                new { controller = "Admin", action = "unclaimeddoctor", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ClinicDetails",
                url: "Admin/ClinicDetails/{action}/{id}",
                new { controller = "Admin", action = "ClinicDetails", id = UrlParameter.Optional }

            );

            routes.MapRoute(
               name: "EmpProfile",
               url: "Employee/{action}/{id}",
               new { controller = "Admin", action = "EmpProfile", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "EmpAppointment",
               url: "Employee/{action}/{id}",
               new { controller = "Admin", action = "Appointments", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Userprofile",
               url: "User/{action}/{id}",
               new { controller = "User", action = "Userprofile", id = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "DoctorDashboard",
              url: "Doctor/{action}/{id}",
              new { controller = "Doctor", action = "DoctorDashboard", id = UrlParameter.Optional }
          );
        }
    }
}
