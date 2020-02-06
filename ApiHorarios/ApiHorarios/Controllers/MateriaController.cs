using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HorarioContext;
using HorarioModelSaac;
using ApiHorarios.DataRepresentationsIN;

namespace ApiHorarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MateriaController : ControllerBase
    {
        private readonly CdaDbContextDB2SAAC context;

        public MateriaController(CdaDbContextDB2SAAC context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<CdaMateria> Get()
        {
            return context.TBL_MATERIA.ToList();
        }

        [HttpGet("{id}")]
        public CdaMateria materia(int id)
        {
            return context.TBL_MATERIA.Where(x => x.intIdMateria == id).FirstOrDefault();
        }

        [HttpPost("profesor")]
        public IQueryable materiasPorProfesor([FromBody] IdPersona data)
        {
            var periodoActual = new PeriodoAcademicoController(context).periodoActual();

            var query =
                from materia in context.TBL_MATERIA
                join curso in context.TBL_CURSO on materia.intIdMateria equals curso.intIdMateria
                join persona in context.TBL_PERSONA on curso.intIdProfesor equals persona.intIdPersona
                where curso.strEstado == "A" && persona.intIdPersona == data.idPersona && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                select new
                {
                    idMateria = materia.intIdMateria,
                    nombreMateria = materia.strNombre,
                    nombreCompletoMateria = materia.strNombreCompleto,
                };

            return query.Distinct().OrderBy(x => x.nombreMateria);
        }

        //Vale
        [HttpPost("facultad")]
        public IQueryable materiasPorFacultad([FromBody] IdFacultad data)
        {
            var periodoActual = new PeriodoAcademicoController(context).periodoActual();
            var query =
                from materia in context.TBL_MATERIA
                join unidad in context.TBL_UNIDAD on materia.intIdUnidad equals unidad.intIdUnidad
                join curso in context.TBL_CURSO on materia.intIdMateria equals curso.intIdMateria
                where unidad.intIdUnidad == data.idFacultad && curso.strEstado == "A" && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                select new
                {
                    idMateria = materia.intIdMateria,
                    nombreMateria = materia.strNombre,
                    nombreCompletoMateria = materia.strNombreCompleto,
                };
            return query.Distinct().OrderBy(x => x.nombreMateria);
        }
    }
}