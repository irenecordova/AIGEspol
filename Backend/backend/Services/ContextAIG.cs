using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using backend.Models;
using backend.Tools;

namespace backend.Services
{
    public class ContextAIG : DbContext
    {
        public ContextAIG(DbContextOptions<ContextAIG> options) : base(options)
        {

        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseOracle(Constants.ApiConnectionString);
        }

        public DbSet<Bloque> TBL_Bloque { get; set; }
        public DbSet<Espacio> TBL_Espacio { get; set; }
        public DbSet<Estado_Invitacion> TBL_Estado_Invitacion { get; set; }
        public DbSet<Invitacion> TBL_Invitacion { get; set; }
        public DbSet<Lista_Personalizada>TBL_Lista_Personalizada {get; set;}
        public DbSet<Lista_Persona> TBL_Lista_Persona { get; set; }
        public DbSet<Reunion> TBL_Reunion { get; set; }
        public DbSet<Tipo_Espacio> TBL_Tipo_Espacio { get; set; }
        public DbSet<Tipo_Filtro> TBL_Tipo_Filtro { get; set; }
    }
}
