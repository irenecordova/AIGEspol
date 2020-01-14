using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Services;

namespace backend.Models.Retornos
{
    public class RetornoListaPersonalizada
    {
        public int id { get; set; }
        public int idPersona { get; set; }
        public string nombre { get; set; }
        public List<Lista_Persona> enlistados { get; set; }

        public RetornoListaPersonalizada(Lista_Personalizada lista, ContextAIG context)
        {
            this.id = lista.id;
            this.idPersona = lista.idPersona;
            this.nombre = lista.nombre;
            this.enlistados = context.TBL_Lista_Persona.Where(x => x.idLista == lista.id).ToList();
        }
    }
}
