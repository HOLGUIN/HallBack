using Hallearn.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hallearn.Data;
using Hallearn.Model.Model;

namespace Hallearn.Model.Model
{
    public class tema
    {
        public int hlntemaid { get; set; }
        public string nombre { get; set; }
        public sbyte hlnmateriaid { get; set; }
        public string materia { get; set; }
        public decimal precio_hora { get; set; }
        public string descripcion { get; set; }
    }

    public class temaProcesos
    {
        db_HallearnEntities context = new db_HallearnEntities();
        materiaProcesos mp = new materiaProcesos();

        public List<tema> getTemas()
        {

            var temas = context.hlntema.Select(x=> new tema {
                hlntemaid = x.hlntemaid,
                nombre = x.nombre,
                hlnmateriaid = x.hlnmateriaid,
                materia = x.hlnmateria.nombre,
                precio_hora = x.preciohora,
                descripcion = x.descripcion
            }).ToList();
         
            return temas;
        }

        public tema getTema(int hlntemaid)
        {
            var tema = context.hlntema.Find(hlntemaid);

            tema modelo = new tema()
            {
                hlntemaid = tema.hlntemaid,
                nombre = tema.nombre,
                hlnmateriaid = tema.hlnmateriaid,
                materia = tema.hlnmateria.nombre,
                precio_hora = tema.preciohora,
                descripcion = tema.descripcion
            };

            return modelo;
        }

        public response putTema(tema tema)
        {
            response response = new response();

            if (!validaNomtema(tema.nombre, tema.hlntemaid))
            {
                response.valida = false;
                response.msj = "LNG_MSJ_5";
                response.modelo = tema;
                return response;
            }

            var modelo = context.hlntema.Find(tema.hlntemaid);
            modelo.preciohora = tema.precio_hora;
            modelo.nombre = tema.nombre;
            modelo.hlnmateriaid = tema.hlnmateriaid;
            modelo.descripcion = tema.descripcion;
            context.Entry(modelo).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            tema.materia = mp.getmateria(tema.hlnmateriaid).nombre;

            response.valida = true;
            response.msj = "";
            response.modelo = tema;
            return response;

        }

        public response postTema(tema tema)
        {
            response response = new response();

            if (!validaNomtema(tema.nombre, null))
            {
                response.valida = false;
                response.msj = "LNG_MSJ_5";
                response.modelo = tema;
                return response;
            }

            hlntema modelo = new hlntema()
            {
                nombre = tema.nombre,
                descripcion = tema.descripcion,
                hlnmateriaid = tema.hlnmateriaid,
                preciohora = tema.precio_hora
            };

            context.hlntema.Add(modelo);
            context.SaveChanges();
            //termina de llenar el modelo para retornarlo al backend
            tema.hlntemaid = modelo.hlntemaid;
            tema.materia = mp.getmateria(modelo.hlnmateriaid).nombre;

            response.valida = true;
            response.msj = "";
            response.modelo = tema;
            return response;
        }

        public bool deletetema(tema tema)
        {
            try
            {
                var modelo = context.hlntema.Find(tema.hlntemaid);
                context.hlntema.Remove(modelo);
                context.SaveChanges();

                return true;
            }
            catch { }
            return false;
        }

        private bool validaNomtema(string nombre, int? hlntemaid)
        {

            int nomtemas = 0;
            if (hlntemaid.HasValue)
            {
                nomtemas = context.hlntema.Where(x => x.nombre.ToUpper().Contains(nombre.ToUpper()) && x.hlntemaid != hlntemaid).Count();
            }
            else
            {
                nomtemas = context.hlntema.Where(x => x.nombre.ToUpper().Contains(nombre.ToUpper())).Count();
            }

            if (nomtemas > 0)
                return false;

            return true;

        }

    }

}