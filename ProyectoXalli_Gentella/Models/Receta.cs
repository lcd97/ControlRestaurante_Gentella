using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoXalli_Gentella.Models
{
    [Table("Recetas", Schema = "Menu")]
    public partial class Receta
    {
        [Key]
        public int Id { get; set; }

        //[Required(ErrorMessage = "El campo es obligatorio")]
        [StringLength(5, ErrorMessage = "La longitud debe ser de 3 dígitos")]
        [Display(Name = "Tiempo Estimado")]
        public string TiempoEstimado { get; set; }

        //[Required(ErrorMessage = "El {0} es obligatorio")]
        [StringLength(150, ErrorMessage = "La longitud no debe exceder de 150 dígitos")]
        [Display(Name = "Tiempo Estimado")]
        public string Ingredientes { get; set; }

        //DEFINCION DE FK
        public int MenuId { get; set; }

        //DEFINICION DE RELACIONES PADRES
        public virtual Menu Menu { get; set; }
    }
}