using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("TIPO_ESPACIO")]
    public class Tipo_Espacio
    {
        [Key]
        [Column("ID")]
        public long id { get; set; }

        [Column("NOMBRE")]
        public string nombre { get; set; }

    }
}
