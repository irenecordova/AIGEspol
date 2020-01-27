using HorarioModelSaac;
using Microsoft.EntityFrameworkCore;
using System;
using HorarioModelSaf3;

namespace HorarioContext
{
    public class CdaDbContextDB2SAF3 : DbContext
    {
        public CdaDbContextDB2SAF3(DbContextOptions<CdaDbContextDB2SAF3> options) : base(options)
        {

        }

        public DbSet<CdaIFGrupo> TBL_IF_GRUPO { get; set; }
        public DbSet<CdaIFGrupoTipo> TBL_IF_GRUPO_TIPO { get; set; }
        public DbSet<CdaIFGrupoNivel> TBL_IF_GRUPO_NIVEL { get; set; }
        public DbSet<CdaIFNivel> TBL_IF_NIVEL { get; set; }
        public DbSet<CdaIFSubgrupo> TBL_IF_SUBGRUPO { get; set; }
    }
}
