using Hallearn.Data;
using Hallearn.Models;
using Hallearn.Utility;
using System.Linq;

namespace Hallearn.Model.Model
{
    class loginModels
    {
    }

    public class loginProcesos
    {

        db_HallearnEntities context = new db_HallearnEntities();
        usuarioProcesos up = new usuarioProcesos();
        MD5Hash md5 = new MD5Hash();

        public response login(string usuario, string clave)
        {

            response response = new response();

            string clave_md5 = md5.CalculateMD5Hash(clave);
            hlnusuario user = context.hlnusuario.Where(c => c.username == usuario && c.md5 == clave_md5 && c.activo == true).FirstOrDefault();

            if (user != null)
            {
                usuario modelo = up.getusuario(user.hlnusuarioid);
                response.valida = true;
                response.modelo = modelo;
                return response;
            }

            response.valida = false;
            response.msj = "Usuario no existe o clave errada";
            return response;

        }
    }
}
