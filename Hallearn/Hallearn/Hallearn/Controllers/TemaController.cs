using System.Net;
using System.Web.Http;
using Hallearn.Models;
using System.Web.Http.Cors;

namespace Hallearn.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TemaController : ApiController
    {

        temaProcesos pt = new temaProcesos();

        [System.Web.Http.HttpGet]
        public IHttpActionResult get()
        {

            try
            {
                var modelo = pt.getTemas();
                return Ok(modelo);
            }
            catch
            {
                return Content(HttpStatusCode.BadRequest, "Error en proceso");
            }
        }


        [System.Web.Http.HttpGet]
        public IHttpActionResult get(int hlntemaid)
        {

            try
            {
                tema modelo = new tema();

                if (hlntemaid != 0)
                {
                    modelo = pt.getTema(hlntemaid);
                }
                return Ok(modelo);
            }
            catch
            {

                return Content(HttpStatusCode.BadRequest, "Error en proceso");
            }

        }


        [System.Web.Http.HttpPost]
        public IHttpActionResult post(tema tema)
        {

            try
            {
                var response = pt.postTema(tema);
                if(response.valida)
                {
                    return Ok(response.modelo);
                }

                return Content(HttpStatusCode.BadRequest, response.msj);
            }
            catch
            {
                return Content(HttpStatusCode.BadRequest, "Debe ingresar los campos *");
            }
        }


        [System.Web.Http.HttpPut]
        public IHttpActionResult put(tema tema)
        {

            try
            {
                var response = pt.putTema(tema);
                if(response.valida)
                {
                    return Ok(tema);
                }
                return Content(HttpStatusCode.BadRequest, response.msj);
            }
            catch
            {
                return Content(HttpStatusCode.BadRequest, "Debe ingresar los campos *"); 
            }

        }


        [System.Web.Http.HttpDelete]
        public IHttpActionResult delete(tema modelo)
        {

            var response = pt.deletetema(modelo);

            if (response)
                return Ok();

            return Content(HttpStatusCode.BadRequest, "Error en proceso");

        }


    }
}
