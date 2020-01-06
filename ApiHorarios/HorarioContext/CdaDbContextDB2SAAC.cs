﻿using HorarioModelSaac;
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
        public DbSet<CdaHorario> TBL_HORARIO { get; set; }
        public DbSet<CdaCurso> TBL_CURSO { get; set; }
        public DbSet<CdaPersona> TBL_PERSONA { get; set; }
        public DbSet<CdaProgramaAcademico> TBL_PROGRAMA_ACADEMICO { get; set; }
        public DbSet<CdaLugar> TBL_LUGAR_ESPOL { get; set; }
    }
}
