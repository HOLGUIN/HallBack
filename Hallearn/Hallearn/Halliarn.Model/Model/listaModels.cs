using System.Web.Mvc;

namespace Hallearn.Models
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
                Listas.paises = new SelectList(Utility.customList.getPaises(), "Value", "Text", null);
            }
            if (depts.HasValue && depts.Value)
            {
                Listas.depts = new SelectList(Utility.customList.getDepartamentos(), "Value", "Text", "Group", null, null);
            }
            if (ciudades.HasValue && ciudades.Value)
            {
                Listas.ciudades = new SelectList(Utility.customList.getCiudades(), "Value", "Text", "Group", null, null);
            }
            if (materias.HasValue && materias.Value)
            {
                Listas.materias = new SelectList(Utility.customList.getMaterias(), "Value", "Text", null);
            }
            if (profes.HasValue && profes.Value)
            {
                Listas.profesores = new SelectList(Utility.customList.getProfesores(), "Value", "Text", null);
            }
            if (temas.HasValue && temas.Value)
            {
                Listas.temas = new SelectList(Utility.customList.getTemas(), "Value", "Text", "Group", null, null);
            }

            return Listas;
        }

    }

}