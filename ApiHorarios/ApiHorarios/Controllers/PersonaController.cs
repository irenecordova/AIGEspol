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
    public class PersonaController : ControllerBase
    {
        private readonly CdaDbContextDB2SAAC context;

        public PersonaController(CdaDbContextDB2SAAC context)
        {
            this.context = context;
        }

        [HttpGet("{numIdentificacion}")]
        public IEnumerable<CdaPersona> Get(string numIdentificacion)
        {
            return context.TBL_PERSONA.ToArray().Where(x => x.strNumeroIdentificacion == numIdentificacion);
        }

        [HttpGet("profesores")]
        public IEnumerable<CdaPersona> profesores(string numIdentificacion)
        {
            return context.TBL_PERSONA.ToArray();
        }
    }
}