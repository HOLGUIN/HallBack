using Hallearn.Model.Model;
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

        [System.Web.Http.HttpGet]
        public IHttpActionResult get(int alumnoid, bool activo)
        {
            if (activo)
            {
                var clases = cp.getClasesActivas(alumnoid);
                return Ok(clases);
            }
            else
            {
                var clases = cp.getClases(alumnoid);
                return Ok(clases);
            }
        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult get(int profesorid)
        {
                var clases = cp.getClasesprof(profesorid);
                return Ok(clases);
        }
    }
}
