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
    [Route("api/[controller]")]
    [ApiController]
    public class CursoController : ControllerBase
    {
        private readonly CdaDbContextDB2SAAC context;

        public CursoController(CdaDbContextDB2SAAC context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<CdaCurso> Get()
        {
            var periodoActual = context.TBL_PERIODO_ACADEMICO.FirstOrDefault(x => x.dtFechaInicio <= DateTime.Today && x.dtFechaFin >= DateTime.Today);
            return context.TBL_CURSO.ToList().Where(x => x.strEstado == "A" && x.intIdPeriodo == periodoActual.intIdPeriodoAcademico);
        }

        [HttpGet("{id}")]
        public CdaCurso Get(int id)
        {
            return context.TBL_CURSO.ToList().Where(x => x.strEstado == "A").FirstOrDefault();
        }

        public IQueryable sacarCursosEstudiante(int idPersona)
        {
            var periodoActual = new PeriodoAcademicoController(context).periodoActual();
            var query =
                from curso in context.TBL_CURSO
                join historia in context.HISTORIA_ANIO on curso.intIdCurso equals historia.intIdCurso
                join persona in context.TBL_PERSONA on historia.strCodEstudiante equals persona.strCodEstudiante
                join materia in context.TBL_MATERIA on curso.intIdMateria equals materia.intIdMateria
                where persona.intIdPersona == idPersona && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                select new
                {
                    idCurso = curso.intIdCurso,
                    nombreMateria = materia.strNombreCompleto,
                    numeroParalelo = curso.intParalelo
                };

            return query;
        }

        public IQueryable sacarCursosProfesor(int idPersona)
        {
            var periodoActual = new PeriodoAcademicoController(context).periodoActual();
            var query =
                from curso in context.TBL_CURSO
                join persona in context.TBL_PERSONA on curso.intIdProfesor equals persona.intIdPersona
                join materia in context.TBL_MATERIA on curso.intIdMateria equals materia.intIdMateria
                where persona.intIdPersona == idPersona && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                select new
                {
                    idCurso = curso.intIdCurso,
                    nombreMateria = materia.strNombreCompleto,
                    numeroParalelo = curso.intParalelo
                };

            return query;
        }

        //Vale
        [HttpPost("estudiante")]
        public IQueryable cursosEstudiante([FromBody] IdPersona data)
        {
            return sacarCursosEstudiante(data.idPersona);
        }

        //Vale
        [HttpPost("profesor")]
        public IQueryable cursosProfesor([FromBody] IdPersona data)
        {
            return sacarCursosProfesor(data.idPersona);
        }

        [HttpPost("relacionados")]
        //Vale
        public IQueryable cursosRelacionados([FromBody] IdPersona data)
        {
            if (new PersonaController(context).esProfesor(data.idPersona)) return sacarCursosProfesor(data.idPersona);
            else return sacarCursosEstudiante(data.idPersona);
        }

    }
}