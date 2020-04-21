using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoXalli_Gentella.Models
{
    [Table("Salidas", Schema = "Inv")]
    public partial class Salida
    {
        //CONSTRUCTOR
        public Salida()
        {
            this.DetallesDeSalida = new HashSet<DetalleDeSalida>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "La longitud debe ser de 3 dígitos")]
        [Display(Name = "Código")]
        public string CodigoSalida { get; set; }

        [Required(ErrorMessage = "La {0} es obligatorio")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de salida")]
        public DateTime FechaSalida { get; set; }

        [Display(Name = "Activo")]
        public bool EstadoTipoSalida { get; set; }

        //DECLARACION DE FK
        public int TipoSalidaId { get; set; }
        public int BodegaId { get; set; }

        //DECLARACION DE RELACIONES HIJAS
        public virtual ICollection<DetalleDeSalida> DetallesDeSalida { get; set; }

        //DECLARACION DE RELACIONES PADRES
        public virtual TipoDeSalida TipoDeSalida { get; set; }
        public virtual Bodega Bodega { get; set; }
    }
}