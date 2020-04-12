using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoXalli_Gentella.Models
{
    [Table("UnidadesDeMedida", Schema = "Inv")]
    public partial class UnidadDeMedida
    {
        //CONSTRUCTOR
        public UnidadDeMedida()
        {
            this.Productos = new HashSet<Producto>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "La longitud debe ser de 3 dígitos")]
        [Display(Name = "Código de unidad de medida")]
        public string CodigoUnidadMedida { get; set; }

        [Required(ErrorMessage = "La {0} es obligatorio")]
        [StringLength(50, ErrorMessage = "La longitud excede los 50 dígitos")]
        [Display(Name = "Descripción de unidad de medida")]
        public string DescripcionUnidadMedida { get; set; }

        [Range(typeof(bool), "true", "true")]
        [Display(Name = "Activo")]
        public bool EstadoUnidadMedida { get; set; }

        //DEFINICION DE RELACIONES HIJOS
        public virtual ICollection<Producto> Productos { get; set; }
    }
}