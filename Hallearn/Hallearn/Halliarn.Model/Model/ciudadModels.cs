using System.Collections.Generic;
using System.Linq;
using Hallearn.Data;
using Hallearn.Model.Model;
using Hallearn.Utility;

namespace Hallearn.Models
{
    public class ciudad
    {
        public int hlnciudadid { get; set; }
        public int hlndepartamentoid { get; set; }
        public sbyte hlnpaisid { get; set; }
        public string nombreciu { get; set; }
        public string nombredept { get; set; }
        public string nombrepais { get; set; }
        public string codigo { get; set; }
        public int? codigopost { get; set; }
    }


    public class ciudadProcesos
    {


        db_HallearnEntities context = new db_HallearnEntities();
        paisProceso pp = new paisProceso();
        deptProcesos dp = new deptProcesos();

        public List<ciudad> getciudades()
        {
            var ciudades = context.hlnciudad.Select(x => new ciudad
            {
                hlnciudadid = x.hlnciudadid,
                hlndepartamentoid = x.hlndepartamentoid,
                hlnpaisid = x.hlndepartamento.hlnpaisid,
                nombreciu = x.nombre,
                nombredept = x.hlndepartamento.nombre,
                nombrepais = x.hlndepartamento.hlnpais.nombre,
                codigo = x.codigo,
                codigopost = x.codigopost
            }).ToList();

            return ciudades;
        }

        public ciudad getciudad(int hlnciudadid)
        {
            var ciudad = context.hlnciudad.Find(hlnciudadid);

            ciudad modelo = new ciudad()
            {
                hlnciudadid = ciudad.hlnciudadid,
                hlndepartamentoid = ciudad.hlndepartamentoid,
                hlnpaisid = ciudad.hlndepartamento.hlnpaisid,
                nombreciu = ciudad.nombre,
                codigopost = ciudad.codigopost,
                codigo = ciudad.codigo,
                nombredept = ciudad.hlndepartamento.nombre,
                nombrepais = ciudad.hlndepartamento.hlnpais.nombre
            };

            return modelo;
        }

        public response putciudad(ciudad ciudad)
        {
            response response = new response();


            if (!validaNomciudad(ciudad.nombreciu, ciudad.hlndepartamentoid, ciudad.hlnciudadid))
            {
                response.valida = false;
                response.msj = "Ya existe un departamento con ese nombre para este pais.";
                response.modelo = ciudad;
                return response;
            }

            var modelo = context.hlnciudad.Find(ciudad.hlnciudadid);


            modelo.hlndepartamentoid = ciudad.hlndepartamentoid;
            modelo.codigo = ciudad.codigo;
            modelo.nombre = ciudad.nombreciu;
            modelo.codigopost = ciudad.codigopost;

            context.Entry(modelo).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            ciudad.nombrepais = pp.getpais(ciudad.hlnpaisid).nombre;
            ciudad.nombredept = dp.getdepto(ciudad.hlndepartamentoid).nombredept;

            response.valida = true;
            response.msj = "";
            response.modelo = ciudad;
            return response;
            
        }

        public response postdepto(ciudad ciudad)
        {
            response response = new response();

            if (!validaNomciudad(ciudad.nombreciu, ciudad.hlndepartamentoid, null))
            {
                response.valida = false;
                response.msj = "Ya existe una ciudad con ese nombre para este deparatamento.";
                response.modelo = ciudad;
                return response;
            }

            hlnciudad modelo = new hlnciudad()
            {
                codigo = ciudad.codigo,
                nombre = ciudad.nombreciu,
                hlndepartamentoid = ciudad.hlndepartamentoid,
                codigopost = ciudad.codigopost
            };

            context.hlnciudad.Add(modelo);
            context.SaveChanges();
    
            //termina de llenar el modelo para retornarlo al backend
            ciudad.hlnciudadid = modelo.hlnciudadid;

            var departamento = dp.getdepto(modelo.hlndepartamentoid);

            ciudad.nombrepais = pp.getpais(departamento.hlnpaisid).nombre;
            ciudad.nombredept = departamento.nombredept;

            response.valida = true;
            response.msj = "";
            response.modelo = ciudad;
            return response;
        }

        public bool deleteciudad(ciudad ciudad)
        {
            try
            {
                var modelo = context.hlnciudad.Find(ciudad.hlnciudadid);
                context.hlnciudad.Remove(modelo);
                context.SaveChanges();

                return true;
            }
            catch { }
            return false;
        }

        private bool validaNomciudad(string nombre, int hlndeptoid, int? hlnciudadid)
        {

            int nomciud = 0;
            if (hlnciudadid.HasValue)
            {
                nomciud = context.hlnciudad.Where(x => x.nombre.ToUpper().Contains(nombre.ToUpper()) && x.hlndepartamentoid == hlndeptoid && x.hlnciudadid != hlnciudadid).Count();
            }
            else
            {
                nomciud = context.hlnciudad.Where(x => x.nombre.ToUpper().Contains(nombre.ToUpper()) && x.hlndepartamentoid == hlndeptoid).Count();
            }

            if (nomciud > 0)
                return false;

            return true;

        }
    }



}