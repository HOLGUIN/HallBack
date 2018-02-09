using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Hallearn.Model.Model;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Hallearn.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ChatController : ApiController
    {

        chatProcesos cp = new chatProcesos();

        [HttpPost]
        public IHttpActionResult post(chat modelo)
        {
            try
            {
                cp.savechat(modelo);
                return Ok();
            }
            catch { return Content(HttpStatusCode.BadRequest, "Error"); }
        }

        [HttpGet]
        public IHttpActionResult get(int hlnclaseid)
            {
            try
            {
                var response = cp.getchat(hlnclaseid);
                return Ok(response);
            }
            catch { return Content(HttpStatusCode.BadRequest, "Error"); }
        }



    }
}
