using Hallearn.Model.Model;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Hallearn.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginController : ApiController
    {

        loginProcesos lp = new loginProcesos();

        [HttpGet]
        public IHttpActionResult Get(int usuarioid)
        {
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult post(string usuario, string clave)
        {

            var response = lp.login(usuario, clave);

            if (response.valida)
                return Ok(response.modelo);

            return Content(HttpStatusCode.BadRequest, response.msj);

        }



    }
}
