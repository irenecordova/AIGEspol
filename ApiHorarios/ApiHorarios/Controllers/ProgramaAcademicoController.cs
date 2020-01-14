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
    public class ProgramaAcademicoController : ControllerBase
    {
        private readonly CdaDbContextDB2SAAC context;

        public ProgramaAcademicoController(CdaDbContextDB2SAAC context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<CdaProgramaAcademico> Get()
        {
            return context.TBL_PROGRAMA_ACADEMICO.ToList().Where(x => x.strEstado == "A" && x.strEstaVigente == "S");
        }

        [HttpGet("unidad/{unidad}")]
        public IEnumerable<CdaProgramaAcademico> PorUnidad(int unidad)
        {
            return context.TBL_PROGRAMA_ACADEMICO.Where(x => x.intIdUnidadEjecuta == unidad && x.strEstado == "A" && x.strEstaVigente == "S").ToList();
        }
    }
}