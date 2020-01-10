using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
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
        public IEnumerable<Reunion> GetReunion(long id)
        {
            return context.TBL_Reunion.Where(x => x.id == id).ToList();
        }

        [HttpPost]
        public IActionResult InsertarReunion([FromForm] string data)
        {
            Dictionary<string, dynamic> dicc = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(data);
            Reunion lista = new Reunion
            {
                idPersonaCreadora = dicc["idPersona"]
            };
            context.Add(lista);
            context.SaveChanges();

            
            //Creación de anidadas


            Dictionary<string, long> resultado = new Dictionary<string, long>();
            resultado.Add("idInsertado", lista.id);
            return Ok(resultado);
        }
    }
}