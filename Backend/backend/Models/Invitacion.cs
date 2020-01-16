using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("INVITACION")]
    public class Invitacion
    {
        [Key]
        [Column("IDINVITACION")]
        public int idInvitacion { get; set; }
        
        [Column("IDREUNION")]
        public int idReunion { get; set; }

        [Column("IDPERSONA")]
        public int idPersona { get; set; }

        //E,A,R
        [Column("ESTADO")]
        public string estado { get; set; }

        //T,F
        [Column("CANCELADA")]
        public string cancelada { get; set; }
    }
}
