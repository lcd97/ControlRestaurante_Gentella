using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoXalli_Gentella.Models
{
    [Table("DetallesDeEntrada", Schema = "Inv")]
    public partial class DetalleDeEntrada
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La {0} es obligatorio")]
        [Range(5, int.MaxValue)]
        [Display(Name = "Cantidad de entrada")]
        public int CantidadEntrada { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        [Range(1, (double)decimal.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "(0:c2)")]
        public double PrecioEntrada { get; set; }

        //DEFINCION DE FK
        public int EntradaId { get; set; }
        public int ProductoId { get; set; }

        //DECLARACION DE RELACIONES PADRES
        public virtual Entrada Entrada { get; set; }
        public virtual Producto Producto { get; set; }
    }
}