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
        public TimeSpan horaini { get; set; }
        public TimeSpan horafin { get; set; }
    }


    public class clasesasgProcesos
    {

        db_HallearnEntities context = new db_HallearnEntities();

        public List<claseasg> getClasesasg(int hlnprogtemaid, DateTime fecha)
        {
            var claseasg = context.hlnclase.Where(x => x.hlnprogtemaid == hlnprogtemaid && x.fecha == fecha.Date)
                .Select(x => new claseasg
                {
                    horaini = x.horaini,
                    horafin = x.horafin
                }).ToList();

            return claseasg;
        }


    }

}
