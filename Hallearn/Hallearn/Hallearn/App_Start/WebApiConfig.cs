using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Web.Http.Cors;
using Microsoft.AspNet.WebApi.Extensions.Compression.Server;
using System.Net.Http.Extensions.Compression.Core.Compressors;

namespace Hallearn
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            var cors = new EnableCorsAttribute("http://localhost:61123/", "*", "*");
            config.EnableCors(cors);

            //esta linea valida que las peticiones sean https de lo contrario las cancela
            //config.Filters.Add(new RequireHttpsAttribute());

            //Configuracion para que las respuestas desde el servidor sean json
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.Add(config.Formatters.JsonFormatter);
            // Web API routes
            config.MapHttpAttributeRoutes();

            //Configuracion para que las respuestas sean comprimidas en gzip
            //Install-Package Microsoft.AspNet.WebApi.Extensions.Compression.Server
            GlobalConfiguration.Configuration.MessageHandlers.Insert(0, new ServerCompressionHandler
                (new GZipCompressor(), new DeflateCompressor()));

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
