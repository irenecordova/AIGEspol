using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Models.Envios;
using backend.Models.Retornos;
using backend.Services;
using backend.Tools;
using Newtonsoft.Json;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ReunionController : ControllerBase
    {
        private readonly ContextAIG context;

        public ReunionController(ContextAIG context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Reunion> GetReuniones()
        {
            return context.TBL_Reunion.ToList();
        }

        [HttpGet("{id}")]
        public RetornoReunion GetReunion(int id)
        {
            Reunion reunion = context.TBL_Reunion.Where(x => x.id == id).FirstOrDefault();
            return new RetornoReunion(reunion, this.context);
        }

        [HttpPost]
        public IActionResult InsertarReunion([FromBody] DatosReunion data)
        {
            //Dictionary<string, dynamic> dicc = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(data);
            //Console.WriteLine("AAHH");
            Reunion reunion = new Reunion
            {
                idCreador = data.idCreador,
                asunto = data.asunto,
                descripcion = data.descripcion,
                idLugar = data.idLugar,
                fechaInicio = data.fechaInicio,
                fechaFin = data.fechaFin,
                cancelada = "F"
            };
            context.Add(reunion);
            context.SaveChanges();

            //Creación de invitaciones
            for (int i = 0; i < data.idPersonas.Count(); i++)
            {
                if(data.idCreador != data.idPersonas[i])
                {
                    Invitacion detalle = new Invitacion
                    {
                        idReunion = reunion.id,
                        idPersona = data.idPersonas[i],
                        //nombrePersona = data.nombrePersonas[i],
                        estado = "E",
                        cancelada = "F",
                    };
                    context.Add(detalle);
                }
            }
            context.SaveChanges();

            //Console.WriteLine("EEHH");
            Dictionary<string, int> resultado = new Dictionary<string, int>();
            resultado.Add("idInsertado", reunion.id);
            return Ok(resultado);
        }

        //Reuniones que ha creado una persona
        [HttpGet("usuario/{idPersona}")]
        public IEnumerable<RetornoReunion> ReunionesCreadas(int idPersona)
        {
            List<RetornoReunion> retorno = new List<RetornoReunion>();
            foreach (Reunion reunion in context.TBL_Reunion.Where(x => x.idCreador == idPersona).ToList())
            {
                retorno.Add(new RetornoReunion(reunion, context));
            }
            return retorno;
        }

        /*
        [HttpGet("{id}/personasInvitadas")]
        public IQueryable GetPersonasInvitadasReunion(int id)
        {
            var query =
                from reunion in context.TBL_Reunion
                join invitacion in context.TBL_Invitacion on reunion.id equals invitacion.idReunion
                where reunion.id == id
                select new
                {
                    idPersona = invitacion.idPersona,
                    nombre = invitacion.nombrePersona
                };
            return query;
        }

        [HttpGet("{id}/personaCreadora")]
        public IQueryable GetPersonaCreadora(int id)
        {
            var query = 
            return query;
        }*/

        //Reuniones a las que va a asistir una persona
        [HttpPost("reunionesAsistir")]
        public IEnumerable<Reunion> ReunionesAsistir(IdPersona data)
        {
            List<Reunion> retorno = context.TBL_Reunion.Where(x => x.idCreador == data.idPersona && x.cancelada == "F").ToList();
            var query =
                from reunion in context.TBL_Reunion
                join invitacion in context.TBL_Invitacion on reunion.id equals invitacion.idReunion
                where invitacion.estado == "A" && invitacion.cancelada == "F" && reunion.cancelada == "F" && invitacion.idPersona == data.idPersona
                select new
                {
                    reunion,
                    invitacion
                    /*};
                foreach (var objeto in query){
                    retorno.Add(objeto.reunion);
                }*/
                }.reunion;

            return retorno.Concat(query.ToList());
        }

        [HttpGet("{id}/invitaciones")]
        public IEnumerable<Invitacion> invitaciones(int id)
        {
            return context.TBL_Invitacion.Where(x => x.idReunion == id).ToList();
        }

        [HttpPost("cancelar")]
        public RetornoResultado Cancelar(IdReunion data)
        {
            Reunion reunion = context.TBL_Reunion.Where(x => x.id == data.idReunion).FirstOrDefault();
            if (reunion != null)
            {
                reunion.cancelada = "T";
                var invitaciones = context.TBL_Invitacion.Where(x => x.idReunion == data.idReunion).ToList();
                foreach(var invitacion in invitaciones)
                {
                    invitacion.cancelada = "T";
                }
                context.SaveChanges();
                return new RetornoResultado
                {
                    mensaje = "Ha cancelado la reunión.",
                    error = 0
                };
            }
            return new RetornoResultado
            {
                mensaje = "No existe esa reunión.",
                error = 1
            };
        }

    }
}