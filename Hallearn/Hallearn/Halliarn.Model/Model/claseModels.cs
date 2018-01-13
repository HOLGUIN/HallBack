using Hallearn.Data;
using Hallearn.Model.Model;

using Hallearn.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hallearn.Model.Model
{

    public class clase : clase_basic
    {
        public TimeSpan horaini { get; set; }
        public TimeSpan horafin { get; set; }
        public int? calificacion { get; set; }
        public int hlnprogtemaid { get; set; }
        public int hlnusuarioid { get; set; }
        public int profesorid { get; set; }
        public decimal precio { get; set; }
    }

    public class clase_basic
    {
        public int hlnclaseid { get; set; }
        public decimal canthoras { get; set; }
        public DateTime fecha { get; set; }
        public int hlnmateriaid { get; set; }
        public int hlntemaid { get; set; }
    }

    public class clases_lista
    {
        public List<clases_vistas> clases_vistas { get; set; }
    }

    public class clases_vistas
    {
        public string materia { get; set; }
        public List<clases_temas> temas { get; set; }
    }

    public class clases_temas
    {
        public string tema { get; set; }
        public string css_class { get; set; }
        public List<clase_basic> clases { get; set; }
    }

    public class claseProcesos
    {
        db_HallearnEntities context = new db_HallearnEntities();
        private List<string> css_class = new List<string>()
        {
            "dark-grey",
            "dark-lime",
            "dark-orange",
            "dark-teal",
            "dark-amber",
            "dark-brown",
            "dark-indigo",
            "dark-pink"
        };

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

        public clases_lista getClases(int hlnusuarioid)
        {
            clases_lista modelo = new clases_lista();
            modelo.clases_vistas = new List<clases_vistas>();
            // Random rmdn = new Random();
            var clases = context.hlnclase.Include("hlnprogtema").Where(x => x.hlnusuarioid == hlnusuarioid).Select(x => new clase_basic
            {
                hlnclaseid = x.hlnclaseid,
                canthoras = x.canthoras,
                fecha = x.fecha,
                hlnmateriaid = x.hlnprogtema.hlntema.hlnmateriaid,
                hlntemaid = x.hlnprogtema.hlntema.hlntemaid
            }).ToList();

           // modelo.clases_activas = clases.Where(x => x.fecha > DateTime.Now).ToList();

            var materias = clases.Where(x => x.fecha < DateTime.Now.Date).Select(x => x.hlnmateriaid).Distinct();
            db_HallearnEntities db2 = new db_HallearnEntities();
            foreach (var item in materias)
            {
                var temas = db2.hlntema.Where(x => x.hlnmateriaid == item).ToList();
                clases_vistas cv = new clases_vistas();
                cv.materia = db2.hlnmateria.Find(Convert.ToSByte(item)).nombre;
                cv.temas = new List<clases_temas>();
                foreach (var t in temas)
                {
                    var tems = clases.Where(x => x.hlntemaid == t.hlntemaid).ToList();

                    if (tems.Count() > 0)
                    {
                        clases_temas ct = new clases_temas()
                        {
                            tema = t.nombre,
                            clases = tems
                        };

                       // ct.css_class = css_class[rmdn.Next(css_class.Count())];

                        cv.temas.Add(ct);
                    }
                }
                modelo.clases_vistas.Add(cv);
            }

            db2.Dispose();
            return modelo;

        }

    }
}
