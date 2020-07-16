using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoXalli_Gentella.Models 
{
    [Table("DetallesDeOrden", Schema = "Ord")]
    public partial class DetalleDeOrden 
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]      
        [Display(Name = "Cantidad")]
        public int CantidadOrden { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]
        [StringLength(150, ErrorMessage = "Las notas no puede exceder los 150 caracteres")]
        [Display(Name = "Notas")]
        public string NotaDetalleOrden { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Activo")]
        public bool EstadoDetalleOrden { get; set; }

        //DEFINICION DE LAS LLAVES FORANEAS
        public int OrdenId { get; set; }
        public int MenuId { get; set; }

        //DECLARACION DE LAS TABLAS PADRES
        public virtual Orden Orden { get; set; }
        public virtual Menu Menu { get; set; }
    }
}