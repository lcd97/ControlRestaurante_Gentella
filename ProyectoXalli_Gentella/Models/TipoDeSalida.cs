using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoXalli_Gentella.Models
{
    [Table("TiposDeSalida", Schema = "Inv")]
    public partial class TipoDeSalida
    {
        //CONSTRUCTOR
        public TipoDeSalida()
        {
            this.Salidas = new HashSet<Salida>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "La longitud debe ser de 3 dígitos")]
        [Display(Name = "Código")]
        public string CodigoTipoSalida { get; set; }

        [Required(ErrorMessage = "La {0} es obligatorio")]
        [StringLength(50, ErrorMessage = "La longitud excede los 50 dígitos")]
        [Display(Name = "Tipo de salida")]
        public string DescripcionTipoSalida { get; set; }

        [Display(Name = "Activo")]
        public bool EstadoTipoSalida { get; set; }

        //DECLARACION DE RELACIONES HIJAS
        public virtual ICollection<Salida> Salidas { get; set; }
    }
}