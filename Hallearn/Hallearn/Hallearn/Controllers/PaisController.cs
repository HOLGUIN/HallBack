using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using Hallearn.Models;

namespace Hallearn.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PaisController : ApiController
    {
        paisProceso pp = new paisProceso();

        [HttpGet]
        public IHttpActionResult get()
        {
            try
            {
                var modelo = pp.getpaises();
                return Ok(modelo);
            }
            catch { }
            return Content(HttpStatusCode.BadRequest, "Error en proceso");
        }

        [HttpGet]
        public IHttpActionResult get(sbyte hlnpaisid)
        {
            try
            {
                pais pais = new pais();

                if(hlnpaisid!=0)
                {
                    pais = pp.getpais(hlnpaisid);
                }
               
                return Ok(pais);
            }
            catch(Exception e) { }

            return Content(HttpStatusCode.BadRequest, "Error en proceso.");
        }


        [HttpPost]
        public IHttpActionResult post(pais modelo)
        {

            try
            {
                var response = pp.postpais(modelo);
                if (response.valida)
                {
                    return Ok(response.modelo);
                }

                return Content(HttpStatusCode.BadRequest, response.msj);
            }
            catch { return Content(HttpStatusCode.BadRequest, "Debe ingresar los campos *"); }
        }


        [HttpPut]
        public IHttpActionResult put(pais modelo)
        {

            try
            {
                var response = pp.putpais(modelo);
                if(response.valida)
                {
                    return Ok(response.modelo);
                }
                return Content(HttpStatusCode.BadRequest, response.msj);
            }
            catch { }
            return Content(HttpStatusCode.BadRequest, "Debe ingresar los campos *");
        }


        [HttpDelete]
        public IHttpActionResult delete(pais modelo)
        {
            var response = pp.deletepais(modelo);

            if (response)
                return Ok();

            return Content(HttpStatusCode.BadRequest, "Error en proceso");
        }

    }
}
