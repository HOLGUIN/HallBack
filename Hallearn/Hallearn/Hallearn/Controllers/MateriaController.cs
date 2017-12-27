using Hallearn.Model.Model;
using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;


namespace Hallearn.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MateriaController : ApiController
    {

        materiaProcesos mp = new materiaProcesos();

        [HttpGet]
        public IHttpActionResult get()
        {
            try
            {
                var materias = mp.getmaterias();
                return Ok(materias);
            }
            catch { return Content(HttpStatusCode.BadRequest, "LNG_ERROR"); }
        }

        [HttpGet]
        public IHttpActionResult get(sbyte hlnmateriaid)
        {
            materia materia = new materia();

            try
            {
                if (hlnmateriaid != 0)
                {
                    materia = mp.getmateria(hlnmateriaid);
                }
                return Ok(materia);
            }
            catch (Exception e)
            { return Content(HttpStatusCode.BadRequest, "LNG_ERROR"); }
        }


        [HttpPost]
        public IHttpActionResult post(materia materia)
        {

            try
            {
                var response = mp.postmateria(materia);
                if (response.valida)
                {
                    return Ok(response.modelo);
                }

                return Content(HttpStatusCode.BadRequest, response.msj);
            }
            catch { return Content(HttpStatusCode.BadRequest, "LNG_MSJ_6"); }
        }


        [HttpPut]
        public IHttpActionResult put(materia materia)
        {
            try
            {
                var response = mp.putmateria(materia);
                if (response.valida)
                {
                    return Ok(response.modelo);
                }
                return Content(HttpStatusCode.BadRequest, response.msj);
            }
            catch { return Content(HttpStatusCode.BadRequest, "LNG_MSJ_6"); }
        }


        [HttpDelete]
        public IHttpActionResult delete(materia modelo)
        {

            var response = mp.deletemateria(modelo);

            if (response)
                return Ok();

            return Content(HttpStatusCode.BadRequest, "LNG_ERROR");
        }

    }
}
