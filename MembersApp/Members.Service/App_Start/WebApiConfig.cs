using Members.EF.Model;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using WebApiContrib.Formatting.Jsonp;

namespace Members.Service
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.EnableCors();

            //var cors = new EnableCorsAttribute("http://localhost:49734", "*", "*");

            var cors = new EnableCorsAttribute("*", "*", "*");

            config.EnableCors(cors);

            //config.AddJsonpFormatter();

            config.Formatters.Add(new XmlMediaTypeFormatter());

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();

            builder.EntitySet<Member>("Members");
            //builder.EntitySet<MemberPayment>("MemberPayments");
            builder.EntitySet<Device>("Device");

            config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());

        }
    }
}
