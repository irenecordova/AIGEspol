﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("LISTA_PERSONALIZADA")]
    public class Lista_Personalizada
    {
        [Key]
        [Column("ID")]
        public long id { get; set; }

        [Column("IDPERSONA")]
        public string idPersona { get; set; }

        [Column("NOMBRE")]
        public string nombre { get; set; }

    }
}
