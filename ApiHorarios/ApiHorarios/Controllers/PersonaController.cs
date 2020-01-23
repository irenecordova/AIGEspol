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

        public class Username
        {
            public string username { get; set; }
        }
        public class RetornoIdPersona { 
            public int idPersona { get; set; }
        }
        [HttpPost("idPersona")]
        public RetornoIdPersona idPersonaPorUsername(Username data)
        {
            string cadenaCorreo = "@espol.edu.ec";
            var persona = context.TBL_PERSONA.Where(x => x.strEmail == data.username + cadenaCorreo).FirstOrDefault();
            if (persona != null)
            {
                return new RetornoIdPersona { idPersona = persona.intIdPersona };
            }

            return new RetornoIdPersona { idPersona = -1 };
        }

        public class ListaIdsPersonas
        {
            public List<int> idsPersonas { get; set; }
        }
        [HttpPost("nombresPersonas")]
        public IQueryable nombresPersonas([FromBody] ListaIdsPersonas data)
        {
            var query =
                from persona in context.TBL_PERSONA
                where data.idsPersonas.Contains(persona.intIdPersona)
                select new
                {
                    idPersona = persona.intIdPersona,
                    nombreCompleto = persona.strNombres + " " + persona.strApellidos
                };

            return query;
        }

        //[HttpGet("Profesores")]
        //public IQueryable profesores([])
    }
}