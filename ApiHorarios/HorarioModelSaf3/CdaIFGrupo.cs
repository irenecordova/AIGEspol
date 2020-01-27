using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorarioModelSaf3
{
    [Table("TBL_IF_GRUPO", Schema = "ESPOL")]
    public class CdaIFGrupo
    {
        [Key]
        [Column("IDIFGRUPO")]
        public virtual int intIdIfGrupo { get; set; }

        [Column("NOMBRE")]
        public virtual string strNombre { get; set; }

        [Column("CODIGO")]
        public virtual string strCodigo { get; set; }

        [Column("CODIGOUBICACION")]
        public virtual string strCodigoUbicacion { get; set; }

        [Column("LATITUD")]
        public virtual string strLatitud { get; set; }

        [Column("LONGITUD")]
        public virtual string strLongitud { get; set; }

        [Column("DESCRIPCION")]
        public virtual string strDescripcion { get; set; }

        [Column("ESTADO")]
        public virtual string strEstado { get; set; }

        [Column("IDLUGARESPOL")]
        public virtual Nullable<int> intIdLugarEspol { get; set; }

        [Column("IDIFGRUPOTIPO")]
        public virtual Nullable<int> intIdGrupoTipo { get; set; }

        [Column("ULTIMO_CAMBIO")]
        public virtual Nullable<DateTime> dtUltimoCambio { get; set; }
    }
}