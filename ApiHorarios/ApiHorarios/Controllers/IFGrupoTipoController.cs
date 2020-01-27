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
    public class IFGrupoTipoController : ControllerBase
    {
        private readonly CdaDbContextDB2SAF3 context;

        public IFGrupoTipoController(CdaDbContextDB2SAF3 context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<CdaIFGrupoTipo> Get()
        {
            return context.TBL_IF_GRUPO_TIPO.ToList();
        }
    }
}