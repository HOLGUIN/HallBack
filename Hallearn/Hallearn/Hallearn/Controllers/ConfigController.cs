using Hallearn.Model.Model;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Hallearn.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ConfigController : ApiController
    {

        configProcesos cp = new configProcesos();

        [HttpGet]
        public IHttpActionResult get()
        {
            try
            {
                var config = cp.getconfig();
                return Ok(config);
            }
            catch { }
            return Content(HttpStatusCode.BadRequest, "LNG_ERROR");
        }

        [HttpPut]
        public IHttpActionResult put(config config)
        {
            try
            {
                config = cp.putconfig(config);
                return Ok(config);
            }
            catch { }
            return Content(HttpStatusCode.BadRequest, "LNG_ERROR");
        }
    }
}
