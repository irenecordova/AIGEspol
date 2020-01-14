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

        public DbSet<Espacio> TBL_Espacio { get; set; }
        public DbSet<Nombre_Espacio> TBL_Nombre_Espacio { get; set; }
        public DbSet<Invitacion> TBL_Invitacion { get; set; }
        public DbSet<Lista_Personalizada>TBL_Lista_Personalizada {get; set;}
        public DbSet<Lista_Persona> TBL_Lista_Persona { get; set; }
        public DbSet<Reunion> TBL_Reunion { get; set; }
    }
}
