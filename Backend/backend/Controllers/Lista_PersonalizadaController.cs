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
using backend.Models.Envios;
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

        /*
        [HttpGet("{id}/prueba")]
        public IQueryable pruebaReunion(int id)
        {
            var query =
                from lista in context.TBL_Lista_Personalizada
                join lp in context.TBL_Lista_Persona on lista.id equals lp.idLista
                where lista.id == id
                select new
                {
                    idLista = lista.id,
                    idLp = lp.id,
                };
            return query;
        }*/

        //Listas que ha creado la persona
        
        [HttpPost("persona")]
        public IEnumerable<Lista_Personalizada> GetListasPorPersona(IdPersona data)
        {
            /*List<RetornoListaPersonalizada> retorno = new List<RetornoListaPersonalizada>();
            foreach(Lista_Personalizada lista in context.TBL_Lista_Personalizada.Where(x => x.idPersona == data.idPersona).ToList())
            {
                retorno.Add(new RetornoListaPersonalizada(lista, context));
            }*/
            return context.TBL_Lista_Personalizada.Where(x => x.idPersona == data.idPersona).ToList();
        }
        
        //Obtener personas en una lista (Con nombres en base)
        [HttpGet("{id}/personas")]
        public IQueryable GetPersonasEnListaBase(int id)
        {
            var query =
                from lp in context.TBL_Lista_Persona
                join lista in context.TBL_Lista_Personalizada on lp.idLista equals lista.id
                where lista.id == id
                select new
                {
                    idPersona = lp.idPersona,
                    nombre = lp.nombrePersona
                };

            return query;
        }

        [HttpPost]
        public string InsertarListaPersonalizada([FromBody] DatosListaPersonalizada data)
        {
            //Dictionary<string, dynamic> dicc = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(data);
            Lista_Personalizada lista = new Lista_Personalizada
            {
                idPersona = data.idCreador,
                nombre = data.nombre
            };
            context.Add(lista);
            context.SaveChanges();

            //Creación de anidadas
            for (int i = 0; i < data.idPersonas.Count(); i++)
            {
                Lista_Persona detalle = new Lista_Persona
                {
                    idLista = lista.id,
                    idPersona = data.idPersonas[i],
                    nombrePersona = data.nombrePersonas[i]
                };
                context.Add(detalle);
            }
            context.SaveChanges();

            Dictionary<string, Lista_Personalizada> resultado = new Dictionary<string, Lista_Personalizada>();
            resultado.Add("listaInsertada", lista);
            return JsonConvert.SerializeObject(resultado);
        }

    }
    //7:30-8:00
}