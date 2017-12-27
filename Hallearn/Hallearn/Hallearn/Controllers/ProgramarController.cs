using Hallearn.Model.Model;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Hallearn.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProgramarController : ApiController
    {
        homeProcesos hp = new homeProcesos();

        [HttpGet]
        public IHttpActionResult get()
        {

            var programas = hp.getProgramas();
            return Ok(programas);

        }



    }
}
