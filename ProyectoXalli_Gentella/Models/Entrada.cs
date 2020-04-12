using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoXalli_Gentella.Models
{
    [Table("Entradas", Schema = "Inv")]
    public partial class Entrada
    {
        //CONSTRUCTOR
        public Entrada()
        {
            this.DetallesDeEntrada = new HashSet<DetalleDeEntrada>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "La longitud debe ser de 3 dígitos")]
        [Display(Name = "Código de entrada")]
        public string CodigoEntrada { get; set; }

        [Required(ErrorMessage = "La {0} es obligatorio")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de entrada")]
        public DateTime FechaEntrada { get; set; }

        [Range(typeof(bool), "true", "true")]
        [Display(Name = "Activo")]
        public bool EstadoTipoEntrada { get; set; }

        //DEFINCION DE FK
        public int TipoEntradaId { get; set; }
        public int BodegaId { get; set; }
        public int ProveedorId { get; set; }

        //DEFINCION DE RELACIONES HIIJAS
        public virtual ICollection<DetalleDeEntrada> DetallesDeEntrada { get; set; }

        //DEFINICION DE RELACIONES PADRES
        public virtual TipoDeEntrada TipoDeEntrada { get; set; }
        public virtual Bodega Bodega { get; set; }
        public virtual Proveedor Proveedor { get; set; }
    }
}