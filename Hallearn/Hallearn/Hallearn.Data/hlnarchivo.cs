//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hallearn.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class hlnarchivo
    {
        public int hlnarchivoid { get; set; }
        public string titulo { get; set; }
        public string filename { get; set; }
        public string extension { get; set; }
        public int hlnclaseid { get; set; }
    
        public virtual hlnclase hlnclase { get; set; }
    }
}
