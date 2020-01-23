using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HorarioContext;
using HorarioModelSaac;

namespace ApiHorarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarreraEstudianteController : ControllerBase
    {
        private readonly CdaDbContextDB2SAAC context;

        public CarreraEstudianteController(CdaDbContextDB2SAAC context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<CdaCarreraEstudiante> Get()
        {
            return context.CARRERA_ESTUDIANTE.Where(x => x.strEstadoCarrEstud == "A").ToList();
        }

        [HttpGet("estudiante/{id}")]
        public IEnumerable<CdaCarreraEstudiante> carreraDeEstudiante(int id)
        {
            var persona = context.TBL_PERSONA.Where(x => x.intIdPersona == id).FirstOrDefault();
            string codEstudiante = persona.strCodEstudiante.Trim();
            return context.CARRERA_ESTUDIANTE.Where(x => x.strCodEstudiante.Contains(codEstudiante) && x.strEstadoCarrEstud == "A").ToList();
        }
    }
}