﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HorarioContext;
using HorarioModelSaac;
using Microsoft.AspNetCore.Mvc;

namespace ApiHorarios.Controllers
{
    [Route("api/PeriodoAcademico")]
    public class PeriodoAcademicoController : ControllerBase
    {
        private readonly CdaDbContextDB2SAAC context;

        public PeriodoAcademicoController(CdaDbContextDB2SAAC context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<CdaPeriodoAcademico> Get()
        { 
            return context.TBL_PERIODO_ACADEMICO.ToList().Where(x => x.chEstado == "A");
        }

        [HttpGet("actual")]
        public CdaPeriodoAcademico periodoActual()
        {
            return context.TBL_PERIODO_ACADEMICO.Where(x => x.dtFechaInicio <= DateTime.Today && x.dtFechaFin >= DateTime.Today).FirstOrDefault();
        }

        public class DataFecha
        {
            public DateTime fecha { get; set; }
        }
        public class TipoSemana
        {
            public string tipo { get; set; } //C, clases - E, exámenes - N, no hay clases ni exámenes
        }
        [HttpPost("tipoSemana")]
        public TipoSemana getTipoSemanaEnPeriodo(DataFecha data)
        {
            var periodoContenedor = context.TBL_PERIODO_ACADEMICO.Where(x => x.dtFechaInicio <= data.fecha && data.fecha <= x.dtFechaFin).FirstOrDefault();
            
            if (periodoContenedor == null) return new TipoSemana { tipo = "N" };

            //Semanas donde habrá examen
            if (data.fecha >= periodoContenedor.FechaIniEval1 && data.fecha <= periodoContenedor.FechaFinEval1) return new TipoSemana { tipo = "E" };
            if (data.fecha >= periodoContenedor.FechaIniEval2 && data.fecha <= periodoContenedor.FechaFinEval2) return new TipoSemana { tipo = "E" };
            if (data.fecha >= periodoContenedor.FechaIniMejoramiento && data.fecha <= periodoContenedor.FechaIniMejoramiento) return new TipoSemana { tipo = "E" };
            
            //Semanas donde no habrá ni examen, ni clases
            if (data.fecha > periodoContenedor.FechaFinMejoramiento) return new TipoSemana { tipo = "N" };
            if (data.fecha < periodoContenedor.FechaIniMejoramiento && data.fecha > periodoContenedor.FechaFinEval2) return new TipoSemana { tipo = "N" };
            
            //Caso contrario, habrá clases
            return new TipoSemana { tipo = "C" };
        }
    }
}