using System.Collections.Generic;
using System.Linq;
using Hallearn.Data;
using Hallearn.Utility;

namespace Hallearn.Models
{
    public class pais
    {
        public sbyte hlnpaisid { get; set; }
        public string nombre { get; set; }
        public string codigo { get; set; }
    }



    public class paisProceso
    {

        db_HallearnEntities context = new db_HallearnEntities();

        public List<pais> getpaises()
        {
            var paises = context.hlnpais.Select(x=> new pais {
                hlnpaisid = x.hlnpaisid,
                codigo = x.codigo,
                nombre = x.nombre
            }).ToList();
   
            return paises;
        }

        public pais getpais(sbyte hlnpaisid)
        {
            var pais = context.hlnpais.Find(hlnpaisid);

            pais modelo = new pais()
            {
                hlnpaisid = pais.hlnpaisid,
                codigo = pais.codigo,
                nombre = pais.nombre
            };

            return modelo;
        }

        public response postpais(pais pais)
        {
            response response = new response();

            if (!validaNompais(pais.nombre, null))
            {
                response.valida = false;
                response.msj = "LNG_MSJ_5";
                response.modelo = pais;
                return response;
            }

            hlnpais modelo = new hlnpais()
            {
                nombre = pais.nombre,
                codigo = pais.codigo    
            };

            context.hlnpais.Add(modelo);
            context.SaveChanges();

            pais.hlnpaisid = modelo.hlnpaisid;

            response.valida = true;
            response.msj = "";
            response.modelo = pais;
            return response;
        }

        public response putpais(pais pais)
        {
            response response = new response();

            if (!validaNompais(pais.nombre, pais.hlnpaisid))
            {
                response.valida = false;
                response.msj = "LNG_MSJ_5";
                response.modelo = pais;
                return response;
            }

            var modelo = context.hlnpais.Find(pais.hlnpaisid);
            modelo.codigo = pais.codigo;
            modelo.nombre = pais.nombre;
            context.Entry(modelo).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();


            response.valida = true;
            response.msj = "";
            response.modelo = pais;
            return response;
        }

        public bool deletepais(pais pais)
        {
            try
            {
                var modelo = context.hlnpais.Find(pais.hlnpaisid);
                context.hlnpais.Remove(modelo);
                context.SaveChanges();
                return true;
            }
            catch { }
            return false;
        }

        public bool validaNompais(string nombre, int? hlnpaisid)
        {

            int paisnom = 0;
            if (hlnpaisid.HasValue)
            {
                paisnom = context.hlnpais.Where(x => x.nombre.ToUpper().Contains(nombre.ToUpper()) && x.hlnpaisid != hlnpaisid).Count();
            }
            else
            {
                paisnom = context.hlnpais.Where(x => x.nombre.ToUpper().Contains(nombre.ToUpper())).Count();
            }

            if (paisnom > 0)
                return false;

            return true;

        }

    }

}