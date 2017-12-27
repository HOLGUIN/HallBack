using System;
using System.Collections.Generic;
using System.Linq;
using Hallearn.Data;
using Hallearn.Utility;

namespace Hallearn.Model.Model
{
    class progtemaModels
    {
    }

    public class progtema
    {
        public int hlnprogtemaid { get; set; }
        public int hlntemaid { get; set; }
        public string tema { get; set; }
        public int hlnprofesorid { get; set; }
        public string profesor { get; set; }
        public TimeSpan hi { get; set; }
        public TimeSpan? hf { get; set; }
        public decimal? canthoras { get; set; }
    }


    public class progtemaProcesos
    {

        db_HallearnEntities context = new db_HallearnEntities();
        temaProcesos tp = new temaProcesos();
        usuarioProcesos up = new usuarioProcesos();

        public List<progtema> getprogtemas()
        {

            var progtemas = context.hlnprogtema.Select(x => new progtema
            {
                hlnprogtemaid = x.hlnprogtemaid,
                hlntemaid = x.hlntemaid,
                tema = x.hlntema.nombre,
                hlnprofesorid = x.hlnprofesorid,
                profesor = x.hlnusuario.descripcion,
                hi = x.horaini,
                hf = x.horafin,
                canthoras = x.canthoras

            }).ToList();
            return progtemas;
        }

        public progtema getprogtema(int hlnprogtemaid)
        {
            var progtema = context.hlnprogtema.Find(hlnprogtemaid);

            progtema modelo = new Model.progtema()
            {
                hlnprogtemaid = progtema.hlnprogtemaid,
                hlntemaid = progtema.hlntemaid,
                tema = tp.getTema(progtema.hlntemaid).nombre,
                hlnprofesorid = progtema.hlnprofesorid,
                profesor = up.getusuario(progtema.hlnprofesorid).descripcion,
                hi = progtema.horaini,
                hf = progtema.horafin,
                canthoras = progtema.canthoras
            };
            return modelo;
        }

        public response putprogtema(progtema progtema)
        {
            response response = new response();


            if (progtema.hf.HasValue && progtema.hi > progtema.hf)
            {
                response.valida = false;
                response.msj = "LNG_MSJ_7";
                response.modelo = progtema;
                return response;
            }

            progtema.canthoras = calculeHoras.CalcularHoras(progtema.hi, progtema.hf.Value);

            var modelo = context.hlnprogtema.Find(progtema.hlnprogtemaid);


            modelo.hlntemaid = progtema.hlntemaid;
            modelo.hlnprofesorid = progtema.hlnprofesorid;
            modelo.horaini = progtema.hi;
            modelo.horafin = progtema.hf;
            modelo.canthoras = progtema.canthoras;

            context.Entry(modelo).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            progtema.tema = tp.getTema(modelo.hlntemaid).nombre;
            progtema.profesor = up.getusuario(modelo.hlnprofesorid).descripcion;

            response.valida = true;
            response.msj = "";
            response.modelo = progtema;
            return response;

        }

        public response postprogtema(progtema progtema)
        {
            response response = new response();

            if (progtema.hf.HasValue && progtema.hi > progtema.hf)
            {
                response.valida = false;
                response.msj = "LNG_MSJ_7";
                response.modelo = progtema;
                return response;
            }

            progtema.canthoras = calculeHoras.CalcularHoras(progtema.hi, progtema.hf.Value);

            hlnprogtema modelo = new hlnprogtema()
            {
                hlntemaid = progtema.hlntemaid,
                hlnprofesorid = progtema.hlnprofesorid,
                horaini = progtema.hi,
                horafin = progtema.hf,
                canthoras = progtema.canthoras
            };

            context.hlnprogtema.Add(modelo);
            context.SaveChanges();

            //termina de llenar el modelo para retornarlo al backend
            progtema.hlnprogtemaid = modelo.hlnprogtemaid;
            progtema.tema = tp.getTema(modelo.hlntemaid).nombre;
            progtema.profesor = up.getusuario(modelo.hlnprofesorid).descripcion;

            response.valida = true;
            response.msj = "";
            response.modelo = progtema;
            return response;
        }

        public bool deleteprogtema(progtema progtema)
        {
            try
            {
                var modelo = context.hlnprogtema.Find(progtema.hlnprogtemaid);
                context.hlnprogtema.Remove(modelo);
                context.SaveChanges();

                return true;
            }
            catch { }
            return false;
        }


    }
}
