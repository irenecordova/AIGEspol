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

        [HttpPost("nombresPersonas")]
        public IQueryable nombresPersonas([FromBody] IdsPersonas data)
        {
            var ids = data.idsPersonas.AsEnumerable();
            var query =
                from persona in context.TBL_PERSONA
                join id in ids on persona.intIdPersona equals (int)id
                select new
                {
                    idPersona = persona.intIdPersona,
                    nombreCompleto = persona.strNombres + " " + persona.strApellidos
                };

            return query;
        }

        [HttpGet("profesores")]
        public IQueryable profesores()
        {
            var periodoActual = context.TBL_PERIODO_ACADEMICO.FirstOrDefault(x => x.dtFechaInicio <= DateTime.Today && x.dtFechaFin >= DateTime.Today);
            var query =
                from curso in context.TBL_CURSO
                join persona in context.TBL_PERSONA on curso.intIdProfesor equals persona.intIdPersona
                where persona.strEstadoPersona == "A" && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                select new
                {
                    idPersona = persona.intIdPersona,
                    //identificacion = persona.strNumeroIdentificacion,
                    //tipoIdentificacion = persona.strTipoIdentificacion,
                    //matricula = persona.strCodEstudiante,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };

            return query.Distinct().OrderBy(x => x.nombres + " " + x.apellidos);
        }

        [HttpGet("directivos")]
        public IQueryable directivosFacultades()
        {
            var query =
                from unidad in context.TBL_UNIDAD
                join persona in context.TBL_PERSONA on Int32.Parse(unidad.strIdDirectivo.Trim()) equals persona.intIdPersona
                where unidad.strIdDirectivo != null
                select new
                {
                    idPersona = persona.intIdPersona,
                    //matricula = persona.strCodEstudiante,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };

            var query2 =
                from unidad in context.TBL_UNIDAD
                join persona in context.TBL_PERSONA on unidad.intIdSubdecano equals persona.intIdPersona
                where unidad.intIdSubdecano != null
                select new
                {
                    idPersona = persona.intIdPersona,
                    //matricula = persona.strCodEstudiante,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };

            return query.Concat(query2).Distinct().OrderBy(x => x.nombres + " " + x.apellidos);
        }

        public bool esProfesor(int idPersona)
        {
            var periodoActual = new PeriodoAcademicoController(context).periodoActual();
            var query = context.TBL_CURSO.Where(curso => curso.strEstado == "A" && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico &&
            (curso.intIdProfesor == idPersona || curso.intIdProfesor1 == idPersona || curso.intIdProfesor2 == idPersona ||
            curso.intIdProfesor3 == idPersona || curso.intIdProfesor4 == idPersona || curso.intIdProfesor5 == idPersona));
            return query.ToList().Count() > 0;
        }

        //Obtiene los datos de las personas cuyos nombres coinciden con el criterio de búsqueda. Recibe nombres y apellidos por separado.
        [HttpPost("porNombreSeparado")]
        public IQueryable personasPorNombreYApellido([FromBody] NombreApellido data)
        {
            var query = context.TBL_PERSONA.Where(persona => persona.strEstadoPersona == "A");

            if (data.nombres != "")
            {
                query = query.Where(persona => (persona.strNombres != null && persona.strNombres.Contains(data.nombres.ToUpper()) && persona.strEstadoPersona == "A"));
            }

            if (data.apellidos != "")
            {
                query = query.Where(persona => (persona.strApellidos != null && persona.strApellidos.Contains(data.apellidos.ToUpper()) && persona.strEstadoPersona == "A"));
            }

            return query;
        }

        //Obtiene los datos de las personas cuyos nombres coinciden con el criterio de búsqueda. Solo recibe nombre y apellido en un string, ya sea completo o no.
        [HttpPost("porNombre")]
        public IActionResult personasPorNombreCompleto([FromBody] NombreCompleto data)
        {
            if (data.nombre != "" && data.nombre != null)
            {
                var query = context.TBL_PERSONA.Where(persona => persona.strEstadoPersona == "A");
                if (data.nombre.Contains(" "))
                {
                    var separado = data.nombre.Split(" ");
                    foreach (var palabra in separado)
                    {
                        query = query.Where(persona => persona.strNombres != null && persona.strApellidos != null &&
                        (persona.strNombres.Trim().Contains(palabra.ToUpper()) || persona.strApellidos.Trim().Contains(palabra.ToUpper())));
                    }
                }
                else
                {
                    query = query.Where(persona => persona.strNombres != null && persona.strApellidos != null &&
                    (persona.strNombres.Trim().Contains(data.nombre.ToUpper()) || persona.strApellidos.Trim().Contains(data.nombre.ToUpper())));
                }
                return Ok(query.OrderBy(x => x.strNombres + " " + x.strApellidos));
            }

            return Ok(new List<CdaPersona>());
        }

        //Obtiene los datos de las personas que se encuentran cursando una carrera.
        [HttpPost("estudiantes/carrera")]
        public IQueryable estudiantesPorCarrera([FromBody] IdPrograma data)
        {
            var query =
                from carrera in context.CARRERA_ESTUDIANTE
                join programa in context.TBL_PROGRAMA_ACADEMICO on carrera.strCodCarrera equals programa.strCodCarrera
                join persona in context.TBL_PERSONA on carrera.strCodEstudiante equals persona.strCodEstudiante
                where persona.strCodEstudiante != null && programa.intIdPrograma == data.idPrograma
                && carrera.strEstadoCarrEstud == "A"
                && persona.strEstadoPersona == "A"
                select new
                {
                    idPersona = persona.intIdPersona,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };

            return query.Distinct().OrderBy(x => x.nombres + " " + x.apellidos);
        }

        //Obtiene los estudiantes cuyas carreras pertenecen a cierta facultad.
        [HttpPost("estudiantes/facultad")]
        public IQueryable estudiantesPorFacultad([FromBody] IdFacultad data)
        {
            var query =
                from persona in context.TBL_PERSONA
                join carrera in context.CARRERA_ESTUDIANTE on persona.strCodEstudiante equals carrera.strCodEstudiante
                join programa in context.TBL_PROGRAMA_ACADEMICO on carrera.strCodCarrera equals programa.strCodCarrera
                where programa.intIdUnidadEjecuta == data.idFacultad
                && carrera.strEstadoCarrEstud == "A"
                && persona.strEstadoPersona == "A"
                select new
                {
                    idPersona = persona.intIdPersona,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };

            return query.Distinct().OrderBy(x => x.nombres + " " + x.apellidos);
        }

        //Obtiene todos los estudiantes que se encuentran cursando una materia específica.
        [HttpPost("estudiantes/materia")]
        public IQueryable estudiantesPorMateria([FromBody] IdMateria data)
        {
            var periodoActual = new PeriodoAcademicoController(context).periodoActual();
            var query =
                from materia in context.TBL_MATERIA
                join historia in context.HISTORIA_ANIO on materia.strCodigoMateria equals historia.strCodMateria
                join persona in context.TBL_PERSONA on historia.strCodEstudiante equals persona.strCodEstudiante
                where historia.strAnio == periodoActual.strAnio && historia.strTermino == periodoActual.strTermino && materia.intIdMateria == data.idMateria
                && persona.strEstadoPersona == "A"
                select new
                {
                    idPersona = persona.intIdPersona,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };

            return query.Distinct().OrderBy(x => x.nombres + " " + x.apellidos);
        }

        //Vale
        [HttpPost("estudiantes/curso")]
        public IQueryable estudiantesPorCurso([FromBody] IdCurso data)
        {
            var periodoActual = new PeriodoAcademicoController(context).periodoActual();
            var query =
                from curso in context.TBL_CURSO
                join historia in context.HISTORIA_ANIO on curso.intIdCurso equals historia.intIdCurso
                join persona in context.TBL_PERSONA on historia.strCodEstudiante equals persona.strCodEstudiante
                where curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico && curso.strEstado == "A" && curso.intIdCurso == data.idCurso && persona.strEstadoPersona == "A"
                select new
                {
                    idPersona = persona.intIdPersona,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };

            return query.Distinct().OrderBy(x => x.nombres + " " + x.apellidos);
        }

        //Vale
        [HttpPost("directivo/facultad")]
        public IQueryable directivoFacultad([FromBody] IdFacultad data)
        {
            var query =
                from unidad in context.TBL_UNIDAD
                join persona in context.TBL_PERSONA on Int32.Parse(unidad.strIdDirectivo.Trim()) equals persona.intIdPersona
                where unidad.intIdUnidad == data.idFacultad
                select new
                {
                    idPersona = persona.intIdPersona,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };
            return query.OrderBy(x => x.nombres + " " + x.apellidos);
        }

        //Vale
        [HttpPost("subdecano/facultad")]
        public IQueryable subdecanoFacultad([FromBody] IdFacultad data)
        {
            var query =
                from unidad in context.TBL_UNIDAD
                join persona in context.TBL_PERSONA on unidad.intIdSubdecano equals persona.intIdPersona
                where unidad.intIdUnidad == data.idFacultad
                select new
                {
                    idPersona = persona.intIdPersona,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };
            return query.OrderBy(x => x.nombres + " " + x.apellidos);
        }

        //Vale
        [HttpPost("profesores/facultad")]
        public IQueryable profesoresPorFacultad([FromBody] IdFacultad data)
        {
            //Se va a tomar en cuenta que enseñen materias que pertenezcan a la facultad.
            //Si enseña al menos una de la facultad, se lo tomará en cuenta.
            var periodoActual = new PeriodoAcademicoController(context).periodoActual();
            var query =
                from persona in context.TBL_PERSONA
                join curso in context.TBL_CURSO on persona.intIdPersona equals curso.intIdProfesor
                join materia in context.TBL_MATERIA on curso.intIdMateria equals materia.intIdMateria
                where curso.strEstado == "A" && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico && materia.intIdUnidad == data.idFacultad
                && persona.strEstadoPersona == "A"
                select new
                {
                    idPersona = persona.intIdPersona,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };

            return query.Distinct().OrderBy(x => x.nombres + " " + x.apellidos);
        }

        //Vale
        [HttpPost("profesores/materia")]
        public IQueryable profesoresPorMateria([FromBody] IdMateria data)
        {
            var periodoActual = new PeriodoAcademicoController(context).periodoActual();
            var query =
                from materia in context.TBL_MATERIA
                join curso in context.TBL_CURSO on materia.intIdMateria equals curso.intIdMateria
                join persona in context.TBL_PERSONA on curso.intIdProfesor equals persona.intIdPersona
                where materia.intIdMateria == data.idMateria && persona.strEstadoPersona == "A" && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                select new
                {
                    idPersona = persona.intIdPersona,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };

            return query.Distinct().OrderBy(x => x.nombres + " " + x.apellidos);
        }

        [HttpPost("emails")]
        public IEnumerable<string> emailsPersonas([FromBody] IdsPersonas data)
        {
            var ids = data.idsPersonas.AsEnumerable();
            var query =
                from persona in context.TBL_PERSONA
                join id in ids on persona.intIdPersona equals (int)id
                select new
                {
                    persona
                };
            return query.Select(x => x.persona.strEmail);
        }
    }
}