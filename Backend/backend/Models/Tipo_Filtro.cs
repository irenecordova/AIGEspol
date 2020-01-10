using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("TIPO_FILTRO")]
    public class Tipo_Filtro
    {
        [Key]
        [Column("ID")]
        public long id { get; set; }

        [Column("CRITERIO")]
        public string criterio { get; set; }

    }
}
