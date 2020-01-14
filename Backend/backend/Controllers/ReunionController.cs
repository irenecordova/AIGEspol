using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
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
        public RetornoReunion GetReunion(long id)
        {
            Reunion reunion = context.TBL_Reunion.Where(x => x.id == id).FirstOrDefault();
            return new RetornoReunion(reunion, this.context);
        }

        [HttpPost]
        public IActionResult InsertarReunion([FromForm] string data)
        {
            Dictionary<string, dynamic> dicc = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(data);
            Reunion reunion = new Reunion
            {
                idCreador = dicc["idCreador"],
                asunto = dicc["asunto"],
                descripcion = dicc["descripcion"],
                idLugar = dicc["idLugar"],
                fecha = dicc["fecha"],
            };
            context.Add(reunion);
            context.SaveChanges();

            //Creación de invitaciones
            foreach (int i in dicc["idPersonas"])
            {
                Invitacion detalle = new Invitacion
                {
                    idReunion = reunion.id,
                    idPersona = i
                };
            }

            Dictionary<string, int> resultado = new Dictionary<string, int>();
            resultado.Add("idInsertado", reunion.id);
            return Ok(resultado);
        }
    }
}