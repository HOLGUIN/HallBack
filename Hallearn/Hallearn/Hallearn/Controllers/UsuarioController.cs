using Hallearn.Models;
using Hallearn.Utility;
using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace Hallearn.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsuarioController : ApiController
    {

        usuarioProcesos up = new usuarioProcesos();

        [System.Web.Http.HttpPost]
        public IHttpActionResult post(usuario usuario)
        {
            try
            {

                var response = up.postusuario(usuario);
                return Ok(response);

            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, "Debe ingresar los campos *");
            }
        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult get()
        {
            MD5Hash ensure = new MD5Hash();
            try
            {
                var usuarios = up.getusuarios();
                return Ok(new { usuarios = usuarios });
            }
            catch (Exception e) { return Content(HttpStatusCode.NotFound, "Error en el proceso"); }
        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult get(int id)
        {
            usuario modelo = new usuario();
            try
            {
                if (id != 0)
                {
                    modelo = up.getusuario(id);
                }
                return Ok(modelo);
            }
            catch { return Content(HttpStatusCode.NotFound, "Usuario no existe"); }
        }

        [System.Web.Http.HttpPut]
        public IHttpActionResult put(usuario modelo)
        {
            try
            {
                var response = up.putusuario(modelo);
                return Ok(response);
            }
            catch { return Content(HttpStatusCode.BadRequest, "Debe ingresar los campos *"); }
        }


        [System.Web.Http.HttpDelete]
        public IHttpActionResult delete(usuario modelo)
        {
            bool response = up.deleteusuario(modelo);

            if (response)
                return Ok();

            return Content(HttpStatusCode.BadRequest, "Error en proceso");

        }

    }



}
