using Hallearn.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hallearn.Model.Model
{

    public class matprogramas
    {
        public string materia { get; set; }
        public int hlnmateriaid { get; set; }    
        public List<temas> temas { get; set; }
    }

    public class temas
    {
        public string tema { get; set; }
        public string desctema { get; set; }
        public decimal preciohora { get; set; }
        public int hlnprogtemaid { get; set; }
        public List<horario> horarios { get; set; }
    }

    public class horario
    {
        public TimeSpan horaini { get; set; }
        public TimeSpan? horafin { get; set; }
        public int hlnprogtemaid { get; set; }
    }

    public class programaProcesos
    {


        db_HallearnEntities context = new db_HallearnEntities();


        public List<matprogramas> getProgramas()
        {
            var progtemas = context.hlnprogtema.ToList();

            var hlnmaterias = progtemas.Select(x => x.hlntema.hlnmateriaid).ToList();
            hlnmaterias = hlnmaterias.Distinct().ToList();

            List<matprogramas> pt = new List<matprogramas>();

            foreach (var item in hlnmaterias)
            {

                var modelo = progtemas.Where(x => x.hlntema.hlnmateriaid == item).ToList();
                var hlntemasids = modelo.Select(x => x.hlntemaid).Distinct().ToList();

                matprogramas mp = new matprogramas();
                mp.hlnmateriaid = modelo.FirstOrDefault().hlntema.hlnmateriaid;
                mp.materia = modelo.FirstOrDefault().hlntema.hlnmateria.nombre;
                mp.temas = new List<temas>();

                foreach (var temaid in hlntemasids)
                {
                    var temas = modelo.Where(x => x.hlntemaid == temaid);
                    temas t = new temas();
                    t.tema = temas.FirstOrDefault().hlntema.nombre;
                    t.preciohora = temas.FirstOrDefault().hlntema.preciohora;
                    t.desctema = temas.FirstOrDefault().hlntema.descripcion;
                    t.horarios = new List<horario>();

                    var hs = modelo.Where(x=>x.hlntemaid == temaid).ToList();

                    foreach (var horario in hs)
                    {
                        horario h = new Model.horario();
                        h.hlnprogtemaid = horario.hlnprogtemaid;
                        h.horaini = horario.horaini;
                        h.horafin = horario.horafin;
                        t.horarios.Add(h);
                    }

                    mp.temas.Add(t);
                }

                pt.Add(mp);
            }

            return pt;
        }

    }


}
