using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HorarioContext;
using HorarioModelSaf3;

namespace ApiHorarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IFGrupoNivelController : ControllerBase
    {
        private readonly CdaDbContextDB2SAF3 context;

        public IFGrupoNivelController(CdaDbContextDB2SAF3 context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<CdaIFGrupoNivel> Get()
        {
            return context.TBL_IF_GRUPO_NIVEL.ToList();
        }

    }
}