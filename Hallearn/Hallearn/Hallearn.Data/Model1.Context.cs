﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class db_HallearnEntities : DbContext
    {
        public db_HallearnEntities()
            : base("name=db_HallearnEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<hlnciudad> hlnciudad { get; set; }
        public virtual DbSet<hlnclase> hlnclase { get; set; }
        public virtual DbSet<hlnconfig> hlnconfig { get; set; }
        public virtual DbSet<hlndepartamento> hlndepartamento { get; set; }
        public virtual DbSet<hlnmateria> hlnmateria { get; set; }
        public virtual DbSet<hlnpais> hlnpais { get; set; }
        public virtual DbSet<hlntema> hlntema { get; set; }
        public virtual DbSet<hlnusuario> hlnusuario { get; set; }
        public virtual DbSet<hlnvideoclase> hlnvideoclase { get; set; }
        public virtual DbSet<hlnprogtema> hlnprogtema { get; set; }
        public virtual DbSet<hlnchat> hlnchat { get; set; }
        public virtual DbSet<hlnarchivo> hlnarchivo { get; set; }
    }
}
