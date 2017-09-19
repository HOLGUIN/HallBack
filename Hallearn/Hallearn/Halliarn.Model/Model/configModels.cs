using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hallearn.Data;

namespace Hallearn.Model.Model
{
    public class config
    {
        public int hlnconfigid { get; set; }
        public int edad { get; set; }
    }


    public class configProcesos
    {

        db_HallearnEntities context = new db_HallearnEntities();

        public config getconfig()
        {
            config modelo = new config();

            var config = context.hlnconfig.FirstOrDefault();
            if(config != null)
            {
                modelo.hlnconfigid = config.hlnconfigid;
                modelo.edad = config.edad.Value;
            }

            return modelo;
        }

        public config putconfig(config config)
        {

            try
            {
                var modelo = context.hlnconfig.Find(config.hlnconfigid);

                if(modelo == null)
                {
                    hlnconfig configm = new hlnconfig();
                    configm.edad = config.edad;
                    context.hlnconfig.Add(configm);
                }
                else
                {
                    modelo.edad = config.edad;
                    context.Entry(modelo).State = System.Data.Entity.EntityState.Modified; 
                }
                context.SaveChanges();
                return config;
            }
            catch { }

            return config;
        }
    }



}
