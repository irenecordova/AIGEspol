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
    }
}