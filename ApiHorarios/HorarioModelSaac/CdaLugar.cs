using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorarioModelSaac
{
    [Table("TBL_LUGAR_ESPOL", Schema = "ESPOL")]
    public class CdaLugar
    {
        [Key]
        [Column("IDLUGARESPOL")]
        public virtual Int16 intIdLugarEspol { get; set; }

        [Column("IDLUGARPADRE")]
        public virtual Nullable<Int16> intIdLugarPadre { get; set; }

        [Column("TIPO")]
        public virtual string strTipo { get; set; }

        [Column("DESCRIPCION")]
        public virtual string strDescripcion { get; set; }

        [Column("CAPACIDAD")]
        public virtual Nullable<int> intCapacidad { get; set; }

        [Column("ULTIMO_CAMBIO")]
        public virtual Nullable<DateTime> dtUltimo_Cambio { get; set; }

        [Column("CAPACIDADOYENTE")]
        public virtual Nullable<int> intCapacidadOyente { get; set; }

        [Column("ESTADO")]
        public virtual string strEstado { get; set; }

        [Column("VERSION")]
        public virtual Nullable<Int64> intVersion { get; set; }

        [Column("CODLOCALIDAD")]
        public virtual string strCodLocalidad { get; set; }

        [Column("LATITUD")]
        public virtual string strLatitud { get; set; }

        [Column("LONGITUD")]
        public virtual string strLongitud { get; set; }

        [Column("OBSERVACION")]
        public virtual string strObservacion { get; set; }
    }
}
