using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("LISTA_PERSONA")]
    public class Lista_Persona
    {
        [Key]
        [Column("IDLISTAPERSONA")]
        public long id { get; set; }

        [Column("IDLISTA")]
        public int idLista { get; set; }

        [Column("IDPERSONA")]
        public string idPersona { get; set; }
    }
}
