using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorarioModelSaf3
{
    [Table("TBL_IF_SUBGRUPO", Schema = "ESPOL")]
    public class CdaIFSubgrupo
    {
        [Key]
        [Column("IDIFSUBGRUPO")]
        public virtual int intIdIfSubgrupo { get; set; }

        [Column("CODIGO")]
        public virtual string strCodigo { get; set; }

        [Column("ETIQUETA")]
        public virtual string strEtiqueta { get; set; }

        [Column("DIMENSION")]
        public virtual string strDimension { get; set; }

        [Column("CAPACIDAD")]
        public virtual Nullable<int> intCapacidad { get; set; }

        [Column("IDIFGRUPONIVEL")]
        public virtual Nullable<int> intIdIfGrupoNivel { get; set; }

        [Column("IDIFGRUPOTIPO")]
        public virtual Nullable<int> intIdIfGrupoTipo { get; set; }

        [Column("NOMBRE")]
        public virtual string strNombre { get; set; }

        [Column("ULTIMO_CAMBIO")]
        public virtual Nullable<DateTime> dtUltimoCambio { get; set; }

        [Column("ESTADO")]
        public virtual string strEstado { get; set; }
    }
}
