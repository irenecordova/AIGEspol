using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("REUNION")]
    public class Reunion
    {
        [Key]
        [Column("ID")]
        public int id { get; set; }

        [Column("IDCREADOR")]
        public int idCreador { get; set; }

        [Column("CANCELADA")]
        public string cancelada { get; set; }

        [Column("ASUNTO")]
        public string asunto { get; set; }

        [Column("DESCRIPCION")]
        public string descripcion { get; set; }

        [Column("IDLUGAR")]
        public int idLugar { get; set; }

        [Column("FECHAINICIO")]
        public Nullable<DateTime> fechaInicio { get; set; }

        [Column("FECHAFIN")]
        public Nullable<DateTime> fechaFin { get; set; }

        [Column("IDPERIODO")]
        public Nullable<int> idPeriodo { get; set; }
    }
}
