using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HorarioContext;
using HorarioModelSaac;
using Microsoft.AspNetCore.Mvc;
using ApiHorarios.DataRepresentationsIN;

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

        public CdaPeriodoAcademico GetPeriodoFecha(DateTime fecha)
        {
            return context.TBL_PERIODO_ACADEMICO.Where(x => x.dtFechaInicio <= fecha && x.dtFechaFin >= fecha).FirstOrDefault();
        }

        [HttpPost("periodoDeFecha")]
        public CdaPeriodoAcademico periodoFecha([FromBody] InDataFecha data)
        {
            return this.GetPeriodoFecha(data.fecha);
        }

        [HttpPost("tipoSemana")]
        public TipoSemana getTipoSemanaEnPeriodo([FromBody] InDataFecha data)
        {
            var periodoContenedor = context.TBL_PERIODO_ACADEMICO.Where(x => x.dtFechaInicio <= data.fecha && data.fecha <= x.dtFechaFin).FirstOrDefault();

            if (periodoContenedor == null) return new TipoSemana { tipo = "N" };

            //Semanas donde habrá examen
            if (data.fecha >= periodoContenedor.FechaIniEval1 && data.fecha.Date <= periodoContenedor.FechaFinEval1) return new TipoSemana { tipo = "E" };
            if (data.fecha >= periodoContenedor.FechaIniEval2 && data.fecha.Date <= periodoContenedor.FechaFinEval2) return new TipoSemana { tipo = "E" };
            if (data.fecha >= periodoContenedor.FechaIniMejoramiento && data.fecha.Date <= periodoContenedor.FechaFinMejoramiento) return new TipoSemana { tipo = "E" };

            //Semanas donde no habrá ni examen, ni clases
            if (data.fecha > periodoContenedor.FechaFinMejoramiento) return new TipoSemana { tipo = "N" };
            if (data.fecha < periodoContenedor.FechaIniMejoramiento && data.fecha > periodoContenedor.FechaFinEval2) return new TipoSemana { tipo = "N" };

            //Caso contrario, habrá clases
            return new TipoSemana { tipo = "C" };
        }

        public string tipoExamen(DateTime fecha) {
            var periodoActual = this.GetPeriodoFecha(fecha);
            string examen = null;
            if (fecha >= periodoActual.FechaIniEval1 && fecha.Date <= periodoActual.FechaFinEval1) examen = "1";
            else if (fecha >= periodoActual.FechaIniEval2 && fecha.Date <= periodoActual.FechaFinEval2) examen = "2";
            else if (fecha >= periodoActual.FechaIniMejoramiento && fecha.Date <= periodoActual.FechaFinMejoramiento) examen = "M";
            return examen;
        }
    }
}