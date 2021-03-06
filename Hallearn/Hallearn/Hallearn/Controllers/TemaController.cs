﻿using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using Hallearn.Model.Model;

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
                return Content(HttpStatusCode.BadRequest, "LNG_ERROR");
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

                return Content(HttpStatusCode.BadRequest, "LNG_ERROR");
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
                return Content(HttpStatusCode.BadRequest, "LNG_MSJ_6");
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
                return Content(HttpStatusCode.BadRequest, "LNG_MSJ_6"); 
            }

        }


        [System.Web.Http.HttpDelete]
        public IHttpActionResult delete(tema modelo)
        {

            var response = pt.deletetema(modelo);

            if (response)
                return Ok();

            return Content(HttpStatusCode.BadRequest, "LNG_ERROR");

        }


    }
}
