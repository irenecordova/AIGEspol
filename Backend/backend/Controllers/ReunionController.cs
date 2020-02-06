using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Models.Envios;
using backend.Models.Retornos;
using backend.Services;
//using Outlook = Microsoft.Office.Interop.Outlook;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Net;

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
                        estado = "E",
                        cancelada = "F",
                    };
                    context.Add(detalle);
                }
            }
            context.SaveChanges();

            RetornoReunion result = new RetornoReunion(reunion, context);
            string correos = "igcordov@espol.edu.ec, larizaga@espol.edu.ec";
            Correo(result, correos);
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

        [HttpGet("correo")]
        public void Correo(RetornoReunion reunion, string correos)
        {
            DateTime fecha_inicio = (DateTime) reunion.fechaInicio;
            DateTime fecha_fin = (DateTime) reunion.fechaFin;

            MailMessage msg = new MailMessage();
            //Now we have to set the value to Mail message properties

            //Note Please change it to correct mail-id to use this in your application
            msg.From = new MailAddress("igcordov@espol.edu.ec", "if171a");
            msg.To.Add(correos);
            msg.Subject = reunion.asunto;
            msg.Body = reunion.descripcion;
            msg.Headers.Add("Content-class", "urn:content-classes:calendarmessage");

            // Now Contruct the ICS file using string builder
            StringBuilder str = new StringBuilder();
            str.AppendLine("BEGIN:VCALENDAR");
            str.AppendLine("PRODID:-//Schedule a Meeting");
            str.AppendLine("VERSION:2.0");
            str.AppendLine("METHOD:REQUEST");
            str.AppendLine("BEGIN:VEVENT");
            //str.AppendLine("DTSTAMP:" + reunion.fechaInicio.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"));
            
            str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", fecha_inicio.AddHours(5)));
            str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", fecha_inicio));
            str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", fecha_fin.AddHours(5)));
            str.AppendLine("LOCATION: " + reunion.nombreLugar);
            str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));
            str.AppendLine(string.Format("DESCRIPTION:{0}", msg.Body));
            str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", msg.Body));
            str.AppendLine(string.Format("SUMMARY:{0}", msg.Subject));
            str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", msg.From.Address));

            str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", msg.To[0].DisplayName, msg.To[0].Address));

            str.AppendLine("BEGIN:VALARM");
            str.AppendLine("TRIGGER:-PT15M");
            str.AppendLine("ACTION:DISPLAY");
            str.AppendLine("DESCRIPTION:Reminder");
            str.AppendLine("END:VALARM");
            str.AppendLine("END:VEVENT");
            str.AppendLine("END:VCALENDAR");

            //Now sending a mail with attachment ICS file.                     
            //SmtpClient smtpclient = new SmtpClient();
            //smtpclient.Host = "smtp-mail.outlook.com"; //-------this has to given the Mailserver IP

            SmtpClient client = new SmtpClient();
            client.Host = "smtp.office365.com";
            client.Credentials = new NetworkCredential("igcordov@espol.edu.ec", "if171a");
            client.EnableSsl = true;
            client.Port = 587;

            ContentType contype = new ContentType("text/calendar");
            contype.Parameters.Add("method", "REQUEST");
            contype.Parameters.Add("name", "Meeting.ics");
            AlternateView avCal = AlternateView.CreateAlternateViewFromString(str.ToString(), contype);
            msg.AlternateViews.Add(avCal);
            try
            {
                client.Send(msg);
                msg.Dispose();
                
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}