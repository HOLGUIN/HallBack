using System;
using System.Net;
using System.Web.Http;
using Hallearn.Models;
using System.Web.Http.Cors;

namespace Hallearn.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DepartamentoController : ApiController
    {
        deptProcesos dp = new deptProcesos();

        [System.Web.Http.HttpGet]
        public IHttpActionResult get()
        {
            try
            {
                var modelo = dp.getdeptos();
                return Ok(modelo);
            }
            catch { }
            return Content(HttpStatusCode.BadRequest, "LNG_ERROR");
        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult get(int hlndepartamentoid)
        {
            
            try
            {
                departamento dep = new departamento();

                if (hlndepartamentoid != 0)
                {
                    dep = dp.getdepto(hlndepartamentoid);
                }

                return Ok(dep);
            }
            catch(Exception e) { }

            return Content(HttpStatusCode.BadRequest, "LNG_ERROR");
        }

        [System.Web.Http.HttpPost]
        public IHttpActionResult post(departamento depto)
        {
            try
            {
                var response = dp.postdepto(depto);
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

        [System.Web.Http.HttpPut]
        public IHttpActionResult put(departamento depto)
        {

            try
            {
                var response = dp.putdepto(depto);
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
        public IHttpActionResult delete(departamento depto)
        {
            var response = dp.deletedepto(depto);

            if (response)
                return Ok();

            return Content(HttpStatusCode.BadRequest, "LNG_ERROR");
        }

    }
}
