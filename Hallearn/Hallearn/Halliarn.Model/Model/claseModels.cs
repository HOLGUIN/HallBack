using Hallearn.Data;
using Hallearn.Model.Model;
using Hallearn.Models;
using Hallearn.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halliarn.Model.Model
{

    public class clase
    {
        public int hlnclaseid { get; set; }
        public TimeSpan horaini { get; set; }
        public TimeSpan horafin { get; set; }
        public decimal canthoras { get; set; }
        public int? calificacion { get; set; }
        public int hlnprogtemaid { get; set; }
        public DateTime fecha { get; set; }
        public int hlnusuarioid { get; set; }
        public int profesorid { get; set; }
        public decimal precio { get; set; }
    }

    public class claseProcesos
    {
        db_HallearnEntities context = new db_HallearnEntities();

        public response postClase(clase modelo)
        {
            response r = new response();

            progtemaProcesos pt = new progtemaProcesos();
            temaProcesos tp = new temaProcesos();
            var progtema = pt.getprogtema(modelo.hlnprogtemaid);
            var tema = tp.getTema(progtema.hlntemaid);

            sbyte ch = Convert.ToSByte(calculeHoras.CalcularHoras(modelo.horaini, modelo.horafin));

            hlnclase clase = new hlnclase()
            {
                hlnusuarioid = modelo.hlnusuarioid,
                hlnprogtemaid = modelo.hlnprogtemaid,
                fecha = modelo.fecha,
                horaini = modelo.horaini,
                horafin = modelo.horafin,
                canthoras = ch,
                profesorid = progtema.hlnprofesorid,
                precio = ch * tema.precio_hora
            };


            context.hlnclase.Add(clase);
            context.SaveChanges();

            r.valida = true;
            r.msj = "Se creo la clase";

            return r;
        }

        public List<clase> getClases(int hlnusuarioid)
        {
            var clases = context.hlnclase.Where(x=>x.hlnusuarioid == hlnusuarioid).Select(x => new clase
            {
                hlnclaseid = x.hlnclaseid,
                horafin = x.horafin,
                horaini = x.horaini,
                fecha = x.fecha,
                canthoras = x.canthoras,
                calificacion = x.calificacion,
                profesorid = x.profesorid,
                hlnprogtemaid = x.hlnprogtemaid,
                precio = x.precio
            }).ToList();

            return clases;

        }

    }
}
