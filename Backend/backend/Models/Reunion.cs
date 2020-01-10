using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("LISTA_PERSONALIZADA")]
    public class Reunion
    {
        [Key]
        [Column("ID")]
        public long id { get; set; }

        [Column("IDPERSONACREADORA")]
        public string idPersonaCreadora { get; set; }

        [Column("ELIMINADO")]
        public string eliminado { get; set; }
    }
}
