using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
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
        [Display(Name = "Código")]
        public string CodigoProducto { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        [StringLength(50, ErrorMessage = "La longitud excede los 50 dígitos")]
        [Display(Name = "Producto")]
        public string DescripcionProducto { get; set; }

        [Required(ErrorMessage = "La {0} es obligatorio")]
        [StringLength(50, ErrorMessage = "La longitud excede los 50 dígitos")]
        [Display(Name = "Marca")]
        public string MarcaProducto { get; set; }

        //[Required(ErrorMessage = "La {0} es obligatorio")]
        ////[Range(5, int.MaxValue)]
        //[Display(Name = "Cantidad de existente")]
        //[DisplayFormat(DataFormatString = "(0:c2)")]//MUESTRA EL FORMATO "10.20"
        //public double CantidadProducto { get; set; }

        [Required(ErrorMessage = "La {0} es obligatorio")]
        //[Range(5, int.MaxValue, ErrorMessage = "La cantidad mínima de producto debe ser mayor a 0")]
        [DisplayFormat(DataFormatString = "(0:c2)")]//MUESTRA EL FORMATO "10.20"
        [Display(Name = "Cantidad máxima")]
        public double CantidadMaxProducto { get; set; }

        [Required(ErrorMessage = "La {0} es obligatorio")]
        //[Range(5, int.MaxValue, ErrorMessage = "La cantidad mínima de producto debe ser mayor a 0")]
        [DisplayFormat(DataFormatString = "(0:c2)")]//MUESTRA EL FORMATO "10.20"
        [Display(Name = "Cantidad mínima")]
        public double CantidadMinProducto { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        [Display(Name = "Activo")]
        public bool EstadoProducto { get; set; }

        //DEFINICION DE FK
        public int UnidadMedidaId { get; set; }
        public int CategoriaId { get; set; }

        //DEFINICION DE RELACIONES PADRES
        public virtual UnidadDeMedida UnidadDeMedida { get; set; }
        public virtual CategoriaProducto Categoria { get; set; }

        //DEFINCION DE RELACIONES HIJOS
        public virtual ICollection<DetalleDeSalida> DetallesDeSalida { get; set; }
        public virtual ICollection<DetalleDeEntrada> DetallesDeEntrada { get; set; }
    }
}