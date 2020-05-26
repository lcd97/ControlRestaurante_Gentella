using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoXalli_Gentella.Models
{
    [Table("Datos", Schema = "Inv")]
    public partial class Dato
    {
        //CONSTRUCTOR
        public Dato()
        {
            this.Proveedores = new HashSet<Proveedor>();
        }

        [Key]
        public int Id { get; set; }

        [StringLength(16, MinimumLength = 16, ErrorMessage = "La longitud debe ser de 16 dígitos")]
        [Display(Name = "Documento de Identidad")]
        public string DNI { get; set; }

        [StringLength(50, ErrorMessage = "La longitud debe ser de 16 dígitos")]
        [Display(Name = "Nombres")]
        public string PNombre { get; set; }

        [StringLength(50, ErrorMessage = "La longitud debe ser de 16 dígitos")]
        [Display(Name = "Apellido 1")]
        public string PApellido { get; set; }

        [StringLength(14, ErrorMessage = "La longitud excede los 14 dígitos")]
        [Display(Name = "RUC")]
        public string RUC { get; set; }

        //[StringLength(50, ErrorMessage = "La longitud debe ser de 16 dígitos")]
        //[Display(Name = "Apellido 2")]
        //public string SApellido { get; set; }

        //DEFINCION DE RELACIONES HIIJAS
        public virtual ICollection<Proveedor> Proveedores { get; set; }
    }
}