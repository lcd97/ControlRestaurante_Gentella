using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoXalli_Gentella.Models
{
    [Table("DetallesDeSalida", Schema = "Inv")]
    public partial class DetalleDeSalida
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Cantidad de salida")]
        public int CantidadSalida { get; set; }

        //DECLARACION DE FK
        public int SalidaId { get; set; }
        public int ProductoId { get; set; }

        //DECLARACION DE RELACIONES PADRES
        public virtual Salida Salida { get; set; }
        public virtual Producto Producto { get; set; }
    }
}