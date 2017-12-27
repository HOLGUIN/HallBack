using Hallearn.Model.Model;
using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Hallearn.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ListasController : ApiController
    {

        listasProcesos lp = new listasProcesos();


        [System.Web.Http.HttpGet]
        public IHttpActionResult get(bool? paises, bool? depts, bool? ciudades, bool? materias, bool? profes, bool? temas)
        {
            try
            {
                var modelo = lp.getlistas(paises, depts, ciudades, materias, profes, temas);
                return Ok(modelo);
            }
            catch (Exception e) { return Content(HttpStatusCode.BadRequest, "Error en proceso"); }

        }





    }
}
