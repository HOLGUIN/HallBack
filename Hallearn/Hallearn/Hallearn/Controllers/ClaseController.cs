using Halliarn.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Hallearn.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ClaseController : ApiController
    {

        claseProcesos cp = new claseProcesos();

        [HttpPost]
        public IHttpActionResult post(clase clase)
        {

            var response = cp.postClase(clase);
            return Ok(response);
        }

        [HttpGet]
        public IHttpActionResult get(int alumnoid)
        {
            var clases = cp.getClases(alumnoid);
            return Ok(clases);
        }
    }
}
