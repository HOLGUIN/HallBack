using Hallearn.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hallearn.Model.Model
{
    public class claseasg
    {
        public bool busy { get; set; }
        public TimeSpan horaini { get; set; }
        public TimeSpan horafin { get; set; }
    }

    public class linetime
    {
        public string hora { get; set; }
        public string class_ball { get; set; }
        public string class_line { get; set; }
        public bool val { get; set; }
    }

    public class clasesasgProcesos
    {

        db_HallearnEntities context = new db_HallearnEntities();

        private List<claseasg> getbusyclass(int hlnprogtemaid, DateTime fecha)
        {
            var claseasg = context.hlnclase.Where(x => x.hlnprogtemaid == hlnprogtemaid && x.fecha == fecha.Date)
                .Select(x => new claseasg
                {
                    horaini = x.horaini,
                    horafin = x.horafin,
                    busy = true
                }).ToList();

            return claseasg;
        }

        public List<claseasg> getClasesasg(int hlnprogtemaid, DateTime fecha)
        {
            var claseasg = getbusyclass(hlnprogtemaid, fecha.Date);

            var aux = new List<claseasg>();

            var progtema = context.hlnprogtema.Find(hlnprogtemaid);

            TimeSpan time1 = TimeSpan.FromHours(1);
            List<claseasg> allclass = new List<Model.claseasg>();
            for (TimeSpan i = progtema.horaini; i < progtema.horafin; i = i.Add(time1))
            {
                claseasg modelo = new Model.claseasg()
                {
                    horaini = i,
                    horafin = i.Add(time1),
                    busy = false
                };
                aux.Add(modelo);
            }


            if (claseasg.Count() > 0)
            {
                foreach (var item in claseasg)
                {
                   // aux.Where(x => x.horaini >= item.horaini && x.horafin <= item.horafin).ToList();
                    foreach (var a in aux)
                    {
                        if (a.horaini >= item.horaini && a.horafin <= item.horafin)
                        {
                            a.busy = item.busy;
                        }
                        
                    }

                }
            }

            return aux.OrderBy(x => x.horaini).ToList();
        }


        public List<linetime> getClass(int hlnprogtemaid, DateTime fecha)
        {
            string active = "active";
            string inactive = "inactive";
            string start = "start";
            string inactive_start = "inactive-start";
            string medium_rigth = "medium_rigth";
            string medium_left = "medium_left ";

            var hlnprograma = context.hlnprogtema.Find(hlnprogtemaid);
            var clasesprog = getbusyclass(hlnprogtemaid, fecha);
            List<linetime> modelo = new List<linetime>();

            TimeSpan? aux = null;
            TimeSpan time1 = TimeSpan.FromHours(1);
            for (TimeSpan i = hlnprograma.horaini; i <= hlnprograma.horafin; i = i.Add(time1))
            {
                linetime lt = new linetime();

                lt.class_ball = start;
                lt.class_line = active;

                foreach (var item in clasesprog)
                {
                    if (i == item.horaini)
                    {
                        if (aux != null && aux == item.horaini)
                        {
                            lt.class_ball = inactive_start;
                            lt.class_line = inactive;
                        }
                        else
                        {
                            lt.class_ball = medium_rigth;
                            lt.class_line = active;
                        }
                    }
                    else if (i == item.horafin)
                    {
                        lt.class_ball = medium_left;
                        lt.class_line = inactive;
                        aux = item.horafin;
                    }
                    else if (i > item.horaini && i < item.horafin)
                    {
                        lt.class_ball = inactive_start;
                        lt.class_line = inactive;
                    }
                }
                lt.hora = i.ToString();
                modelo.Add(lt);
            }


            return modelo;

        }

    }

}
