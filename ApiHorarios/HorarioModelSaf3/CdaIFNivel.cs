using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorarioModelSaf3
{
    [Table("TBL_IF_NIVEL", Schema = "ESPOL")]
    public class CdaIFNivel
    {
        [Key]
        [Column("IDIFNIVEL")]
        public virtual int intIdIfNivel { get; set; }

        [Column("CODIGO")]
        public virtual string strCodigo { get; set; }

        [Column("DESCRIPCION")]
        public virtual string strDescripcion { get; set; }

        [Column("ULTIMO_CAMBIO")]
        public virtual Nullable<DateTime> dtUltimoCambio { get; set; }
    }
}
