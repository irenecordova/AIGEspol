using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("NOMBRE_ESPACIO")]
    public class Nombre_Espacio
    {
        [Key]
        [Column("ID")]
        public int id { get; set; }

        [Column("ESPACIOID")]
        public int espacioId { get; set; }

        [Column("NOMBRE")]
        public string nombre { get; set; }
    }
}
