using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hallearn.Data;
using Halliarn.Model.Model;

namespace Hallearn.Models
{


    public class homeProcesos
    {
        db_HallearnEntities context = new db_HallearnEntities();


        public List<matprogramas> getProgramas()
        {
            programaProcesos pp = new programaProcesos();
            var programas = pp.getProgramas();
            return programas;
        }




    }

}
