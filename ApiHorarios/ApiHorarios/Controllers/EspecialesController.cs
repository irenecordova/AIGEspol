using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HorarioContext;
using HorarioModelSaac;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiHorarios.Controllers
{
    //[Route("api/[controller]")]
    [Route("api")]
    [ApiController]
    public class EspecialesController : ControllerBase
    {
        private readonly CdaDbContextDB2SAAC context;

        public EspecialesController(CdaDbContextDB2SAAC context)
        {
            this.context = context;
        }
        
        //[HttpGet("periodoActual")]
        public CdaPeriodoAcademico periodoActual()
        {
            return context.TBL_PERIODO_ACADEMICO.FirstOrDefault(x => x.dtFechaInicio <= DateTime.Today && x.dtFechaFin >= DateTime.Today);
        }

        public class InDatosMapa
        {
            public int dia { get; set; }
            public string tipoSemana { get; set; }
        }
        [HttpPost("datosMapa")]
        public IQueryable datosMapa([FromBody] InDatosMapa data)
        //[HttpGet("datosMapa/{dia}", Name = "horarios_por_dia")]
        //public IQueryable datosMapa(int dia)
        {
            int idPeriodoActual = this.periodoActual().intIdPeriodoAcademico;
            var query =
                from horario in context.TBL_HORARIO
                join curso in context.TBL_CURSO on horario.intIdCurso equals curso.intIdCurso
                join lugar in context.TBL_LUGAR_ESPOL on horario.intIdAula equals lugar.intIdLugarEspol
                where horario.intDia == data.dia && curso.intIdPeriodo == idPeriodoActual && horario.chTipo == data.tipoSemana
                select new
                {
                    idHorario = horario.intIdHorario,
                    fecha = horario.dtFecha,
                    horaInicio = horario.dtHoraInicio,
                    horaFin = horario.dtHoraFin,
                    tipoHorario = horario.chTipo,
                    numRegistrados = curso.intNumRegistrados,
                    tipoCurso = curso.strTipoCurso,
                    idLugar = lugar.intIdLugarEspol,
                    descripcionLugar = lugar.strDescripcion,
                    latitud = lugar.strLatitud,
                    longitud = lugar.strLongitud,
                    tipoLugar = lugar.strTipo
                };
            return query;
        }

        // Cantidad de estudiantes/Cantidad registrados en periodo
        // Top 3 bloques con más personas
        // Cantidad de bloques usados/Cantidad de bloques totales
        // Prom. de personas por bloque
        // Cantidad de lugares usados (Aulas, labs, canchas)
        // Promedio personas por lugar (Aulas, labs, canchas)

        public class NombreApellido
        {
            public string nombres { get; set; }
            public string apellidos { get; set; }
        }

        [HttpPost("personasPorNombreYApellido")]
        public IQueryable personasPorNombreYApellido([FromBody] NombreApellido data)
        {
            var query = context.TBL_PERSONA.Where(persona => persona.strEstadoPersona == "A");

            if (data.nombres != "")
            {
                query = query.Where(persona => (persona.strNombres != null && persona.strNombres.Contains(data.nombres.ToUpper())));
            }

            if (data.apellidos != "")
            {
                query = query.Where(persona => (persona.strApellidos != null && persona.strApellidos.Contains(data.apellidos.ToUpper())));
            }

            return query;
        }

        public class IdPrograma
        {
            public int idPrograma { get; set; }
        }
        [HttpPost("estudiantesPorCarrera")]
        public IQueryable estudiantesPorCarrera([FromBody] IdPrograma data)
        {
            var query =
                from carrera in context.CARRERA_ESTUDIANTE
                join programa in context.TBL_PROGRAMA_ACADEMICO on carrera.strCodCarrera.Trim() equals programa.strCodCarrera.Trim()
                join estudiante in context.TBL_PERSONA on carrera.strCodEstudiante.Trim() equals estudiante.strCodEstudiante.Trim()
                where estudiante.strCodEstudiante != null && programa.intIdPrograma == data.idPrograma
                select new
                {
                    idPersona = estudiante.intIdPersona,
                    matricula = estudiante.strCodEstudiante,
                    nombres = estudiante.strNombres,
                    apellidos = estudiante.strApellidos,
                    email = estudiante.strEmail
                };

            return query;
        }

        public class IdFacultad
        {
            public int idFacultad { get; set; }
        }
        [HttpPost("estudiantesPorFacultad")]
        public IQueryable estudiantesPorFacultad([FromForm] IdFacultad data)
        {
            var query =
                from persona in context.TBL_PERSONA
                join carrera in context.CARRERA_ESTUDIANTE on persona.strCodEstudiante.Trim() equals carrera.strCodEstudiante.Trim()
                join programa in context.TBL_PROGRAMA_ACADEMICO on carrera.strCodCarrera.Trim() equals programa.strCodCarrera.Trim()
                where persona.strCodEstudiante != null && programa.intIdUnidadEjecuta == data.idFacultad
                select new
                {
                    idPersona = persona.intIdPersona,
                    matricula = persona.strCodEstudiante,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };

            return query;
        }

        public class IdMateria
        {
            public int idMateria { get; set; }
        }
        [HttpPost("estudiantesPorMateria")]
        public IQueryable estudiantesPorMateria([FromBody] IdMateria data)
        {
            var periodoActual = this.periodoActual();
            var query =
                from materia in context.TBL_MATERIA
                join historia in context.HISTORIA_ANIO on materia.strCodigoMateria equals historia.strCodMateria
                join persona in context.TBL_PERSONA on historia.strCodEstudiante equals persona.strCodEstudiante
                where historia.strAnio == periodoActual.strAnio && historia.strTermino == periodoActual.strTermino && materia.intIdMateria == data.idMateria
                select new
                {
                    idPersona = persona.intIdPersona,
                    matricula = persona.strCodEstudiante,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };

            return query;
        }

        public class IdCurso
        {
            public int idCurso { get; set; }
        }
        [HttpPost("estudiantesPorCurso")]
        public IActionResult estudiantesPorCurso([FromBody] IdCurso data)
        {
            var periodoActual = this.periodoActual();
            var query =
                from curso in context.TBL_CURSO
                join historia in context.HISTORIA_ANIO on curso.intIdCurso equals historia.intIdCurso
                join persona in context.TBL_PERSONA on historia.strCodEstudiante equals persona.strCodEstudiante
                where curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico && curso.strEstado == "A" && curso.intIdCurso == data.idCurso
                select new
                {
                    idPersona = persona.intIdPersona,
                    matricula = persona.strCodEstudiante,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };

            //if (query.ToArray().Length == 0) return NotFound();
            return Ok(JsonConvert.SerializeObject(query.ToArray()));
        }

        public IQueryable sacarHorarioEstudiante(int idPersona)
        {
            var periodoActual = this.periodoActual();

            var query =
                from historia in context.HISTORIA_ANIO
                join persona in context.TBL_PERSONA on historia.strCodEstudiante equals persona.strCodEstudiante
                join curso in context.TBL_CURSO on historia.intIdCurso equals curso.intIdCurso
                join horario in context.TBL_HORARIO on historia.intIdCurso equals horario.intIdCurso
                join materia in context.TBL_MATERIA on historia.strCodMateria equals materia.strCodigoMateria
                where curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                      && persona.intIdPersona == idPersona
                select new
                {
                    idPersona = persona.intIdPersona,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    idCurso = curso.intIdCurso,
                    nombreMateria = materia.strNombre,
                    nombreCompletoMateria = materia.strNombreCompleto,
                    cursoFechaInicio = curso.dtFechaInicio,
                    cursoFechaFin = curso.dtFechaFin,
                    horarioDia = horario.intDia,
                    horarioFecha = horario.dtFecha,
                    horarioHoraInicio = horario.dtHoraInicio,
                    horarioHoraFin = horario.dtHoraFin,
                    horarioTipo = horario.chTipo,
                };

            return query;
        }

        public IQueryable sacarHorarioProfesor(int idPersona)
        {
            var periodoActual = this.periodoActual();

            var query =
                from curso in context.TBL_CURSO
                join persona in context.TBL_PERSONA on curso.intIdProfesor equals persona.intIdPersona
                join horario in context.TBL_HORARIO on curso.intIdCurso equals horario.intIdCurso
                join materia in context.TBL_MATERIA on curso.intIdMateria equals materia.intIdMateria
                where curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                      && persona.intIdPersona == idPersona
                select new
                {
                    idPersona = persona.intIdPersona,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    idCurso = curso.intIdCurso,
                    nombreMateria = materia.strNombre,
                    nombreCompletoMateria = materia.strNombreCompleto,
                    cursoFechaInicio = curso.dtFechaInicio,
                    cursoFechaFin = curso.dtFechaFin,
                    horarioDia = horario.intDia,
                    horarioFecha = horario.dtFecha,
                    horarioHoraInicio = horario.dtHoraInicio,
                    horarioHoraFin = horario.dtHoraFin,
                    horarioTipo = horario.chTipo,
                };

            return query;
        }

        public bool esProfesor(int idPersona)
        {
            var periodoActual = this.periodoActual();
            var query = context.TBL_CURSO.Where(curso => curso.strEstado == "A" && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico && 
            (curso.intIdProfesor == idPersona || curso.intIdProfesor1 == idPersona || curso.intIdProfesor2 == idPersona || 
            curso.intIdProfesor3 == idPersona || curso.intIdProfesor4 == idPersona || curso.intIdProfesor5 == idPersona));
            return query.ToList().Count() > 0;
        }

        public class IdPersona
        {
            public int idPersona { get; set; }
        }
        [HttpPost("horarioEstudiante")]
        public IQueryable horarioEstudiante([FromBody] IdPersona data)
        {
            return sacarHorarioEstudiante(data.idPersona);
        }

        [HttpPost("horarioProfesor")]
        public IQueryable horarioProfesor([FromBody] IdPersona data)
        {
            return sacarHorarioProfesor(data.idPersona);
        }

        public class IdsPersonas
        {
            public List<int> idsPersonas { get; set; }
        }
        [HttpPost("horariosPersonas")]
        public List<IQueryable> horariosPersonas([FromBody] IdsPersonas data)
        {
            var periodoActual = this.periodoActual();

            List<IQueryable> lista = new List<IQueryable>();

            foreach(int idPersona in data.idsPersonas)
            {
                if (esProfesor(idPersona))
                {
                    lista.Add(sacarHorarioProfesor(idPersona));
                }
                else
                {
                    lista.Add(sacarHorarioEstudiante(idPersona));
                }
                
            };
            return lista;
        }

        [HttpPost("materiasPorProfesor")]
        public IQueryable materiasPorProfesor([FromBody] IdPersona data)
        {
            var periodoActual = this.periodoActual();

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

            return query;
        }

        [HttpPost("esProfesor")]
        public Dictionary<string, bool> wsEsProfesor([FromBody] IdPersona data)
        {
            var periodoActual = this.periodoActual();
            var query =
                from curso in context.TBL_CURSO
                where curso.strEstado == "A" && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico && curso.intIdProfesor == data.idPersona
                select new
                {
                    idCurso = curso.intIdCurso
                };

            Dictionary<string, bool> resultado = new Dictionary<string, bool>();
            resultado.Add("resultado",query.ToArray().Length != 0);
            return resultado;
        }

        [HttpPost("profesoresPorFacultad")]
        public IQueryable profesoresPorFacultad([FromBody] IdFacultad data)
        {
            //Se va a tomar en cuenta que enseñen materias que pertenezcan a la facultad.
            //Si enseña al menos una de la facultad, se lo tomará en cuenta.
            var periodoActual = this.periodoActual();
            var query =
                from persona in context.TBL_PERSONA
                join curso in context.TBL_CURSO on persona.intIdPersona equals curso.intIdProfesor
                join materia in context.TBL_MATERIA on curso.intIdMateria equals materia.intIdMateria
                where curso.strEstado == "A" && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico && materia.intIdUnidad == data.idFacultad
                select new
                {
                    idPersona = persona.intIdPersona,
                    matricula = persona.strCodEstudiante,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };

            return query;
        }

        [HttpPost("profesoresPorMateria")]
        public IQueryable profesoresPorMateria([FromBody] IdMateria data )
        {
            var periodoActual = this.periodoActual();
            var query =
                from materia in context.TBL_MATERIA
                join curso in context.TBL_CURSO on materia.intIdMateria equals curso.intIdMateria
                join persona in context.TBL_PERSONA on curso.intIdProfesor equals persona.intIdPersona
                where materia.intIdMateria == data.idMateria
                select new
                {
                    idPersona = persona.intIdPersona,
                    matricula = persona.strCodEstudiante,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };

            //if (query.ToArray().Length == 0) return NotFound();
            return query;
        }

        [HttpPost("directivoFacultad")]
        public IQueryable directivoFacultad([FromBody] IdFacultad data)
        {
            var query =
                from unidad in context.TBL_UNIDAD
                join persona in context.TBL_PERSONA on Int32.Parse(unidad.strIdDirectivo.Trim()) equals persona.intIdPersona
                where unidad.intIdUnidad == data.idFacultad
                select new
                {
                    idPersona = persona.intIdPersona,
                    matricula = persona.strCodEstudiante,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };
            return query;
        }

        [HttpPost("subdecanoFacultad")]
        public IQueryable subdecanoFacultad([FromBody] IdFacultad data)
        {
            var query =
                from unidad in context.TBL_UNIDAD
                join persona in context.TBL_PERSONA on unidad.intIdSubdecano equals persona.intIdPersona
                where unidad.intIdUnidad == data.idFacultad
                select new
                {
                    idPersona = persona.intIdPersona,
                    matricula = persona.strCodEstudiante,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };
            return query;
        }

        /*
        [HttpPost("coordinadoresMateriasFacultad")]
        public IActionResult coordinadoresMateriasFacultad ([FromForm] int idFacultad)
        {
            var query =
                from unidad in context.TBL_UNIDAD
                join persona in context.TBL_PERSONA on unidad.intIdSubdecano equals persona.intIdPersona
                where unidad.intIdUnidad == idFacultad
                select new
                {
                    idPersona = persona.intIdPersona,
                    matricula = persona.strCodEstudiante,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };
            return Ok(JsonConvert.SerializeObject(query.ToList()));
        }
        */
        [HttpPost("coordinadorCarrera")]
        public IQueryable coordinadorMateria([FromBody] IdPrograma data)
        {
            var query =
                from programa in context.TBL_PROGRAMA_ACADEMICO
                join persona in context.TBL_PERSONA on programa.intIdCoordinador equals persona.intIdPersona
                where programa.intIdPrograma == data.idPrograma
                select new
                {
                    idPersona = persona.intIdPersona,
                    matricula = persona.strCodEstudiante,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };
            return query;
        }

        [HttpPost("coordinadoresFacultad")]
        public IQueryable coordinadoresFacultad([FromBody] IdFacultad data)
        {
            var query =
                from programa in context.TBL_PROGRAMA_ACADEMICO
                join unidad in context.TBL_UNIDAD on programa.intIdUnidadEjecuta equals unidad.intIdUnidad
                join persona in context.TBL_PERSONA on programa.intIdCoordinador equals persona.intIdPersona
                where unidad.intIdUnidad == data.idFacultad
                select new
                {
                    idPersona = persona.intIdPersona,
                    matricula = persona.strCodEstudiante,
                    nombres = persona.strNombres,
                    apellidos = persona.strApellidos,
                    email = persona.strEmail
                };
            return query;
        }

        [HttpPost("LugarPadre")]
        public object idLugarPadre([FromForm] int idLugar)
        {
            CdaLugar lugar = context.TBL_LUGAR_ESPOL.Where(x => x.intIdLugarEspol == idLugar).FirstOrDefault();
            if (lugar.intIdLugarPadre != null)
            {
                return new { idPadre = context.TBL_LUGAR_ESPOL.Where(x => x.intIdLugarEspol == lugar.intIdLugarPadre).FirstOrDefault().intIdLugarEspol };
            }

            return new { idPadre = -1 };
        }

        public IQueryable sacarCursosEstudiante(int idPersona)
        {
            var periodoActual = this.periodoActual();
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
            var periodoActual = this.periodoActual();
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

        [HttpPost("cursosEstudiante")]
        public IQueryable cursosEstudiante([FromBody] IdPersona data)
        {       
            return sacarCursosEstudiante(data.idPersona);
        }

        [HttpPost("cursosProfesor")]
        public IQueryable cursosProfesor([FromBody] IdPersona data)
        {
            return sacarCursosProfesor(data.idPersona);
        }

        [HttpPost("cursosRelacionados")]
        public IQueryable cursosRelacionados([FromBody] IdPersona data)
        {
            if (esProfesor(data.idPersona)) return sacarCursosProfesor(data.idPersona);
            else return sacarCursosEstudiante(data.idPersona);
        }
    }
}