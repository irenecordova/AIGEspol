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

        [HttpGet]
        public IEnumerable<CdaPersona> Get()
        {
            return context.TBL_PERSONA.ToList();
        }

        [HttpGet("{id}")]
        public IEnumerable<CdaPersona> profesores(int id)
        {
            return context.TBL_PERSONA.Where(x => x.intIdPersona == id).ToList();
        }
    }
}