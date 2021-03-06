﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoXalli_Gentella.Models 
{
    [Table("Clientes", Schema = "Ord")]
    public partial class Cliente 
    {
        [Key]
        public int Id { get; set; }

        [StringLength(150, ErrorMessage = "El correo no debe exceder los 50 dígitos")]
        [Display(Name = "Correo Electrónico")]
        public string EmailCliente { get; set; }

        [StringLength(9, ErrorMessage = "El número telefónico no debe exceder los 9 dígitos")]
        [Display(Name = "Teléfono")]
        public string TelefonoCliente { get; set; }
        
        [Display(Name = "Activo")]
        public bool EstadoCliente { get; set; }

        [StringLength(10, ErrorMessage = "El pasaporte no debe exceder los 10 dígitos")]
        [Display(Name = "Pasaporte")]
        public string PasaporteCliente { get; set; }

        //FOREIGN KEY
        public int DatoId { get; set; }

        //DECLARACION DE RELACION PADRE
        public virtual Dato Dato { get; set; }
    }
}