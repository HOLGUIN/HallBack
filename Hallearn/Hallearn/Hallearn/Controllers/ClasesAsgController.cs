using Hallearn.Model.Model;
using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Hallearn.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ClasesAsgController : ApiController
    {

        clasesasgProcesos cp = new clasesasgProcesos();

        [HttpGet]
        public IHttpActionResult get(int hlnprogtemaid, DateTime fecha)
        {
            try
            {
                var modelo = cp.getClasesasg(hlnprogtemaid, fecha);
                return Ok(modelo);
            }
            catch (Exception e) { }
            return Content(HttpStatusCode.BadRequest, "LNG_ERROR");
        }


        [HttpGet]
        public IHttpActionResult getlt(int hlnprogtemaidlt, DateTime fecha)
        {
            try
            {
                var modelo = cp.getClass(hlnprogtemaidlt, fecha);
                return Ok(modelo);
            }
            catch (Exception e) { }
            return Content(HttpStatusCode.BadRequest, "LNG_ERROR");
        }

    }
}
