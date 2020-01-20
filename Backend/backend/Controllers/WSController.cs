using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.Models;
using backend.Tools;
using backend.Models.Envios;
using backend.Models.Retornos;
using Newtonsoft.Json;

namespace backend.Controllers
{
    [Route("api")]
    [ApiController]
    [Produces("application/json")]
    public class WSController : ControllerBase
    {

        private readonly ContextAIG context;

        public WSController(ContextAIG context)
        {
            this.context = context;
        }

        [HttpPost("personasPorNombreYApellido")]
        public IActionResult personasPorNombreYApellido([FromBody] NombrePersona data)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = conexionEspol.personaPorNombreYApellido(data.nombres, data.apellidos).Result;
            var datosQuery = JsonConvert.DeserializeObject<List<DatosPersonaWS>>(resultado);
            return Ok(datosQuery);
        }

        [HttpPost("prueba")]
        public string prueba()
        {
            return Constants.datosMapaPrueba();
        }

        public class TipoSemana
        {
            public string tipo { get; set; } //C, clases - E, exámenes - N, no hay clases ni exámenes
        }
        [HttpPost("datosMapa")]
        public string datosMapa([FromBody] DatosMapaInput data)
        //[HttpGet("datosMapa/{dia}")]
        //public string datosMapa(int dia)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            TipoSemana tipoSemana = JsonConvert.DeserializeObject<TipoSemana>(conexionEspol.TipoSemana(data.fecha).Result);

            string resultado = conexionEspol.datosMapa((int)data.fecha.DayOfWeek, tipoSemana.tipo).Result;

            List<DatosMapaWS> datosQuery;
            datosQuery = JsonConvert.DeserializeObject<List<DatosMapaWS>>(resultado);

            Dictionary<string, List<DatosMapaRetorno>> retorno = new Dictionary<string, List<DatosMapaRetorno>>();
            DateTime horaInicioRango = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 7, 0, 0); //Fecha actual con 07:00:00
            DateTime horaFinRango = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 7, 30, 0);
            DateTime finBusqueda = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 20, 30, 0);
            Dictionary<int, DatosMapaRetorno> cantPorLugar;
            while (horaFinRango <= finBusqueda)
            {
                cantPorLugar = new Dictionary<int, DatosMapaRetorno>();

                //Llenado con datos del WS
                foreach (DatosMapaWS dato in datosQuery)
                {
                    if (dato.horaInicio <= horaInicioRango && dato.horaFin > horaFinRango)
                    {
                        string latitud = dato.latitud;
                        string longitud = dato.longitud;

                        if (dato.latitud == null || dato.longitud == null)
                        {
                            var espacio = this.context.TBL_Espacio.Where(x => x.idLugarBaseEspol == dato.idLugar).FirstOrDefault();
                            if (espacio != null)
                            {
                                latitud = espacio.latitud;
                                longitud = espacio.longitud;
                            }
                        }

                        if (!cantPorLugar.ContainsKey(dato.idLugar))
                        {
                            cantPorLugar.Add(dato.idLugar, new DatosMapaRetorno
                            {
                                lat = latitud,
                                lng = longitud,
                                count = dato.numRegistrados,
                            });
                        }
                        else
                        {
                            cantPorLugar[dato.idLugar].count += dato.numRegistrados;
                        }
                    }
                }

                //Llenado con datos de reuniones
                var reuniones = context.TBL_Reunion.Where(x => x.cancelada == "F" && x.fechaInicio >= DateTime.Today && x.fechaFin <= DateTime.Today.AddDays(1)).ToList();
                foreach (Reunion reunion in reuniones)
                {
                    if (!cantPorLugar.ContainsKey(reunion.idLugar))
                    {
                        string latitud = null;
                        string longitud = null;

                        var espacio = this.context.TBL_Espacio.Where(x => x.idLugarBaseEspol == reunion.idLugar).FirstOrDefault();
                        if (espacio != null)
                        {
                            latitud = espacio.latitud;
                            longitud = espacio.longitud;
                        }

                        cantPorLugar.Add(reunion.idLugar, new DatosMapaRetorno
                        {
                            lat = latitud,
                            lng = longitud,
                            count = this.context.TBL_Invitacion.Where(x => x.idReunion == reunion.id && x.estado != "A" && x.cancelada == "F").Count(),
                        });
                    }
                    else
                    {
                        cantPorLugar[reunion.idLugar].count += this.context.TBL_Invitacion.Where(x => x.idReunion == reunion.id && x.estado != "A" && x.cancelada == "F").Count();
                    }
                }

                retorno.Add(
                    horaInicioRango.ToString("HH:mm"),
                    cantPorLugar.Values.ToList()
                    );
                //Se suman 30 minutos a cada rango
                horaInicioRango = horaInicioRango.AddMinutes(30);
                horaFinRango = horaFinRango.AddMinutes(30);
            }

            return JsonConvert.SerializeObject(retorno);
        }

        [HttpPost("cursosRelacionados")]
        public IActionResult cursosRelacionados([FromForm] int idPersona)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = "";
            if (conexionEspol.esProfesor(idPersona))
            {
                resultado = conexionEspol.cursosProfesor(idPersona).Result;
            }
            else
            {
                resultado = conexionEspol.cursosEstudiante(idPersona).Result;
            }
            var datosQuery = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(resultado);
            return Ok(datosQuery);
        }

        [HttpPost("estudiantesPorCarrera")]
        public IActionResult estudiantesPorCarrera([FromForm] int idCarrera )
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = conexionEspol.estudiantesPorCarrera(idCarrera).Result;
            var datosQuery = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(resultado);
            return Ok(datosQuery);
        }

        [HttpPost("estudiantesPorFacultad")]
        public IActionResult estudiantesPorFacultad([FromForm] int idFacultad)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = conexionEspol.estudiantesPorFacultad(idFacultad).Result;
            var datosQuery = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(resultado);
            return Ok(datosQuery);
        }

        [HttpPost("estudiantesPorCurso")]
        public IActionResult estudiantesPorCurso([FromForm] int idCurso)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = conexionEspol.estudiantesPorCurso(idCurso).Result;
            var datosQuery = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(resultado);
            return Ok(datosQuery);
        }

        [HttpPost("profesoresPorFacultad")]
        public IActionResult profesoresPorFacultad([FromForm] int idFacultad)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = conexionEspol.profesoresPorFacultad(idFacultad).Result;
            var datosQuery = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(resultado);
            return Ok(datosQuery);
        }

        [HttpPost("dirigentesFacultad")]
        public IActionResult dirigentesFacultad([FromForm] int idFacultad)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado1 = conexionEspol.decanoFacultad(idFacultad).Result;
            var datosQuery1 = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(resultado1);
            string resultado2 = conexionEspol.subdecanoFacultad(idFacultad).Result;
            var datosQuery2 = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(resultado2);

            return Ok(datosQuery1.Concat(datosQuery2));
        }

        [HttpPost("horarioDisponibilidad")]
        public IActionResult horarioDisponibilidad([FromBody] IdsPersonas data)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            if (data.ids != null)
            {

            }
            //return Ok(conexionEspol.prueba(data));
            return Ok();
        }

        //La clase Prueba sale de carpeta Models.Envios
        //Ahí están todos los 'jsons' que pueden recibir mis funciones
        [HttpPost("prueba1")]
        public Object cantidadElementos([FromBody] Prueba argumento)
        {
            return new { 
                suma = argumento.num1 + argumento.num2,
                resta = argumento.num1 - argumento.num2,
                multiplicacion = argumento.num1 * argumento.num2,
                division = argumento.num1 / argumento.num2,
            };
        }

    }
}