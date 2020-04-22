﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoXalli_Gentella.Models
{
    [Table("TiposDeEntrada", Schema = "Inv")]
    public partial class TipoDeEntrada
    {
        //CONSTRUCTOR
        public TipoDeEntrada()
        {
            this.Entradas = new HashSet<Entrada>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "La longitud debe ser de 3 dígitos")]
        [Display(Name = "Código")]
        public string CodigoTipoEntrada { get; set; }

        [Required(ErrorMessage = "La {0} es obligatorio")]
        [StringLength(50, ErrorMessage = "La longitud excede los 50 dígitos")]
        [Display(Name = "Tipo de entrada")]
        public string DescripcionTipoEntrada { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        [Display(Name = "Activo")]
        public bool EstadoTipoEntrada { get; set; }

        //DECLARACION DE RELACIONES HIJAS
        public virtual ICollection<Entrada> Entradas { get; set; }
    }
}