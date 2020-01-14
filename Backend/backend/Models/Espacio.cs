using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("ESPACIO")]
    public class Espacio
    {
        [Key]
        [Column("ID")]
        public int id { get; set; }

        [Column("DESCRIPCION")]
        public int descripcion { get; set; }

        [Column("LATITUD")]
        public string latitud { get; set; }
        
        [Column("LONGITUD")]
        public string longitud { get; set; }

        [Column("IDLUGARBASEESPOL")]
        public int idLugarBaseEspol { get; set; }

    }
}
