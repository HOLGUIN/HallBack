//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hallearn.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class hlnvideoclase
    {
        public int hlnvideoclaseid { get; set; }
        public int hlnusuarioid { get; set; }
        public int hlnclaseid { get; set; }
        public string descripcion { get; set; }
    
        public virtual hlnclase hlnclase { get; set; }
        public virtual hlnusuario hlnusuario { get; set; }
    }
}
