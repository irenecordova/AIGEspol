﻿using System;
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
    public class LugarController : ControllerBase
    {
        private readonly CdaDbContextDB2SAAC context;

        public LugarController(CdaDbContextDB2SAAC context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<CdaLugar> Get()
        {
            return context.TBL_LUGAR_ESPOL.Where(x => x.strEstado == "V").ToList();
        }

        [HttpGet("todos")]
        public IEnumerable<CdaLugar> GetTodos()
        {
            return context.TBL_LUGAR_ESPOL.ToList();
        }

        [HttpGet("{id}")]
        public CdaLugar GetById(int id)
        {
            return context.TBL_LUGAR_ESPOL.Where(x => x.intIdLugarEspol == id).FirstOrDefault();
        }

        [HttpGet("Edificios")]
        public IEnumerable<CdaLugar> facultades()
        {
            return context.TBL_LUGAR_ESPOL.Where(x => x.strTipo == "E" && x.strEstado == "V");
        }

        [HttpGet("Aulas")]
        public IEnumerable<CdaLugar> Aulas()
        {
            return context.TBL_LUGAR_ESPOL.Where(x => x.strTipo == "A" && x.strEstado == "V");
        }

        [HttpPost("Aulas/Bloque")]
        public IEnumerable<CdaLugar> AulasPorBloque([FromBody] IdLugar data)
        {
            return context.TBL_LUGAR_ESPOL.Where(x => x.strTipo == "A" && x.strEstado == "V" && data.idLugar == x.intIdLugarPadre);
        }

        [HttpPost("padre")]
        public object idLugarPadre([FromBody] IdLugar data)
        {
            CdaLugar lugar = context.TBL_LUGAR_ESPOL.Where(x => x.intIdLugarEspol == data.idLugar).FirstOrDefault();
            if (lugar.intIdLugarPadre != null)
            {
                return new { idPadre = context.TBL_LUGAR_ESPOL.Where(x => x.intIdLugarEspol == lugar.intIdLugarPadre).FirstOrDefault().intIdLugarEspol };
            }

            return new { idPadre = -1 };
        }

        [HttpPost("ocupados")]
        public IQueryable lugaresOcupados([FromBody] InDataFecha data)
        {
            var periodoController = new PeriodoAcademicoController(context);
            var tipoSemana = periodoController.getTipoSemanaEnPeriodo(new InDataFecha { fecha = data.fecha });
            var periodoActual = periodoController.GetPeriodoFecha(data.fecha);
            string examen = periodoController.tipoExamen(data.fecha);
            int dia = (int)data.fecha.DayOfWeek;
            

            var lugaresOcupados =
                from horario in context.TBL_HORARIO
                where horario.strExamen == examen
                join curso in context.TBL_CURSO on horario.intIdCurso equals curso.intIdCurso
                join lugar in context.TBL_LUGAR_ESPOL on horario.intIdAula equals lugar.intIdLugarEspol
                join padre in context.TBL_LUGAR_ESPOL on lugar.intIdLugarPadre equals padre.intIdLugarEspol
                where lugar.strTipo == "A"
                && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                && horario.intDia == dia
                && horario.chTipo == tipoSemana.tipo
                && horario.tsHoraInicio <= data.fecha.TimeOfDay
                && horario.tsHoraFin > data.fecha.TimeOfDay
                select new
                {
                    idLugar = lugar.intIdLugarEspol,
                    nombreLugar = lugar.strDescripcion,
                    idPadre = padre.intIdLugarEspol,
                    nombrePadre = padre.strDescripcion
                };

            return lugaresOcupados.Distinct();
        }

        [HttpPost("disponibles")]
        public IQueryable obtenerLugaresDisponibles([FromBody] InDataFecha data)
        {
            var periodoController = new PeriodoAcademicoController(context);
            var tipoSemana = periodoController.getTipoSemanaEnPeriodo(new InDataFecha { fecha = data.fecha });
            var periodoActual = periodoController.GetPeriodoFecha(data.fecha);
            string examen = periodoController.tipoExamen(data.fecha);


            var lugaresOcupados = 
                from horario in context.TBL_HORARIO
                where horario.strExamen == examen
                join curso in context.TBL_CURSO on horario.intIdCurso equals curso.intIdCurso
                join lugar in context.TBL_LUGAR_ESPOL on horario.intIdAula equals lugar.intIdLugarEspol
                join padre in context.TBL_LUGAR_ESPOL on lugar.intIdLugarPadre equals padre.intIdLugarEspol
                where lugar.strTipo == "A"
                && horario.chTipo == tipoSemana.tipo
                && horario.tsHoraInicio <= data.fecha.TimeOfDay
                && horario.tsHoraFin > data.fecha.TimeOfDay
                select new
                {
                    idLugar = lugar.intIdLugarEspol,
                    nombreLugar = lugar.strDescripcion,
                    idPadre = padre.intIdLugarEspol,
                    nombrePadre = padre.strDescripcion
                };

            var lugaresOcupadosRecuperaciones = 
                from horario in context.TBL_HORARIO_CONTENIDO
                join lugar in context.TBL_LUGAR_ESPOL on horario.intIdLugarEspol equals lugar.intIdLugarEspol
                join padre in context.TBL_LUGAR_ESPOL on lugar.intIdLugarPadre equals padre.intIdLugarEspol
                where lugar.strTipo == "A"
                && horario.dtFecha == data.fecha.Date
                && horario.tsHoraInicio <= data.fecha.TimeOfDay
                && horario.tsHoraFin <= data.fecha.TimeOfDay
                select new
                {
                    idLugar = lugar.intIdLugarEspol,
                    nombreLugar = lugar.strDescripcion,
                    idPadre = padre.intIdLugarEspol,
                    nombrePadre = padre.strDescripcion
                };

            var infoLugares = 
                from lugar in context.TBL_LUGAR_ESPOL
                join padre in context.TBL_LUGAR_ESPOL on lugar.intIdLugarPadre equals padre.intIdLugarEspol
                where lugar.strTipo == "A" && lugar.strEstado == "V"
                select new
                {
                    idLugar = lugar.intIdLugarEspol,
                    nombreLugar = lugar.strDescripcion,
                    idPadre = padre.intIdLugarEspol,
                    nombrePadre = padre.strDescripcion
                };

            return infoLugares.Except(lugaresOcupados.Union(lugaresOcupadosRecuperaciones)).Distinct().OrderBy(x => x.nombreLugar); ;
        }

        [HttpPost("ocupados/rango")]
        public IQueryable ocupadosRango([FromBody] InDataFechasReunion data)
        {
            var periodoController = new PeriodoAcademicoController(context);
            var tipoSemana = periodoController.getTipoSemanaEnPeriodo(new InDataFecha { fecha = data.fechaInicio });
            var periodoActual = periodoController.GetPeriodoFecha(data.fechaInicio.Date);
            string examen = periodoController.tipoExamen(data.fechaInicio.Date);
            /*List<int> diasTomarEnCuenta = new List<int>();
            var fechaIni = data.fechaInicio.Date;
            var fechaFin = data.fechaFin.Date;
            while (fechaIni < fechaFin)
            {
                if (!diasTomarEnCuenta.Contains((int)data.fechaInicio.DayOfWeek)) diasTomarEnCuenta.Add((int)fechaIni.DayOfWeek);
                fechaIni.AddDays(1);
            }
            if (!diasTomarEnCuenta.Contains((int)data.fechaFin.DayOfWeek)) diasTomarEnCuenta.Add((int)fechaFin.DayOfWeek);
            var dias = diasTomarEnCuenta.AsEnumerable();*/

            var lugaresOcupados =
                from horario in context.TBL_HORARIO
                where horario.strExamen == examen
                join curso in context.TBL_CURSO on horario.intIdCurso equals curso.intIdCurso
                join lugar in context.TBL_LUGAR_ESPOL on horario.intIdAula equals lugar.intIdLugarEspol
                where lugar.strTipo == "A" && lugar.intIdLugarPadre != null
                join padre in context.TBL_LUGAR_ESPOL on lugar.intIdLugarPadre equals padre.intIdLugarEspol
                //join dia in dias on horario.intDia equals (short)dia
                where horario.chTipo == tipoSemana.tipo
                && horario.strExamen == examen
                && horario.intDia == (int)data.fechaInicio.DayOfWeek
                && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                /*&& (
                    (data.fechaInicio.Date.Add(horario.tsHoraInicio.GetValueOrDefault(new TimeSpan(0, 0, 0))) > data.fechaInicio && data.fechaInicio.Date.Add(horario.tsHoraInicio.GetValueOrDefault(new TimeSpan(0, 0, 0))) < data.fechaFin)
                    || (data.fechaInicio.Date.Add(horario.tsHoraFin.GetValueOrDefault(new TimeSpan(0, 0, 0))) > data.fechaInicio && data.fechaInicio.Date.Add(horario.tsHoraFin.GetValueOrDefault(new TimeSpan(0, 0, 0))) < data.fechaFin)
                    || (data.fechaInicio.Date.Add(horario.tsHoraInicio.GetValueOrDefault(new TimeSpan(0, 0, 0))) < data.fechaInicio && data.fechaFin.Date.Add(horario.tsHoraFin.GetValueOrDefault(new TimeSpan(0, 0, 0))) > data.fechaFin)
                )*/
                && (
                    (horario.tsHoraInicio >= data.fechaInicio.TimeOfDay && horario.tsHoraInicio < data.fechaFin.TimeOfDay)
                    || (horario.tsHoraFin > data.fechaInicio.TimeOfDay && horario.tsHoraFin <= data.fechaFin.TimeOfDay)
                    || (horario.tsHoraInicio <= data.fechaInicio.TimeOfDay && horario.tsHoraFin >= data.fechaFin.TimeOfDay)
                )
                select new
                {
                    idLugar = lugar.intIdLugarEspol,
                    nombreLugar = lugar.strDescripcion,
                    idPadre = padre.intIdLugarEspol,
                    nombrePadre = padre.strDescripcion
                };
            
            var lugaresOcupadosRecuperaciones =
                from horario in context.TBL_HORARIO_CONTENIDO
                join lugar in context.TBL_LUGAR_ESPOL on horario.intIdLugarEspol equals lugar.intIdLugarEspol
                join padre in context.TBL_LUGAR_ESPOL on lugar.intIdLugarPadre equals padre.intIdLugarEspol
                where lugar.strTipo == "A"
                && (horario.dtFecha >= data.fechaInicio.Date && horario.dtFecha <= data.fechaFin.Date)
                && (
                    (horario.tsHoraInicio >= data.fechaInicio.TimeOfDay && horario.tsHoraInicio < data.fechaFin.TimeOfDay)
                    || (horario.tsHoraFin > data.fechaInicio.TimeOfDay && horario.tsHoraFin <= data.fechaFin.TimeOfDay)
                    || (horario.tsHoraInicio <= data.fechaInicio.TimeOfDay && horario.tsHoraFin >= data.fechaFin.TimeOfDay)
                )
                select new
                {
                    idLugar = lugar.intIdLugarEspol,
                    nombreLugar = lugar.strDescripcion,
                    idPadre = padre.intIdLugarEspol,
                    nombrePadre = padre.strDescripcion
                };

            return lugaresOcupados.Union(lugaresOcupadosRecuperaciones).Distinct().OrderBy(x => x.nombreLugar); ;
        }
        

        [HttpPost("disponibles/rango")]
        public IQueryable obtenerLugaresDisponiblesRango([FromBody] InDataFechasReunion data)
        {
            var periodoController = new PeriodoAcademicoController(context);
            var tipoSemana = periodoController.getTipoSemanaEnPeriodo(new InDataFecha { fecha = data.fechaInicio });
            var periodoActual = periodoController.GetPeriodoFecha(data.fechaInicio.Date);
            string examen = periodoController.tipoExamen(data.fechaInicio.Date);
            /*List<int> diasTomarEnCuenta = new List<int>();
            var fechaIni = data.fechaInicio.Date;
            var fechaFin = data.fechaFin.Date;
            while (fechaIni < fechaFin)
            {
                if (!diasTomarEnCuenta.Contains((int)data.fechaInicio.DayOfWeek)) diasTomarEnCuenta.Add((int)fechaIni.DayOfWeek);
                fechaIni.AddDays(1);
            }
            if (!diasTomarEnCuenta.Contains((int)data.fechaFin.DayOfWeek)) diasTomarEnCuenta.Add((int)fechaFin.DayOfWeek);
            var dias = diasTomarEnCuenta.AsEnumerable();*/

            var lugaresOcupados =
                from horario in context.TBL_HORARIO
                where horario.strExamen == examen
                join curso in context.TBL_CURSO on horario.intIdCurso equals curso.intIdCurso
                join lugar in context.TBL_LUGAR_ESPOL on horario.intIdAula equals lugar.intIdLugarEspol
                where lugar.strTipo == "A" && lugar.intIdLugarPadre != null
                join padre in context.TBL_LUGAR_ESPOL on lugar.intIdLugarPadre equals padre.intIdLugarEspol
                //join dia in dias on horario.intDia equals (short)dia
                where horario.chTipo == tipoSemana.tipo
                && horario.strExamen == examen
                && horario.intDia == (int)data.fechaInicio.DayOfWeek
                && curso.intIdPeriodo == periodoActual.intIdPeriodoAcademico
                /*&& (
                    (data.fechaInicio.Date.Add(horario.tsHoraInicio.GetValueOrDefault(new TimeSpan(0, 0, 0))) > data.fechaInicio && data.fechaInicio.Date.Add(horario.tsHoraInicio.GetValueOrDefault(new TimeSpan(0, 0, 0))) < data.fechaFin)
                    || (data.fechaInicio.Date.Add(horario.tsHoraFin.GetValueOrDefault(new TimeSpan(0, 0, 0))) > data.fechaInicio && data.fechaInicio.Date.Add(horario.tsHoraFin.GetValueOrDefault(new TimeSpan(0, 0, 0))) < data.fechaFin)
                    || (data.fechaInicio.Date.Add(horario.tsHoraInicio.GetValueOrDefault(new TimeSpan(0, 0, 0))) < data.fechaInicio && data.fechaFin.Date.Add(horario.tsHoraFin.GetValueOrDefault(new TimeSpan(0, 0, 0))) > data.fechaFin)
                )*/
                && (
                    (horario.tsHoraInicio >= data.fechaInicio.TimeOfDay && horario.tsHoraInicio < data.fechaFin.TimeOfDay)
                    || (horario.tsHoraFin > data.fechaInicio.TimeOfDay && horario.tsHoraFin <= data.fechaFin.TimeOfDay)
                    || (horario.tsHoraInicio <= data.fechaInicio.TimeOfDay && horario.tsHoraFin >= data.fechaFin.TimeOfDay)
                )
                select new
                {
                    idLugar = lugar.intIdLugarEspol,
                    nombreLugar = lugar.strDescripcion,
                    idPadre = padre.intIdLugarEspol,
                    nombrePadre = padre.strDescripcion
                };

            var lugaresOcupadosRecuperaciones =
                from horario in context.TBL_HORARIO_CONTENIDO
                join lugar in context.TBL_LUGAR_ESPOL on horario.intIdLugarEspol equals lugar.intIdLugarEspol
                join padre in context.TBL_LUGAR_ESPOL on lugar.intIdLugarPadre equals padre.intIdLugarEspol
                where lugar.strTipo == "A"
                && (horario.dtFecha >= data.fechaInicio.Date && horario.dtFecha <= data.fechaFin.Date)
                && (
                    (horario.tsHoraInicio >= data.fechaInicio.TimeOfDay && horario.tsHoraInicio < data.fechaFin.TimeOfDay)
                    || (horario.tsHoraFin > data.fechaInicio.TimeOfDay && horario.tsHoraFin <= data.fechaFin.TimeOfDay)
                    || (horario.tsHoraInicio <= data.fechaInicio.TimeOfDay && horario.tsHoraFin >= data.fechaFin.TimeOfDay)
                )
                select new
                {
                    idLugar = lugar.intIdLugarEspol,
                    nombreLugar = lugar.strDescripcion,
                    idPadre = padre.intIdLugarEspol,
                    nombrePadre = padre.strDescripcion
                };

            var infoLugares =
                from lugar in context.TBL_LUGAR_ESPOL
                join padre in context.TBL_LUGAR_ESPOL on lugar.intIdLugarPadre equals padre.intIdLugarEspol
                where lugar.strTipo == "A" && lugar.strEstado == "V"
                select new
                {
                    idLugar = lugar.intIdLugarEspol,
                    nombreLugar = lugar.strDescripcion,
                    idPadre = padre.intIdLugarEspol,
                    nombrePadre = padre.strDescripcion
                };

            return infoLugares.Except(lugaresOcupados.Union(lugaresOcupadosRecuperaciones)).Distinct().OrderBy(x => x.nombreLugar);
        }
    }
}