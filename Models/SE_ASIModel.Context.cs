﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SE_ASI.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SE_ASIEntities : DbContext
    {
        public SE_ASIEntities()
            : base("name=SE_ASIEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Diagnostico> Diagnostico { get; set; }
        public virtual DbSet<Indicadores_Obtenidos> Indicadores_Obtenidos { get; set; }
        public virtual DbSet<Informe_Recibido> Informe_Recibido { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
    }
}
