using HorarioModelSaac;
using Microsoft.EntityFrameworkCore;
using System;

namespace HorarioContext
{
    public class CdaDbContextDB2SAAC : DbContext
    {
        public CdaDbContextDB2SAAC(DbContextOptions<CdaDbContextDB2SAAC> options) : base(options)
        {

        }
        public DbSet<CdaPeriodoAcademico> TBL_PERIODO_ACADEMICO { get; set; }
        

    }
}
