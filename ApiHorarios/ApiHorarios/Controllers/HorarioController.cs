using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HorarioContext;
using HorarioModelSaac;
using Microsoft.AspNetCore.Mvc;

namespace ApiHorarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorarioController : ControllerBase
    {
        private readonly CdaDbContextDB2SAAC context;

        public HorarioController(CdaDbContextDB2SAAC context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<CdaHorario> Get()
        {
            var periodoActual = context.TBL_PERIODO_ACADEMICO.FirstOrDefault(x => x.dtFechaInicio <= DateTime.Today && x.dtFechaFin >= DateTime.Today);
            var query =
                from horario in context.TBL_HORARIO
                join curso in context.TBL_CURSO on horario.intIdCurso equals curso.intIdCurso
                where curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                select horario;
            return query.ToList();
        }
    }
}