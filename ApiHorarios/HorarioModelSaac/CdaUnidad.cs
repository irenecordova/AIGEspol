using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorarioModelSaac
{
    [Table("TBL_UNIDAD", Schema = "ESPOL")]
    public class CdaUnidad
    {
        [Key]
        [Column("IDUNIDAD")]
        public virtual Int16 intIdUnidad { get; set; }

        [Column("IDTIPOUNIDAD")]
        public virtual Nullable<Int16> intIdTipoUnidad { get; set; }

        [Column("CODUNIDAD")]
        public virtual string strCodUnidad { get; set; }

        [Column("CODAREA")]
        public virtual string strCodArea { get; set; }

        [Column("NOMBREUNIDAD")]
        public virtual string strNombreUnidad { get; set; }

        [Column("PREDECESOR")]
        public virtual Nullable<Int16> intPredecesor { get; set; }

        [Column("JEFE")]
        public virtual Nullable<int> intJefe { get; set; }

        [Column("PROXIMONUMERO")]
        public virtual Nullable<int> intProximoNumero { get; set; }

        [Column("CODACAD")]
        public virtual string strCodAcad { get; set; }

        [Column("CODFACULTAD")]
        public virtual string strCodFacultad { get; set; }

        [Column("ESTADO")]
        public virtual string strEstado { get; set; }

        [Column("COD_UNIDAD_PADRE")]
        public virtual string strCodUnidadPadre { get; set; }

        [Column("TIPO")]
        public virtual string strTipo { get; set; }

        [Column("IDSUBDECANO")]
        public virtual Nullable<int> intIdSubdecano { get; set; }

        [Column("IDSUBRODECANO")]
        public virtual Nullable<int> intIdSubRoDecano { get; set; }

        [Column("IDSUBROSUBDECANO")]
        public virtual Nullable<int> intIdSubRoSubDecano { get; set; }

        [Column("DESCRIPCIONUNIDAD")]
        public virtual string strDescripcionUnidad { get; set; }

    }
}
