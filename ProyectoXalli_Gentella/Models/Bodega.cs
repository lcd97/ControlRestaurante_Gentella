﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoXalli_Gentella.Models
{
    [Table("Bodegas", Schema = "Inv")]
    public partial class Bodega
    {
        //CONSTRUCTOR
        public Bodega()
        {
            this.Entradas = new HashSet<Entrada>();
            this.Salidas = new HashSet<Salida>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "La longitud debe ser de 3 dígitos")]
        [Display(Name = "Código")]
        public string CodigoBodega { get; set; }

        [Required(ErrorMessage = "La {0} es obligatorio")]
        [StringLength(50, ErrorMessage = "La longitud excede los 50 dígitos")]
        [Display(Name = "Área")]
        public string DescripcionBodega { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        [Display(Name = "Activo")]
        public bool EstadoBodega { get; set; }

        //DEFINICION DE RELACIONES HIJOS
        public virtual ICollection<Entrada> Entradas { get; set; }
        public virtual ICollection<Salida> Salidas { get; set; }
    }
}