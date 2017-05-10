using Hallearn.Model.Model;
using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Hallearn.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProgTemaController : ApiController
    {


        progtemaProcesos pp = new progtemaProcesos();

        [HttpGet]
        public IHttpActionResult get()
        {
            try
            {
                var modelo = pp.getprogtemas();
                return Ok(modelo);
            }
            catch(Exception e) { }
            return Content(HttpStatusCode.BadRequest, "Error en proceso");
        }

        [HttpGet]
        public IHttpActionResult get(sbyte hlnprogtemaid)
        {
            try
            {
                progtema progtema = new progtema();

                if (hlnprogtemaid != 0)
                {
                    progtema = pp.getprogtema(hlnprogtemaid);
                }

                return Ok(progtema);
            }
            catch (Exception e) { }

            return Content(HttpStatusCode.BadRequest, "Error en proceso.");
        }


        [HttpPost]
        public IHttpActionResult post(progtema modelo)
        {

            try
            {
                var response = pp.postprogtema(modelo);
                if (response.valida)
                {
                    return Ok(response.modelo);
                }

                return Content(HttpStatusCode.BadRequest, response.msj);
            }
            catch { return Content(HttpStatusCode.BadRequest, "Debe ingresar los campos *"); }
        }


        [HttpPut]
        public IHttpActionResult put(progtema modelo)
        {

            try
            {
                var response = pp.putprogtema(modelo);
                if (response.valida)
                {
                    return Ok(response.modelo);
                }
                return Content(HttpStatusCode.BadRequest, response.msj);
            }
            catch { }
            return Content(HttpStatusCode.BadRequest, "Debe ingresar los campos *");
        }


        [HttpDelete]
        public IHttpActionResult delete(progtema modelo)
        {
            var response = pp.deleteprogtema(modelo);

            if (response)
                return Ok();

            return Content(HttpStatusCode.BadRequest, "Error en proceso");
        }


    }
}
