using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Services;
using backend.Tools;
using backend.Models.Retornos;
using Newtonsoft.Json;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class Lista_PersonalizadaController : ControllerBase
    {
        private readonly ContextAIG context;

        public Lista_PersonalizadaController(ContextAIG context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Lista_Personalizada> GetListasPersonalizadas()
        {
            return context.TBL_Lista_Personalizada.ToList();
        }

        [HttpGet("{id}")]
        public RetornoListaPersonalizada GetListaPersonalizada(int id)
        {
            Lista_Personalizada lista = context.TBL_Lista_Personalizada.Where(x => x.id == id).FirstOrDefault();
            return new RetornoListaPersonalizada(lista, this.context);
        }

        [HttpPost]
        public IActionResult InsertarListaPersonalizada([FromForm] string data)
        {
            Dictionary<string, dynamic> dicc = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(data);
            Lista_Personalizada lista = new Lista_Personalizada
            {
                idPersona = dicc["idDueño"],
                nombre = dicc["nombre"]
            };
            context.Add(lista);
            context.SaveChanges();

            //Creación de anidadas
            for (int i = 0; i < dicc.Count(); i++)
            {
                Lista_Persona detalle = new Lista_Persona
                {
                    idLista = lista.id,
                    idPersona = dicc["idPersonas"][i],
                    nombrePersona = dicc["nombresPersonas"][i]
                };
            }

            Dictionary<string, int> resultado = new Dictionary<string, int>();
            resultado.Add("idInsertado", lista.id);
            return Ok(resultado);
        }
    }
}