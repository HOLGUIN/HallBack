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
    
    public partial class hlnusuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public hlnusuario()
        {
            this.hlnclase = new HashSet<hlnclase>();
            this.hlnvideoclase = new HashSet<hlnvideoclase>();
            this.hlnprogtema = new HashSet<hlnprogtema>();
        }
    
        public int hlnusuarioid { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string telefono { get; set; }
        public string celular { get; set; }
        public string correo { get; set; }
        public sbyte hlnpaisid { get; set; }
        public Nullable<int> hlndepartamentoid { get; set; }
        public Nullable<int> hlnciudadid { get; set; }
        public sbyte edad { get; set; }
        public string md5 { get; set; }
        public Nullable<bool> activo { get; set; }
        public Nullable<bool> administrador { get; set; }
        public Nullable<bool> profesor { get; set; }
        public Nullable<bool> alumno { get; set; }
        public string descripcion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<hlnclase> hlnclase { get; set; }
        public virtual hlnpais hlnpais { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<hlnvideoclase> hlnvideoclase { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<hlnprogtema> hlnprogtema { get; set; }
    }
}
