using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("LISTA_PERSONALIZADA")]
    public class Lista_Personalizada
    {
        [Key]
        [Column("ID")]
        public virtual int id { get; set; }

        [Column("IDPERSONA")]
        public virtual int idPersona { get; set; }

        [Column("NOMBRE")]
        public virtual string nombre { get; set; }

        //public List<Lista_Persona> lista_personas { get; set; }

        /*
        public Lista_Personalizada()
        {
            lista_personas = new List<Lista_Persona>();
        }
        */
    }
}
