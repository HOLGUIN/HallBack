using System.Collections.Generic;
using System.Linq;
using Hallearn.Data;
using Hallearn.Utility;
using System;

namespace Hallearn.Models
{
    public class usuario
    {
        public int hlnusuarioid { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string descripcion { get; set; }
        public sbyte edad { get; set; }
        public string telefono { get; set; }
        public string celular { get; set; }
        public string correo { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string password2 { get; set; }
        public sbyte hlnpaisid { get; set; }
        public int? hlndepartamentoid { get; set; }
        public int? hlnciudadid { get; set; }
        public string pais { get; set; }
        public string departamento { get; set; }
        public string ciudad { get; set; }
        public string md5 { get; set; }
        public bool administrador { get; set; }
        public bool profesor { get; set; }
        public bool alumno { get; set; }
        public bool activo { get; set; }
    }




    public class usuarioProcesos
    {
        db_HallearnEntities context = new db_HallearnEntities();
        MD5Hash ensure = new MD5Hash();
        paisProceso pp = new paisProceso();
        deptProcesos dp = new deptProcesos();
        ciudadProcesos cp = new ciudadProcesos();

        public usuario getusuario(int hlnusuarioid)
        {
            hlnusuario hlnusuario = context.hlnusuario.Find(hlnusuarioid);
            response response = new response();
            usuario modelo = new usuario()
            {
                nombres = hlnusuario.nombres,
                apellidos = hlnusuario.apellidos,
                descripcion = hlnusuario.nombres + " " + hlnusuario.apellidos,
                edad = hlnusuario.edad,
                telefono = hlnusuario.telefono,
                celular = hlnusuario.celular,
                correo = hlnusuario.correo,
                username = hlnusuario.username,
                hlnpaisid = hlnusuario.hlnpaisid,
                hlndepartamentoid = hlnusuario.hlndepartamentoid,
                hlnciudadid = hlnusuario.hlnciudadid,
                //pais = getpais(hlnusuario.hlnpaisid),
                //departamento = getdepartamento(hlnusuario.hlndepartamentoid),
                //ciudad = getciudad(hlnusuario.hlnciudadid),
                hlnusuarioid = hlnusuario.hlnusuarioid,
                password = hlnusuario.password,
                md5 = ensure.CalculateMD5Hash(hlnusuario.password),
                administrador = hlnusuario.administrador.Value,
                profesor = hlnusuario.profesor.Value,
                alumno = hlnusuario.alumno.Value,
                activo = hlnusuario.activo.Value,
                password2 = hlnusuario.password2
            };


            modelo.pais = pp.getpais(hlnusuario.hlnpaisid).nombre;
            if (modelo.hlndepartamentoid.HasValue)
                modelo.departamento = dp.getdepto(modelo.hlndepartamentoid.Value).nombredept;
            if (modelo.hlnciudadid.HasValue)
                modelo.ciudad = cp.getciudad(modelo.hlnciudadid.Value).nombreciu;

            return modelo;
        }

        public List<usuario> getusuarios()
        {

            usuario usuario;
            List<usuario> usuarios = new List<usuario>();
            var modelo = context.hlnusuario.ToList();
            foreach (var item in modelo)
            {
                usuario = new usuario();
                usuario = getusuario(item.hlnusuarioid);
                usuarios.Add(usuario);
            }

            return usuarios;
        }

        public response putusuario(usuario modelo)
        {

            response response = new response();

            var new_password = modelo.password;
     
            if (!validaContraseñas(modelo))
            {
                response.valida = false;
                response.modelo = modelo;
                response.msj = "Las contraseñas no coinciden";
                return response;
            }
            if (!validaEdad(modelo.edad))
            {
                response.valida = false;
                response.modelo = modelo;
                response.msj = "La edad no es valida";
                return response;
            }
            if (validaUsername(modelo))
            {
                response.valida = false;
                response.modelo = modelo;
                response.msj = "El username ya existe.";
                return response;
            }
            if (validarol(modelo))
            {
                response.valida = false;
                response.modelo = modelo;
                response.msj = "Debe seleccionar un rol para el usuario.";
                return response;
            }


            hlnusuario usuario = context.hlnusuario.Find(modelo.hlnusuarioid);

            //si el nuevo password es diferente del md5 guardaro como password calcula de nuevo el md5 
            if(usuario.password != new_password)
            {
                usuario.password = ensure.CalculateMD5Hash(new_password);
                usuario.md5 = usuario.password;
                usuario.password2 = usuario.password;
            }

            usuario.activo = modelo.activo;
            usuario.administrador = modelo.administrador;
            usuario.alumno = modelo.alumno;
            usuario.apellidos = modelo.apellidos;
            usuario.celular = modelo.celular;
            usuario.correo = modelo.correo;
            usuario.edad = modelo.edad;
            usuario.telefono = modelo.telefono;
            usuario.hlndepartamentoid = modelo.hlndepartamentoid;
            usuario.hlnciudadid = modelo.hlnciudadid;
            usuario.hlnpaisid = modelo.hlnpaisid;
            usuario.nombres = modelo.nombres;
            usuario.descripcion = modelo.descripcion;
            
            context.Entry(usuario).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            modelo.pais = pp.getpais(modelo.hlnpaisid).nombre;
            if (modelo.hlndepartamentoid.HasValue)
                modelo.departamento = dp.getdepto(modelo.hlndepartamentoid.Value).nombredept;
            if (modelo.hlnciudadid.HasValue)
                modelo.ciudad = cp.getciudad(modelo.hlnciudadid.Value).nombreciu;

            response.valida = true;
            response.modelo = modelo;
            response.msj = "";
            return response;
        }

        public bool deleteusuario(usuario modelo)
        {
            try
            {
                var usuario = context.hlnusuario.Find(modelo.hlnusuarioid);
                context.hlnusuario.Remove(usuario);
                context.SaveChanges();
                return true;
            }
            catch { }

            return false;
        }

        public response postusuario(usuario modelo)
        {

            response response = new response();

            if (!validaContraseñas(modelo))
            {
                response.valida = false;
                response.modelo = modelo;
                response.msj = "Las contraseñas no coinciden";
                return response;
            }
            if (!validaEdad(modelo.edad))
            {
                response.valida = false;
                response.modelo = modelo;
                response.msj = "La edad no es valida";
                return response;
            }
            if (validaUsername(modelo))
            {
                response.valida = false;
                response.modelo = modelo;
                response.msj = "El username ya existe.";
                return response;
            }
            if (validarol(modelo))
            {
                response.valida = false;
                response.modelo = modelo;
                response.msj = "Debe seleccionar un rol para el usuario.";
                return response;
            }

            hlnusuario usuario = new hlnusuario()
            {
                nombres = modelo.nombres,
                apellidos = modelo.apellidos,
                descripcion = modelo.nombres + " " + modelo.apellidos,
                edad = modelo.edad,
                telefono = modelo.telefono,
                celular = modelo.celular,
                correo = modelo.correo,
                username = modelo.username,
                password = modelo.password,
                md5 = ensure.CalculateMD5Hash(modelo.password),
                hlnpaisid = modelo.hlnpaisid,
                hlndepartamentoid = modelo.hlndepartamentoid,
                hlnciudadid = modelo.hlnciudadid,
                alumno = modelo.alumno,
                administrador = modelo.administrador,
                profesor = modelo.profesor,
                activo = modelo.activo,
                password2 = ensure.CalculateMD5Hash(modelo.password2),
                fechains = DateTime.Now
            };
            context.hlnusuario.Add(usuario);
            context.SaveChanges();
            modelo.hlnusuarioid = usuario.hlnusuarioid;

            modelo.pais = pp.getpais(modelo.hlnpaisid).nombre;
            if (modelo.hlndepartamentoid.HasValue)
                modelo.departamento = dp.getdepto(modelo.hlndepartamentoid.Value).nombredept;
            if (modelo.hlnciudadid.HasValue)
                modelo.ciudad = cp.getciudad(modelo.hlnciudadid.Value).nombreciu;

            response.valida = true;
            response.modelo = modelo;
            response.msj = "";
            return response;

        }


        /// <summary>
        /// Valida el username no exista en la base de datos
        /// </summary>
        /// <param name="username">username del usuario</param>
        /// <returns>True = existe username - False = no existe username</returns>
        public bool validaUsername(usuario usuario)
        {
            int unames = 0;

            if (usuario.hlnusuarioid == 0)
            {
                unames = context.hlnusuario.Where(x => x.username.Contains(usuario.username)).Count();
            }
            else
            {
                unames = context.hlnusuario.Where(x => x.username.Contains(usuario.username) && x.hlnusuarioid != usuario.hlnusuarioid).Count();
            }


            if (unames > 0)
                return true;

            return false;
        }


        /// <summary>
        /// valida que las contraseñas coincidan para asegurarce de guardar la contraseña
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private bool validaContraseñas(usuario usuario)
        {
            usuario.password = ensure.CalculateMD5Hash(usuario.password);
            usuario.password2 = ensure.CalculateMD5Hash(usuario.password2);
            if (usuario.password == usuario.password2)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// valida que la edad del usuario cumpla con el minimo de edad para poder registrarse
        /// </summary>
        /// <param name="edad"></param>
        /// <returns></returns>
        private bool validaEdad(int edad)
        {
            var config = context.hlnconfig.FirstOrDefault();

            if (edad < config.edad)
                return false;

            return true;
        }


        /// <summary>
        /// valida que el usuario tenga por lo menos un rol
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private bool validarol(usuario usuario)
        {

            if (usuario.administrador == false && usuario.alumno == false && usuario.profesor == false)
            {
                return true;
            }

            return false;
        }



    }





}