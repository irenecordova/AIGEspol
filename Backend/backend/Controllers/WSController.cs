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
        public List<DatosMapaRetorno> datosMapa([FromBody] DatosMapaInput data)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            List<DatosMapaWS> datosQuery = JsonConvert.DeserializeObject<List<DatosMapaWS>>(conexionEspol.datosMapa(data.Fecha).Result);
            Dictionary<int, DatosMapaRetorno> cantPorLugar = new Dictionary<int, DatosMapaRetorno>();
            Dictionary<int, List<string>> latsYLongs = new Dictionary<int, List<string>>();
            Dictionary<int, int> hijoPadre = new Dictionary<int, int>();
            foreach (var dato in datosQuery)
            {
                int idUsar = dato.idLugar;
                string latitud = dato.latitud;
                string longitud = dato.longitud;
                
                //Si es null, busco en la base local o en los diccionarios
                if (latitud == null || longitud == null)
                {
                    if (hijoPadre.ContainsKey(dato.idLugar))
                    {
                        latitud = latsYLongs[hijoPadre[dato.idLugar]][0];
                        longitud = latsYLongs[hijoPadre[dato.idLugar]][1];
                        idUsar = hijoPadre[dato.idLugar];
                    }
                    else if (latsYLongs.ContainsKey(dato.idLugar))
                    {
                        latitud = latsYLongs[dato.idLugar][0];
                        longitud = latsYLongs[dato.idLugar][1];
                    }
                    else
                    {
                        var latLong = this.buscarLatitudYLongitud(dato.idLugar);
                        latitud = latLong[0];
                        longitud = latLong[1];
                        if(latitud!=null && longitud != null)
                        {
                            latsYLongs.Add(dato.idLugar, latLong);
                        }
                    }
                }

                //Si sigue siendo null, busco la información del padre en la base local y en la base de espol
                if ((latitud == null || longitud == null) && !hijoPadre.ContainsKey(dato.idLugar))
                {
                    var idPadre = JsonConvert.DeserializeObject<IdPadre>(conexionEspol.idLugarPadre(dato.idLugar).Result);
                    var latLong = this.buscarLatitudYLongitud(idPadre.idPadre, true);
                    latitud = latLong[0];
                    longitud = latLong[1];
                    if (!latsYLongs.ContainsKey(idPadre.idPadre)) latsYLongs.Add(idPadre.idPadre, latLong);
                    hijoPadre.Add(dato.idLugar, idPadre.idPadre);
                    idUsar = hijoPadre[dato.idLugar];
                }

                if (!cantPorLugar.ContainsKey(idUsar))
                {
                    cantPorLugar.Add(idUsar, new DatosMapaRetorno
                    {
                        lat = latitud,
                        lng = longitud,
                        count = 0,
                    });
                }
                cantPorLugar[idUsar].count += dato.numRegistrados;

            }

            var reuniones = context.TBL_Reunion.Where(x => x.cancelada == "F" && x.fechaInicio <= data.Fecha && x.fechaFin > data.Fecha).ToList();
            foreach (Reunion reunion in reuniones)
            {
                int idUsar = reunion.idLugar;
                string latitud = null;
                string longitud = null;

                if (hijoPadre.ContainsKey(reunion.idLugar))
                {
                    latitud = latsYLongs[hijoPadre[reunion.idLugar]][0];
                    longitud = latsYLongs[hijoPadre[reunion.idLugar]][1];
                    idUsar = hijoPadre[reunion.idLugar];
                }
                else if (latsYLongs.ContainsKey(reunion.idLugar))
                {
                    latitud = latsYLongs[reunion.idLugar][0];
                    longitud = latsYLongs[reunion.idLugar][1];
                }
                else
                {
                    var latLong = this.buscarLatitudYLongitud(reunion.idLugar);
                    latitud = latLong[0];
                    longitud = latLong[1];
                    if (latitud != null && longitud != null) latsYLongs.Add(reunion.idLugar, latLong);
                }

                //Si sigue siendo null, busco la información del padre en la base local y en la base de espol
                if ((latitud == null || longitud == null) && !hijoPadre.ContainsKey(reunion.idLugar))
                {
                    var idPadre = JsonConvert.DeserializeObject<IdPadre>(conexionEspol.idLugarPadre(reunion.idLugar).Result);
                    var latLong = this.buscarLatitudYLongitud(idPadre.idPadre, true);
                    latitud = latLong[0];
                    longitud = latLong[1];
                    if (!latsYLongs.ContainsKey(idPadre.idPadre))  latsYLongs.Add(idPadre.idPadre, latLong);
                    hijoPadre.Add(reunion.idLugar, idPadre.idPadre);
                    idUsar = hijoPadre[reunion.idLugar];
                }

                if (!cantPorLugar.ContainsKey(idUsar))
                {
                    cantPorLugar.Add(idUsar, new DatosMapaRetorno
                    {
                        lat = latitud,
                        lng = longitud,
                        count = 0,
                    });
                }
                cantPorLugar[idUsar].count += this.context.TBL_Invitacion.Where(x => x.idReunion == reunion.id && x.estado == "A" && x.cancelada == "F").Count() + 1; //+1 por el id del creador;

            }

            return cantPorLugar.Values.ToList();
        }

        // Busca la latitud y longitud en la tabla de lugares de la espol y de la base local, buscando, de ser necesario, en el padre también
        public List<string> buscarLatitudYLongitud(int idLugar, bool buscarEnEspol = false)
        {
            List<string> retorno = new List<string>();
            string latitud = null;
            string longitud = null;

            var espacio = this.context.TBL_Espacio.Where(x => x.idLugarBaseEspol == idLugar).FirstOrDefault();
            if (espacio != null)
            {
                latitud = espacio.latitud;
                longitud = espacio.longitud;
            }

            if ((latitud == null || longitud == null) && buscarEnEspol)
            {
                var conexionEspol = new ConexionEspol();
                var lugar = JsonConvert.DeserializeObject<DatosLugar>(conexionEspol.Lugar(idLugar).Result);
                if (lugar != null)
                {
                    latitud = lugar.strLatitud;
                    longitud = lugar.strLongitud;
                }
            }
            retorno.Add(latitud);
            retorno.Add(longitud);
            return retorno;
        }

        //Vale
        [HttpPost("estadisticas")]
        public string estadisticas([FromBody] DatosMapaInput data)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            TipoSemana tipoSemana = JsonConvert.DeserializeObject<TipoSemana>(conexionEspol.TipoSemana(data.Fecha).Result);
            string resultado = conexionEspol.estadisticas(data.Fecha).Result;
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
            
            foreach (int idPersona in data.idsPersonas)
            {
                if (!nombresPersonas.ContainsKey(idPersona)) {
                    var per = JsonConvert.DeserializeObject<List<ClasePersona>>(conexionEspol.infoPersona(idPersona).Result);
                    if (per.Count > 0) nombresPersonas.Add(idPersona,per[0].strNombres + " " + per[0].strApellidos);
                }
                var reunionesPersona = new ReunionController(context).ReunionesAsistir(new IdPersona { idPersona = idPersona });
                foreach (Reunion reunion in reunionesPersona)
                {
                    List<int> indices = new List<int>();
                    foreach (var hora in horas)
                    {
                        if (reunion.fechaInicio <= hora && hora < reunion.fechaFin) indices.Add(horas.IndexOf(hora));
                    }
                    foreach (var i in indices)
                    {
                        var datosMomento = retorno[i][(int)reunion.fechaInicio.DayOfWeek];
                        if (!datosMomento.idsPersonas.Contains(idPersona))
                        {
                            datosMomento.numOcupados += 1;
                            datosMomento.nombresPersonas.Add(nombresPersonas.GetValueOrDefault(idPersona));
                            datosMomento.idsPersonas.Add(idPersona);
                        }
                    }
                }
            }

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

        [HttpPost("aulasDisponibles")]
        public List<WsInfoLugaresAgendamiento> Disponibles([FromBody] InLugaresDisponibles data)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            var resultado = JsonConvert.DeserializeObject<List<WsInfoLugaresAgendamiento>>(conexionEspol.aulasDisponibles(data.fechaInicio).Result); //Para probar debido al error
            //var resultado = JsonConvert.DeserializeObject<WsInfoLugaresAgendamiento>(conexionEspol.aulasDisponibles(data.fechaInicio, data.fechaFin).Result); //Para probar debido al error

            var idsLugaresUsadosReunion = context.TBL_Reunion.Where(x => (
                x.fechaInicio >= data.fechaInicio && x.fechaInicio < data.fechaFin)
                || (x.fechaFin >= data.fechaInicio && x.fechaFin < data.fechaFin)
            ).Select(x => x.idLugar).Distinct().ToList();
            
            return resultado.Where(x => x.idPadre == data.idBloque && !idsLugaresUsadosReunion.Contains(x.idLugar)).ToList();
        }

        [HttpPost("correosPersonas")]
        public string CorreosPersonas([FromBody] IdsPersonas data)
        {
            ConexionEspol conexionEspol = new ConexionEspol();
            return conexionEspol.correosPersonas(data.ids).Result;
        }

    }
}