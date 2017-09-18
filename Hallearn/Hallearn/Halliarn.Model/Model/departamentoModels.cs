using System.Collections.Generic;
using System.Linq;
using Hallearn.Data;
using Hallearn.Utility;

namespace Hallearn.Models
{
    public class departamento
    {
        public int hlndepartamentoid { get; set; }
        public sbyte hlnpaisid { get; set; }
        public string nombredept { get; set; }
        public string nombrepais { get; set; }
        public string codigo { get; set; }
    }


    public class deptProcesos
    {
        db_HallearnEntities context = new db_HallearnEntities();
        paisProceso pp = new paisProceso();
        // materiaProcesos mp = new materiaProcesos();

        public List<departamento> getdeptos()
        {
       
            var modelo = context.hlndepartamento.Select(x=>new departamento {
                hlndepartamentoid = x.hlndepartamentoid,
                hlnpaisid = x.hlnpaisid,
                codigo = x.codigo,
                nombredept = x.nombre,
                nombrepais = x.hlnpais.nombre
            }).ToList();

            return modelo;
        }

        public departamento getdepto(int hlndepartamentoid)
        {
            var dept = context.hlndepartamento.Find(hlndepartamentoid);


            departamento modelo = new departamento()
            {
                hlndepartamentoid = dept.hlndepartamentoid,
                hlnpaisid = dept.hlnpaisid,
                codigo = dept.codigo,
                nombredept = dept.nombre,
                nombrepais = dept.hlnpais.nombre
            };

            return modelo;
        }

        public response putdepto(departamento depto)
        {
            response response = new response();


            if (!validaNomdepto(depto.nombredept, depto.hlnpaisid, depto.hlndepartamentoid))
            {
                response.valida = false;
                response.msj = "LNG_MSJ_8";
                response.modelo = depto;
                return response;
            }

            var modelo = context.hlndepartamento.Find(depto.hlndepartamentoid);
            modelo.hlnpaisid = depto.hlnpaisid;
            modelo.codigo = depto.codigo;
            modelo.nombre = depto.nombredept;
            context.Entry(modelo).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            depto.nombrepais = pp.getpais(depto.hlnpaisid).nombre;

            response.valida = true;
            response.msj = "";
            response.modelo = depto;
            return response;

        }

        public response postdepto(departamento depto)
        {
            response response = new response();

            if (!validaNomdepto(depto.nombredept, depto.hlnpaisid, null))
            {
                response.valida = false;
                response.msj = "LNG_MSJ_8";
                response.modelo = depto;
                return response;
            }


            hlndepartamento modelo = new hlndepartamento()
            {
                codigo = depto.codigo,
                nombre = depto.nombredept,
                hlnpaisid = depto.hlnpaisid
            };

            context.hlndepartamento.Add(modelo);
            context.SaveChanges();
            //termina de llenar el modelo para retornarlo al backend
            depto.hlndepartamentoid = modelo.hlndepartamentoid;
            depto.nombrepais = pp.getpais(modelo.hlnpaisid).nombre;

            response.valida = true;
            response.msj = "";
            response.modelo = depto;
            return response;
        }

        public bool deletedepto(departamento depto)
        {
            try
            {
                var modelo = context.hlndepartamento.Find(depto.hlndepartamentoid);
                context.hlndepartamento.Remove(modelo);
                context.SaveChanges();

                return true;
            }
            catch { }
            return false;
        }

        private bool validaNomdepto(string nombre, int hlnpaisid, int? hlndeptoid)
        {

            int nomdepto = 0;
            if (hlndeptoid.HasValue)
            {
                nomdepto = context.hlndepartamento.Where(x => x.nombre.ToUpper().Contains(nombre.ToUpper()) && x.hlnpaisid == hlnpaisid && x.hlndepartamentoid != hlndeptoid).Count();
            }
            else
            {
                nomdepto = context.hlndepartamento.Where(x => x.nombre.ToUpper().Contains(nombre.ToUpper()) && x.hlnpaisid == hlnpaisid).Count();
            }

            if (nomdepto > 0)
                return false;

            return true;

        }
    }


}