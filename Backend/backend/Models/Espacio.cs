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
        public virtual int id { get; set; }

        [Column("DESCRIPCION")]
        public virtual int descripcion { get; set; }

        [Column("LATITUD")]
        public virtual string latitud { get; set; }
        
        [Column("LONGITUD")]
        public virtual string longitud { get; set; }

        [Column("IDLUGARBASEESPOL")]
        public virtual Nullable<int> idLugarBaseEspol { get; set; }

        [Column("IDLUGARPADRE")]
        public virtual int idLugarPadre { get; set; }

        [Column("TIPO")]
        public virtual string tipo { get; set; }

        [Column("ESTADO")]
        public virtual string estado { get; set; }

        [Column("SUCESOR")]
        public virtual Nullable<int> sucesor { get; set; }

    }
}
