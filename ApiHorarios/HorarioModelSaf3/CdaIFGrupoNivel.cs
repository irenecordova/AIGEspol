using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorarioModelSaf3
{
    [Table("TBL_IF_GRUPO_NIVEL", Schema = "ESPOL")]
    public class CdaIFGrupoNivel
    {
        [Key]
        [Column("IDIFGRUPONIVEL")]
        public virtual int intIdIfGrupoNivel { get; set; }

        [Column("NOMBRE")]
        public virtual string strNombre { get; set; }

        [Column("ESTADO")]
        public virtual string strEstado { get; set; }

        [Column("IDIFGRUPO")]
        public virtual Nullable<int> intIdIfGrupo { get; set; }

        [Column("ULTIMO_CAMBIO")]
        public virtual Nullable<DateTime> dtUltimoCambio { get; set; }

        [Column("IDIFNIVEL")]
        public virtual Nullable<int> intIdIfNivel { get; set; }
    }
}
