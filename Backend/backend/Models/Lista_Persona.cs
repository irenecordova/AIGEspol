using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Services;
using Newtonsoft.Json;

namespace backend.Models
{
    [Table("LISTA_PERSONA")]
    public class Lista_Persona
    {
        [Key]
        [Column("IDLISTAPERSONA")]
        public virtual int id { get; set; }

        [Column("IDLISTA")]
        public virtual int idLista { get; set; }

        [Column("IDPERSONA")]
        public virtual int idPersona { get; set; }

        [Column("NOMBREPERSONA")]
        public virtual string nombrePersona { get; set; }

        /*
        public DatosPersona datosPersona()
        {
            string resultado = new ConexionEspol().datosPersona(this.idPersona).Result;
            var dict = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(resultado);
            return new DatosPersona {
                idPersona = dict["intIdPersona"],
                nombre = dict["strNombres"].trim() + " " + dict["strApellidos"].trim()
            };
        }*/
    }
}
