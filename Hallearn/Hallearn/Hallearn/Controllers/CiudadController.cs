using Hallearn.Model.Model;
using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Hallearn.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CiudadController : ApiController
    {

        ciudadProcesos cp = new ciudadProcesos();

        [System.Web.Http.HttpGet]
        public IHttpActionResult get(int hlnciudadid)
        {
            ciudad ciudad = new ciudad();

            try
            {

                if(hlnciudadid!=0)
                {
                    ciudad = cp.getciudad(hlnciudadid);
                }
                return Ok(ciudad);
            }
            catch { }

            return Content(HttpStatusCode.BadRequest, "LNG_ERROR");
        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult get()
        {
            try
            {
                var ciudades = cp.getciudades();
                return Ok(ciudades);
            }
            catch { }
            return Content(HttpStatusCode.BadRequest, "LNG_ERROR");
        }


        [System.Web.Http.HttpPost]
        public IHttpActionResult post(ciudad ciudad)
        {

            try
            {
                var response = cp.postdepto(ciudad);
                if (response.valida)
                {
                    return Ok(response.modelo);
                }

                return Content(HttpStatusCode.BadRequest, response.msj);
            }
            catch(Exception e)
            {
                return Content(HttpStatusCode.BadRequest, "LNG_MSJ_6");
            }
        }

        [System.Web.Http.HttpPut]
        public IHttpActionResult put(ciudad ciudad)
        {

            try
            {
                var response = cp.putciudad(ciudad);
                if (response.valida)
                {
                    return Ok(response.modelo);
                }
                return Content(HttpStatusCode.BadRequest, response.msj);
            }
            catch
            {
                return Content(HttpStatusCode.BadRequest, "LNG_MSJ_6");
            }


        }

        [System.Web.Http.HttpDelete]
        public IHttpActionResult delete(ciudad ciudad)
        {
            var response = cp.deleteciudad(ciudad);

            if (response)
                return Ok();

            return Content(HttpStatusCode.BadRequest, "LNG_ERROR");

        }

    }
}
