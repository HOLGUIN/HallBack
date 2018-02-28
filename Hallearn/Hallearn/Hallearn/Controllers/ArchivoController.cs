using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Hallearn.Model.Model;
using System.Web.Http.Results;

namespace Hallearn.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ArchivoController : ApiController
    {

        archivoProcesos ap = new archivoProcesos();
   
        [System.Web.Http.HttpPost]
        public IHttpActionResult UploadFile(int hlnclaseid)
        {
            return Ok(ap.crearArchivo(System.Web.HttpContext.Current.Request.Files, hlnclaseid));
        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult get(int hlnclaseid)
        {
            return Ok(ap.getArchivos(hlnclaseid));
        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult DownloadFile(int hlnarchivoid)
        {
           
            var response = ap.DescargarArchivos(hlnarchivoid);
            if (response != null)
            {

                ResponseMessageResult responseMessageResult = ResponseMessage(response);
                return Ok(responseMessageResult);
            }

         

            return Content(HttpStatusCode.BadRequest, "LNG_ERROR");
        }

        [System.Web.Http.HttpDelete]
        public IHttpActionResult delete(int hlnarchivoid)
        {
            if (ap.eliminarArchivo(hlnarchivoid))
                return Ok(true);

            return Content(HttpStatusCode.BadRequest, "LNG_ERROR");
        }

    }
}
