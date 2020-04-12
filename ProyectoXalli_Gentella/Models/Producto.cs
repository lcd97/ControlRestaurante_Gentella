using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoXalli_Gentella.Models
{
    [Table("Productos", Schema = "Inv")]
    public partial class Producto
    {
        //CONSTRUCTOR
        public Producto()
        {
            this.DetallesDeEntrada = new HashSet<DetalleDeEntrada>();
            this.DetallesDeSalida = new HashSet<DetalleDeSalida>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "La longitud debe ser de 3 dígitos")]
        [Display(Name = "Código de producto")]
        public string CodigoProducto { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        [StringLength(50, ErrorMessage = "La longitud excede los 50 dígitos")]
        [Display(Name = "Nombre del producto")]
        public string DescripcionProducto { get; set; }

        [Required(ErrorMessage = "La {0} es obligatorio")]
        [StringLength(50, ErrorMessage = "La longitud excede los 50 dígitos")]
        [Display(Name = "Marca del producto")]
        public string MarcaProducto { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        [Range(1, (double)decimal.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "(0:c2)")]
        [Display(Name = "Precio de producto")]
        public double PrecioProducto { get; set; }

        [Required(ErrorMessage = "La {0} es obligatorio")]
        [Range(5, int.MaxValue)]
        [Display(Name = "Cantidad de producto")]
        public int CantidadProducto { get; set; }

        [Required(ErrorMessage = "La {0} es obligatorio")]
        [Range(5, int.MaxValue, ErrorMessage = "La cantidad mínima de producto debe ser mayor a 0")]
        [Display(Name = "Cantidad máxima de producto")]
        public int CantidadMaxProducto { get; set; }

        [Required(ErrorMessage = "La {0} es obligatorio")]
        [Range(5, int.MaxValue, ErrorMessage = "La cantidad mínima de producto debe ser mayor a 0")]
        [Display(Name = "Cantidad mínima de producto")]
        public int CantidadMinProducto { get; set; }

        [Range(typeof(bool), "true", "true")]
        [Display(Name = "Activo")]
        public bool EstadoProducto { get; set; }

        //DEFINICION DE FK
        public int UnidadMedidaId { get; set; }
        public int CategoriaId { get; set; }

        //DEFINICION DE RELACIONES PADRES
        public virtual UnidadDeMedida UnidadDeMedida { get; set; }
        public virtual Categoria Categoria { get; set; }

        //DEFINCION DE RELACIONES HIJOS
        public virtual ICollection<DetalleDeSalida> DetallesDeSalida { get; set; }
        public virtual ICollection<DetalleDeEntrada> DetallesDeEntrada { get; set; }
    }
}