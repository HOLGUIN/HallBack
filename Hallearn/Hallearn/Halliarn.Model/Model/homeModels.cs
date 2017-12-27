using System.Collections.Generic;
using Hallearn.Data;

namespace Hallearn.Model.Model
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
