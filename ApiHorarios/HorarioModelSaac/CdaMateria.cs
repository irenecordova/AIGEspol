using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorarioModelSaac
{
    [Table("TBL_MATERIA", Schema = "ESPOL")]
    public class CdaMateria
    {
        [Key]
        [Column("IDMATERIA")]
        public virtual int intIdMateria { get; set; }

        [Column("IDRESPONSABLE")]
        public virtual Nullable<int> intIdResponsable { get; set; }

        [Column("IDUNIDAD")]
        public virtual Nullable<Int16> intIdUnidad { get; set; }

        [Column("NOMBRE")]
        public virtual string strNombre { get; set; }

        [Column("NUMHORAS")]
        public virtual Nullable<int> intNumHoras { get; set; }

        [Column("NUMCREDITOS")]
        public virtual Nullable<decimal> decNumCreditos { get; set; }

        [Column("TIPO")]
        public virtual string strTipo { get; set; }

        [Column("CODIGOMATERIA")]
        public virtual string strCodigoMateria { get; set; }

        [Column("ULTIMO_CAMBIO")]
        public virtual Nullable<DateTime> dtUltimo_Cambio { get; set; }

        [Column("NOMBRECOMPLETO")]
        public virtual string strNombreCompleto { get; set; }

        [Column("IDMETODOEVAL")]
        public virtual Nullable<int> intIdMetodoEval { get; set; }

        [Column("TIPOMATERIA")]
        public virtual string strTipoMateria { get; set; }

        [Column("DESCRIPCION")]
        public virtual string strDescripcion { get; set; }

    }
}
