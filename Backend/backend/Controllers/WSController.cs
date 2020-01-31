﻿using System;
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
            string resultado = conexionEspol.personaPorNombreYApellido(data.nombre).Result;
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
            //Console.WriteLine(resultado);

            List<DatosMapaWS> datosQuery;
            datosQuery = JsonConvert.DeserializeObject<List<DatosMapaWS>>(resultado);

            Dictionary<string, List<DatosMapaRetorno>> retorno = new Dictionary<string, List<DatosMapaRetorno>>();
            DateTime horaInicioRango = new DateTime(data.Fecha.Year, data.Fecha.Month, data.Fecha.Day, 7, 0, 0); //Fecha enviada con 07:00:00
            DateTime horaFinRango = new DateTime(data.Fecha.Year, data.Fecha.Month, data.Fecha.Day, 7, 30, 0);
            DateTime finBusqueda = new DateTime(data.Fecha.Year, data.Fecha.Month, data.Fecha.Day, 20, 30, 0);
            Dictionary<int, DatosMapaRetorno> cantPorLugar;
            Dictionary<int, List<string>> latsYLongsPadres = new Dictionary<int, List<string>>();
            while (horaFinRango <= finBusqueda)
            {
                cantPorLugar = new Dictionary<int, DatosMapaRetorno>();
                Console.WriteLine(horaInicioRango.TimeOfDay.ToString() + "-" + horaFinRango.TimeOfDay.ToString());
                //Llenado con datos del WS
                foreach (DatosMapaWS dato in datosQuery)
                {

                    if (dato.horaInicio <= horaInicioRango.TimeOfDay && dato.horaFin > horaInicioRango.TimeOfDay)
                    {
                        string latitud = dato.latitud;
                        string longitud = dato.longitud;
                        
                        //En caso de que la latitud y la longitud de la base de espol sean null
                        if (dato.latitud == null || dato.longitud == null)
                        {
                            var espacio = this.context.TBL_Espacio.Where(x => x.idLugarBaseEspol == dato.idLugar).FirstOrDefault();
                            if (espacio != null)
                            {
                                latitud = espacio.latitud;
                                longitud = espacio.longitud;
                            }
                        }

                        //Si seguimos teniendo null, hay que buscar la info del padre
                        if (latitud == null || longitud == null)
                        {
                            var idPadre = JsonConvert.DeserializeObject<IdPadre>(conexionEspol.idLugarPadre(dato.idLugar).Result);
                            var lugar = JsonConvert.DeserializeObject<DatosLugar>(conexionEspol.Lugar(idPadre.idPadre).Result);
                            if (lugar.strLatitud == null || lugar.strLongitud == null)
                            {
                                var lugarPadre = this.context.TBL_Espacio.Where(x => x.idLugarBaseEspol == idPadre.idPadre).FirstOrDefault();
                                if (lugarPadre != null)
                                {
                                    latitud = lugarPadre.latitud;
                                    longitud = lugarPadre.longitud;
                                }
                            } else
                            {
                                latitud = lugar.strLatitud;
                                longitud = lugar.strLongitud;
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

                        //Si seguimos teniendo null, hay que buscar la info del padre
                        if (latitud == null || longitud == null)
                        {
                            var idPadre = JsonConvert.DeserializeObject<IdPadre>(conexionEspol.idLugarPadre(reunion.idLugar).Result);
                            var lugar = JsonConvert.DeserializeObject<DatosLugar>(conexionEspol.Lugar(idPadre.idPadre).Result);
                            if (lugar.strLatitud == null || lugar.strLongitud == null)
                            {
                                var lugarPadre = this.context.TBL_Espacio.Where(x => x.idLugarBaseEspol == lugar.intIdLugarEspol).FirstOrDefault();
                                if (lugarPadre != null)
                                {
                                    latitud = lugarPadre.latitud;
                                    longitud = lugarPadre.longitud;
                                }
                            }
                            else
                            {
                                latitud = lugar.strLatitud;
                                longitud = lugar.strLongitud;
                            }
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

        //Vale
        [HttpPost("estadisticas")]
        public string estadisticas([FromBody] DatosMapaInput data)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            TipoSemana tipoSemana = JsonConvert.DeserializeObject<TipoSemana>(conexionEspol.TipoSemana(data.Fecha).Result);
            string resultado = conexionEspol.estadisticas(data.Fecha, (int)data.Fecha.DayOfWeek, tipoSemana.tipo).Result;
            return resultado;
        }

        public class DatosHorarioDisponibilidad
        {
            public int numOcupados { get; set; }
            public List<string> nombresPersonas { get; set; }
            public List<int> idsPersonas { get; set; }

            public DatosHorarioDisponibilidad()
            {
                this.numOcupados = 0;
                this.nombresPersonas = new List<string>();
                this.idsPersonas = new List<int>();
            }
        } 
        [HttpPost("horarioDisponibilidad")]
        public string horarioDisponibilidad([FromBody] DatosHorarioDisponibilidadInput data)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = conexionEspol.horariosPersonas(data.idsPersonas, data.fecha).Result;
            var datos = JsonConvert.DeserializeObject<List<List<HorarioPersona>>>(resultado);
            TipoSemana tipoSemana = JsonConvert.DeserializeObject<TipoSemana>(conexionEspol.TipoSemana(data.fecha).Result);
            DateTime horaInicioRango = new DateTime(data.fecha.Year, data.fecha.Month, data.fecha.Day, 7, 0, 0); //Fecha enviada con 07:00:00
            DateTime horaFinRango = new DateTime(data.fecha.Year, data.fecha.Month, data.fecha.Day, 7, 30, 0);
            DateTime finBusqueda = new DateTime(data.fecha.Year, data.fecha.Month, data.fecha.Day, 20, 30, 0);
            List<Dictionary<int, DatosHorarioDisponibilidad>> retorno = new List<Dictionary<int, DatosHorarioDisponibilidad>>();
            List<DateTime> horas = new List<DateTime>();
            Dictionary<int, string> nombresPersonas = new Dictionary<int, string>();
            while (horaFinRango <= finBusqueda)
            {
                horas.Add(horaInicioRango);
                var momento = new Dictionary<int, DatosHorarioDisponibilidad>();
                for (var i = 1; i < 7; i++)
                {
                    momento.Add(i, new DatosHorarioDisponibilidad());
                }
                retorno.Add(momento);
                horaInicioRango = horaInicioRango.AddMinutes(30);
                horaFinRango = horaFinRango.AddMinutes(30);
            }

            foreach (var sublista in datos)
            {
                foreach(var item in sublista)
                {
                    if (!nombresPersonas.ContainsKey(item.idPersona)) nombresPersonas.Add(item.idPersona, item.nombres + " " + item.apellidos);
                    List<int> indices = new List<int>();
                    foreach(var hora in horas)
                    {
                        if (item.horarioHoraInicio.Value.TimeOfDay <= hora.TimeOfDay && hora.TimeOfDay < item.horarioHoraFin.Value.TimeOfDay) indices.Add(horas.IndexOf(hora));
                    }
                    foreach (var i in indices) {
                        var datosMomento = retorno[i][item.horarioDia];
                        if (!datosMomento.idsPersonas.Contains(item.idPersona))
                        {
                            datosMomento.numOcupados += 1;
                            datosMomento.nombresPersonas.Add(item.nombres.Trim() + " " + item.apellidos.Trim());
                            datosMomento.idsPersonas.Add(item.idPersona);
                        }
                    }
                    
                }
            }
            /*
            foreach (int idPersona in data.idsPersonas)
            {
                var reunionesPersona = new ReunionController(context).ReunionesAsistir(new IdPersona { idPersona = idPersona });
                foreach (Reunion reunion in reunionesPersona)
                {
                    List<int> indices = new List<int>();
                    foreach (var hora in horas)
                    {
                        if (reunion.fechaInicio.TimeOfDay <= hora.TimeOfDay && hora.TimeOfDay < reunion.fechaFin.TimeOfDay) indices.Add(horas.IndexOf(hora));
                    }
                    foreach (var i in indices)
                    {
                        var datosMomento = retorno[i][(int)reunion.fechaInicio.DayOfWeek];
                        if (!datosMomento.idsPersonas.Contains(reunion.id))
                        {
                            datosMomento.numOcupados += 1;
                            datosMomento.nombresPersonas.Add(nombresPersonas.GetValueOrDefault(idPersona));
                            datosMomento.idsPersonas.Add(idPersona);
                        }
                    }
                }
            }*/

            return JsonConvert.SerializeObject(retorno);
        }

        //Vale
        [HttpPost("cursosRelacionados")]
        public string cursosRelacionados([FromBody] IdPersona data)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = conexionEspol.cursosRelacionados(data.idPersona).Result;
            return resultado;
        }

        //Vale
        [HttpPost("estudiantesPorCarrera")]
        public string estudiantesPorCarrera([FromBody] IdCarrera data )
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = conexionEspol.estudiantesPorCarrera(data.idCarrera).Result;
            return resultado;
        }

        //Vale
        [HttpPost("estudiantesPorFacultad")]
        public string estudiantesPorFacultad([FromBody] IdFacultad data)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = conexionEspol.estudiantesPorFacultad(data.idFacultad).Result;
            return resultado;
        }

        //Vale
        [HttpPost("estudiantesPorCurso")]
        public string estudiantesPorCurso([FromBody] IdCurso data)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = conexionEspol.estudiantesPorCurso(data.idCurso).Result;
            return resultado;
        }

        //Vale
        [HttpPost("estudiantesPorMateria")]
        public string estudiantesPorMateria([FromBody] IdMateria data)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = conexionEspol.estudiantesPorMateria(data.idMateria).Result;
            return resultado;
        }

        //Vale
        [HttpPost("profesoresPorFacultad")]
        public string profesoresPorFacultad([FromBody] IdFacultad data)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = conexionEspol.profesoresPorFacultad(data.idFacultad).Result;
            return resultado;
        }

        //Vale
        [HttpPost("profesoresPorMateria")]
        public string profesoresPorMateria([FromBody] IdMateria data)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = conexionEspol.profesoresPorMateria(data.idMateria).Result;
            return resultado;
        }

        //Vale
        [HttpPost("dirigentesFacultad")]
        public List<DatosPersonaWS> dirigentesFacultad([FromBody] IdFacultad data)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado1 = conexionEspol.decanoFacultad(data.idFacultad).Result;
            var datosQuery1 = JsonConvert.DeserializeObject<List<DatosPersonaWS>>(resultado1);
            string resultado2 = conexionEspol.subdecanoFacultad(data.idFacultad).Result;
            var datosQuery2 = JsonConvert.DeserializeObject<List<DatosPersonaWS>>(resultado2);

            return datosQuery1.Concat(datosQuery2).ToList();
        }

        //Vale
        [HttpGet("facultades")]
        public string facultades()
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            return conexionEspol.facultades().Result;
        }
        //Vale
        [HttpGet("carreras")]
        public string carreras()
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            return conexionEspol.carreras().Result;
        }
        //Vale
        [HttpGet("carreras/{idFacultad}")]
        public string carrerasPorFacultad(int idFacultad)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            return conexionEspol.carrerasPorFacultad(idFacultad).Result;
        }
        //Vale
        public class InUsuario
        {
            public string usuario { get; set; }
        }
        [HttpPost("idPersonaPorUsuario")]
        public string idPersonaPorUsuario([FromBody] InUsuario data)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = conexionEspol.idPersonaPorUsuario(data.usuario).Result;
            return resultado;
        }

        //Vale
        [HttpPost("materiasPorFacultad")]
        public string materiasPorFacultad([FromBody] IdFacultad data)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            string resultado = conexionEspol.materiasPorFacultad(data.idFacultad).Result;
            return resultado;
        }

        [HttpGet("profesores")]
        public string profesoresTodos()
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            return conexionEspol.profesoresTodos().Result;
        }

        [HttpGet("directivos")]
        public string directivosTodos()
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            return conexionEspol.directivosTodos().Result;
        }
    }
}