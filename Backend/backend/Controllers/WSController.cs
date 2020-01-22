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

        //Vale
        [HttpPost("personasPorNombreYApellido")]
        public string personasPorNombreYApellido([FromBody] NombrePersona data)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = conexionEspol.personaPorNombreYApellido(data.nombres, data.apellidos).Result;
            return resultado;
        }
        //Luis Arízaga: 90122
        //Irene Córdova: 90118
        //Alfonso Pulido: 61970
        //Luis Eduardo Mendoza: 191387
        //Boris Vintimilla: 3333
        //Lucio Arias: 90124

        //Vale
        [HttpPost("periodoSegunFecha")]
        public string periodoDeFecha([FromBody] DatosMapaInput data)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            return conexionEspol.PeriodoDeFecha(data.Fecha).Result;
        }

        //Vale
        [HttpPost("tipoSemana")]
        public string tipoSemana([FromBody] DatosMapaInput data)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            return conexionEspol.TipoSemana(data.Fecha).Result;
        }

        public class TipoSemana
        {
            public string tipo { get; set; } //C, clases - E, exámenes - N, no hay clases ni exámenes
        }
        //Vale
        [HttpPost("datosMapa")]
        public string datosMapa([FromBody] DatosMapaInput data)
        //[HttpGet("datosMapa/{dia}")]
        //public string datosMapa(int dia)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            TipoSemana tipoSemana = JsonConvert.DeserializeObject<TipoSemana>(conexionEspol.TipoSemana(data.Fecha).Result);

            string resultado = conexionEspol.datosMapa(data.Fecha, (int)data.Fecha.DayOfWeek, tipoSemana.tipo).Result;
            Console.WriteLine(resultado);

            List<DatosMapaWS> datosQuery;
            datosQuery = JsonConvert.DeserializeObject<List<DatosMapaWS>>(resultado);

            Dictionary<string, List<DatosMapaRetorno>> retorno = new Dictionary<string, List<DatosMapaRetorno>>();
            DateTime horaInicioRango = new DateTime(data.Fecha.Year, data.Fecha.Month, data.Fecha.Day, 7, 0, 0); //Fecha enviada con 07:00:00
            DateTime horaFinRango = new DateTime(data.Fecha.Year, data.Fecha.Month, data.Fecha.Day, 7, 30, 0);
            DateTime finBusqueda = new DateTime(data.Fecha.Year, data.Fecha.Month, data.Fecha.Day, 20, 30, 0);
            Dictionary<int, DatosMapaRetorno> cantPorLugar;
            while (horaFinRango <= finBusqueda)
            {
                cantPorLugar = new Dictionary<int, DatosMapaRetorno>();

                //Llenado con datos del WS
                foreach (DatosMapaWS dato in datosQuery)
                {
                    Console.WriteLine(dato.horaInicio.ToString() + "-" +dato.horaFin.ToString());
                    if (dato.horaInicio <= horaInicioRango.TimeOfDay && dato.horaFin > horaFinRango.TimeOfDay)
                    {
                        string latitud = dato.latitud;
                        string longitud = dato.longitud;

                        /*
                        if (dato.latitud == null || dato.longitud == null)
                        {
                            var espacio = this.context.TBL_Espacio.Where(x => x.idLugarBaseEspol == dato.idLugar).FirstOrDefault();
                            if (espacio != null)
                            {
                                latitud = espacio.latitud;
                                longitud = espacio.longitud;
                            }
                        }*/

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

                /*
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
                */

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

        [HttpPost("estadisticas")]
        public string estadisticas([FromBody] DatosMapaInput data)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            TipoSemana tipoSemana = JsonConvert.DeserializeObject<TipoSemana>(conexionEspol.TipoSemana(data.Fecha).Result);
            string resultado = conexionEspol.estadisticas(data.Fecha, (int)data.Fecha.DayOfWeek, tipoSemana.tipo).Result;
            return resultado;
        }

        [HttpPost("horarioDisponibilidad")]
        public List<Dictionary<int, int>> horarioDisponibilidad([FromBody] DatosHorarioDisponibilidadInput data)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = conexionEspol.horariosPersonas(data.idsPersonas).Result;
            var datos = JsonConvert.DeserializeObject<List<List<HorarioPersona>>>(resultado);
            TipoSemana tipoSemana = JsonConvert.DeserializeObject<TipoSemana>(conexionEspol.TipoSemana(data.fecha).Result);

            DateTime horaInicioRango = new DateTime(data.fecha.Year, data.fecha.Month, data.fecha.Day, 7, 0, 0); //Fecha enviada con 07:00:00
            DateTime horaFinRango = new DateTime(data.fecha.Year, data.fecha.Month, data.fecha.Day, 7, 30, 0);
            DateTime finBusqueda = new DateTime(data.fecha.Year, data.fecha.Month, data.fecha.Day, 20, 30, 0);
            List<Dictionary<int, int>> retorno = new List<Dictionary<int, int>>();
            List<DateTime> horas = new List<DateTime>();
            while (horaFinRango <= finBusqueda)
            {
                horas.Add(horaInicioRango);
                var momento = new Dictionary<int, int>();
                momento.Add(1, 0);
                momento.Add(2, 0);
                momento.Add(3, 0);
                momento.Add(4, 0);
                momento.Add(5, 0);
                momento.Add(6, 0);
                retorno.Add(momento);
                horaInicioRango = horaInicioRango.AddMinutes(30);
                horaFinRango = horaFinRango.AddMinutes(30);
            }
            foreach(var sublista in datos)
            {
                foreach(var item in sublista)
                {
                    if(item.horarioTipo == tipoSemana.tipo)
                    {
                        List<int> indices = new List<int>();
                        foreach(var hora in horas)
                        {
                            if (item.horarioHoraInicio.Value.TimeOfDay <= hora.TimeOfDay && hora.TimeOfDay < item.horarioHoraFin.Value.TimeOfDay) indices.Add(horas.IndexOf(hora));
                        }
                        foreach(var i in indices) retorno[i][item.horarioDia] += 1;
                    }
                    
                }
            }

            return retorno;
        }

        //Vale
        [HttpPost("cursosRelacionados")]
        public string cursosRelacionados([FromBody] IdPersona data)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = conexionEspol.cursosRelacionados(data.idPersona).Result;
            return resultado;
        }

        [HttpPost("estudiantesPorCarrera")]
        public string estudiantesPorCarrera([FromBody] IdCarrera data )
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = conexionEspol.estudiantesPorCarrera(data.idCarrera).Result;
            return resultado;
        }

        [HttpPost("estudiantesPorFacultad")]
        public string estudiantesPorFacultad([FromBody] IdFacultad data)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = conexionEspol.estudiantesPorFacultad(data.idFacultad).Result;
            return resultado;
        }

        [HttpPost("estudiantesPorCurso")]
        public string estudiantesPorCurso([FromBody] IdCurso data)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = conexionEspol.estudiantesPorCurso(data.idCurso).Result;
            return resultado;
        }

        [HttpPost("profesoresPorFacultad")]
        public string profesoresPorFacultad([FromBody] IdFacultad data)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = conexionEspol.profesoresPorFacultad(data.idFacultad).Result;
            return resultado;
        }

        [HttpPost("dirigentesFacultad")]
        public List<DatosPersonaWS> dirigentesFacultad([FromForm] int idFacultad)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado1 = conexionEspol.decanoFacultad(idFacultad).Result;
            var datosQuery1 = JsonConvert.DeserializeObject<List<DatosPersonaWS>>(resultado1);
            string resultado2 = conexionEspol.subdecanoFacultad(idFacultad).Result;
            var datosQuery2 = JsonConvert.DeserializeObject<List<DatosPersonaWS>>(resultado2);

            return datosQuery1.Concat(datosQuery2).ToList();
        }

    }
}