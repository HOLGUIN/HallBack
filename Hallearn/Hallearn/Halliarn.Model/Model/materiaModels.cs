using System.Collections.Generic;
using System.Linq;
using Hallearn.Data;
using Hallearn.Utility;

namespace Hallearn.Model.Model
{
    public class materia
    {
        public sbyte hlnmateriaid { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
    }


    public class materiaProcesos
    {

        db_HallearnEntities context = new db_HallearnEntities();

        public List<materia> getmaterias()
        {
            var materias = context.hlnmateria.Select(x=>new materia {
                hlnmateriaid = x.hlnmateriaid,
                nombre = x.nombre,
                descripcion = x.descripcion
            }).ToList();

            return materias;
        }

        public materia getmateria(sbyte hlnmateriaid)
        {
            var materia = context.hlnmateria.Find(hlnmateriaid);

            materia modelo = new materia()
            {
                hlnmateriaid = materia.hlnmateriaid,
                nombre = materia.nombre,
                descripcion = materia.descripcion
            };

            return modelo;
        }

        public response postmateria(materia materia)
        {
            response response = new response();

            if (!validaNommateria(materia.nombre, null))
            {
                response.valida = false;
                response.msj = "LNG_MSJ_5";
                response.modelo = materia;
                return response;
            }

            hlnmateria modelo = new hlnmateria()
            {
                nombre = materia.nombre,
                descripcion = materia.descripcion
            };

            context.hlnmateria.Add(modelo);
            context.SaveChanges();

            materia.hlnmateriaid = modelo.hlnmateriaid;

            response.valida = true;
            response.msj = "";
            response.modelo = materia;
            return response;
        }

        public response putmateria(materia materia)
        {
            response response = new response();

            if (!validaNommateria(materia.nombre, materia.hlnmateriaid))
            {
                response.valida = false;
                response.msj = "LNG_MSJ_5";
                response.modelo = materia;
                return response;
            }

            var modelo = context.hlnmateria.Find(materia.hlnmateriaid);
            modelo.descripcion = materia.descripcion;
            modelo.nombre = materia.nombre;
            context.Entry(modelo).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();


            response.valida = true;
            response.msj = "";
            response.modelo = materia;
            return response;
        }

        public bool deletemateria(materia materia)
        {
            try
            {
                var modelo = context.hlnmateria.Find(materia.hlnmateriaid);
                context.hlnmateria.Remove(modelo);
                context.SaveChanges();
                return true;
            }
            catch { }
            return false;
        }

        public bool validaNommateria(string nombre, int? hlnmateriaid)
        {

            int matnom = 0;
            if (hlnmateriaid.HasValue)
            {
                matnom = context.hlnmateria.Where(x => x.nombre.ToUpper().Contains(nombre.ToUpper()) && x.hlnmateriaid != hlnmateriaid).Count();
            }
            else
            {
                matnom = context.hlnmateria.Where(x => x.nombre.ToUpper().Contains(nombre.ToUpper())).Count();
            }

            if (matnom > 0)
                return false;

            return true;

        }


    }



}