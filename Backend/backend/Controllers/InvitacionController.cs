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

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class InvitacionController : ControllerBase
    {
        private readonly ContextAIG context;

        public InvitacionController(ContextAIG context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Invitacion> GetInvitaciones()
        {
            return context.TBL_Invitacion.ToList();
        }

        [HttpGet("{id}")]
        public Invitacion GetInvitacion(int id)
        {

            return context.TBL_Invitacion.Where(x => x.idInvitacion == id).FirstOrDefault();
        }

        [HttpPost("reuniones")]
        public IEnumerable<RetornoInvitacionDetallada> GetReunionesInvitadas(IdPersona data)
        {
            List<RetornoInvitacionDetallada> reuniones = new List<RetornoInvitacionDetallada>();
            var invitaciones = context.TBL_Invitacion.Where(x => x.idPersona == data.idPersona && x.cancelada == "F" ).ToList();
            foreach(Invitacion invitacion in invitaciones)
            {
                reuniones.Add(new RetornoInvitacionDetallada(invitacion, context));
            }
            return reuniones;
        }

        [HttpPost("pendientes")]
        public IEnumerable<RetornoInvitacionDetallada> GetInvitacionesPendientes(IdPersona data)
        {
            List<RetornoInvitacionDetallada> invitaciones = new List<RetornoInvitacionDetallada>();
            var invitacionesPendientes = context.TBL_Invitacion.Where(x => x.idPersona == data.idPersona && x.estado == "E" && x.cancelada == "F").ToList();
            foreach(Invitacion invitacion in invitacionesPendientes)
            {
                invitaciones.Add(new RetornoInvitacionDetallada(invitacion,context));
            }
            return invitaciones;
        } 

        [HttpPost("aceptar")]
        public RetornoResultado Aceptar(IdInvitacion data)
        {
            Invitacion invitacion = context.TBL_Invitacion.Where(x => x.idInvitacion == data.idInvitacion).FirstOrDefault();
            if (invitacion != null)
            {
                Reunion reunion = context.TBL_Reunion.Where(x => x.id == invitacion.idReunion).FirstOrDefault();
                if (reunion.fechaInicio < DateTime.Now && DateTime.Now < reunion.fechaFin) return new RetornoResultado
                {
                    mensaje = "No se puede cambiar el estado de esta invitación. La reunión ya empezó.",
                    error = 1
                };
                if (reunion.fechaFin <= DateTime.Now) return new RetornoResultado
                {
                    mensaje = "No se puede cambiar el estado de esta invitación. La reunión ya culminó.",
                    error = 1
                };
                invitacion.estado = "A";
                context.SaveChanges();
                return new RetornoResultado
                {
                    mensaje = "Ha aceptado la invitación.",
                    error = 0
                };
            }
            return new RetornoResultado
            {
                mensaje = "No existe esa invitación.",
                error = 1
            };
        }

        [HttpPost("rechazar")]
        public RetornoResultado Rechazar(IdInvitacion data)
        {
            Invitacion invitacion = context.TBL_Invitacion.Where(x => x.idInvitacion == data.idInvitacion).FirstOrDefault();
            if (invitacion != null)
            {
                Reunion reunion = context.TBL_Reunion.Where(x => x.id == invitacion.idReunion).FirstOrDefault();
                if (reunion.fechaInicio < DateTime.Now && DateTime.Now < reunion.fechaFin) return new RetornoResultado
                {
                    mensaje = "No se puede cambiar el estado de esta invitación. La reunión ya empezó.",
                    error = 1
                };
                if (reunion.fechaFin <= DateTime.Now) return new RetornoResultado
                {
                    mensaje = "No se puede cambiar el estado de esta invitación. La reunión ya culminó.",
                    error = 1
                };
                invitacion.estado = "R";
                context.SaveChanges();
                return new RetornoResultado
                {
                    mensaje = "Ha rechazado la invitación.",
                    error = 0
                };
            }
            return new RetornoResultado
            {
                mensaje = "No existe esa invitación.",
                error = 1
            };
        }

        [HttpPost("cancelar")]
        public RetornoResultado Cancelar(IdInvitacion data)
        {
            Invitacion invitacion = context.TBL_Invitacion.Where(x => x.idInvitacion == data.idInvitacion).FirstOrDefault();
            if (invitacion != null)
            {
                Reunion reunion = context.TBL_Reunion.Where(x => x.id == invitacion.idReunion).FirstOrDefault();
                if (reunion.fechaInicio < DateTime.Now && DateTime.Now < reunion.fechaFin) return new RetornoResultado
                {
                    mensaje = "No se puede cambiar el estado de esta invitación. La reunión ya empezó.",
                    error = 1
                };
                if (reunion.fechaFin <= DateTime.Now) return new RetornoResultado
                {
                    mensaje = "No se puede cambiar el estado de esta invitación. La reunión ya culminó.",
                    error = 1
                };
                invitacion.cancelada = "T";
                context.SaveChanges();
                return new RetornoResultado
                {
                    mensaje = "Ha cancelado la invitación.",
                    error = 0
                };
            }
            return new RetornoResultado
            {
                mensaje = "No existe esa invitación.",
                error = 1
            };
        }
    }
}