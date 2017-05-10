using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hallearn.Data;

namespace Hallearn.Utility
{
    public static class customList
    {

        public static IEnumerable getPaises()
        {
            db_HallearnEntities context = new db_HallearnEntities();

            IEnumerable paises = context.hlnpais.ToList().Select(x => new
            {
                Value = x.hlnpaisid.ToString(),
                Text = x.nombre
            });
            return paises;
        }

        public static IEnumerable getDepartamentos()
        {
            db_HallearnEntities context = new db_HallearnEntities();
            IEnumerable departamentos = context.hlndepartamento.ToList().Select(x => new
            {
                Value = x.hlndepartamentoid.ToString(),
                Text = x.nombre,
                Group = x.hlnpais.hlnpaisid.ToString()
            });
            return departamentos;
        }

        public static IEnumerable getCiudades()
        {
            db_HallearnEntities context = new db_HallearnEntities();
            IEnumerable ciudades = context.hlnciudad.ToList().Select(x => new
            {
                Value = x.hlnciudadid.ToString(),
                Text = x.nombre,
                Group = x.hlndepartamentoid.ToString()
            });
            return ciudades;
        }

        public static IEnumerable getMaterias()
        {
            db_HallearnEntities context = new db_HallearnEntities();
            IEnumerable materias = context.hlnmateria.ToList().Select(x => new
            {
                Value = x.hlnmateriaid,
                Text = x.nombre
            });

            return materias;
        }

        public static IEnumerable getProfesores()
        {

            db_HallearnEntities context = new db_HallearnEntities();
            IEnumerable profesores = context.hlnusuario.Where(x => x.activo == true && x.profesor == true).ToList().Select(x => new
            {
                Value = x.hlnusuarioid.ToString(),
                Text = x.descripcion
            });

            return profesores;
        }

        public static IEnumerable getTemas()
        {
            db_HallearnEntities context = new db_HallearnEntities();
            IEnumerable temas = context.hlntema.Select(x => new
            {
                Value = x.hlntemaid.ToString(),
                Text = x.nombre,
                Group = x.hlnmateriaid.ToString()
            });

            return temas;
        }

    }
}