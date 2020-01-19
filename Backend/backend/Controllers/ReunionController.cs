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
            Console.WriteLine("AAHH");
            Reunion reunion = new Reunion
            {
                idCreador = data.idCreador,
                asunto = data.asunto,
                descripcion = data.descripcion,
                idLugar = data.idLugar,
                fechaInicio = DateTime.Now,
                fechaFin = DateTime.Now,
                cancelada = "F"
            };
            context.Add(reunion);
            context.SaveChanges();

            //Creación de invitaciones
            foreach (int i in data.idPersonas)
            {
                Invitacion detalle = new Invitacion
                {
                    idReunion = reunion.id,
                    idPersona = i,
                    estado = "E",
                    cancelada = "F",
                };
                context.Add(detalle);
            }
            context.SaveChanges();

            Console.WriteLine("EEHH");
            Dictionary<string, int> resultado = new Dictionary<string, int>();
            resultado.Add("idInsertado", reunion.id);
            return Ok(resultado);
        }
    }
}