using System;
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

            
        }
    }
}