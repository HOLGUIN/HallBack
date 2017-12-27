using System.Web.Mvc;
using Hallearn.Utility;

namespace Hallearn.Model.Model
{
    public class Lista
    {
        public SelectList modelo { get; set; }
        public string lista { get; set; }
    }

    public class Listas
    {
        public SelectList paises { get; set; }
        public SelectList depts { get; set; }
        public SelectList ciudades { get; set; }
        public SelectList materias { get; set; }
        public SelectList profesores { get; set; }
        public SelectList temas { get; set; }
    }


    public class listasProcesos
    {

        public Listas getlistas(bool? paises, bool? depts, bool? ciudades, bool? materias, bool? profes, bool? temas)
        {


            Listas Listas = new Listas();

            if (paises.HasValue && paises.Value)
            {
                Listas.paises = new SelectList(Hallearn.Utility.customList.getPaises(), "Value", "Text", null);
            }
            if (depts.HasValue && depts.Value)
            {
                Listas.depts = new SelectList(Hallearn.Utility.customList.getDepartamentos(), "Value", "Text", "Group", null, null);
            }
            if (ciudades.HasValue && ciudades.Value)
            {
                Listas.ciudades = new SelectList(Hallearn.Utility.customList.getCiudades(), "Value", "Text", "Group", null, null);
            }
            if (materias.HasValue && materias.Value)
            {
                Listas.materias = new SelectList(Hallearn.Utility.customList.getMaterias(), "Value", "Text", null);
            }
            if (profes.HasValue && profes.Value)
            {
                Listas.profesores = new SelectList(Hallearn.Utility.customList.getProfesores(), "Value", "Text", null);
            }
            if (temas.HasValue && temas.Value)
            {
                Listas.temas = new SelectList(Hallearn.Utility.customList.getTemas(), "Value", "Text", "Group", null, null);
            }

            return Listas;
        }

    }

}