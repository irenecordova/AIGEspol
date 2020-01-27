using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorarioModelSaf3
{
    [Table("TBL_IF_GRUPO_TIPO", Schema = "ESPOL")]
    public class CdaIFGrupoTipo
    {
        [Key]
        [Column("IDIFGRUPOTIPO")]
        public virtual int intIdIfGrupoTipo { get; set; }

        [Column("NOMBREGRUPOTIPO")]
        public virtual string strNombreGrupoTipo { get; set; }

        [Column("CODIGOGRUPOTIPO")]
        public virtual string strCodigoGrupoTipo { get; set; }

        [Column("ESTADO")]
        public virtual string strEstado { get; set; }

        [Column("DESCRIPCION")]
        public virtual string strDescripcion { get; set; }

        [Column("ULTIMO_CAMBIO")]
        public virtual Nullable<DateTime> dtUltimoCambio { get; set; }

        [Column("GRUPO")]
        public virtual string strGrupo { get; set; }
    }
}
