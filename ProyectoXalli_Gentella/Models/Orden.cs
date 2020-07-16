﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoXalli_Gentella.Models 
{
    [Table("Ordenes", Schema = "Ord")]
    public partial class Orden {

        public Orden()
        {
            this.DetallesDeOrden = new HashSet<DetalleDeOrden>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "El campo {0} debe ser de 3 dígitos")]
        [Display(Name = "Código")]
        public string CodigoOrden { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de orden")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime FechaOrden { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Activo")]
        public bool EstadoOrden { get; set; }

        //FOREIGN KEY
        public int MeseroId { get; set; }
        public int ClienteId { get; set; }
        public int ImagenId { get; set; }
        public int TurnoId { get; set; }

        //DECLARACION DE RELACION PADRE
        public virtual Mesero Mesero { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Imagen Imagen { get; set; }
        public virtual Turno Turno { get; set; }

        //DECLARACION DE RELACION HIJA
        public virtual ICollection<DetalleDeOrden> DetallesDeOrden { get; set; }
    }
}