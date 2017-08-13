
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using Hallearn.Models;

namespace Hallearn.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ServerwsController : ApiController
    {


        [System.Web.Http.HttpGet]
        public IHttpActionResult get()
        {
            websocket s = new websocket();
            return Ok();
        }





    }
}