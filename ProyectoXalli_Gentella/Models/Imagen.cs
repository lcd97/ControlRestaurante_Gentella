using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoXalli_Gentella.Models
{
    [Table("Imagenes", Schema = "Menu")]
    public class Imagen
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        [StringLength(100,ErrorMessage = "La longitud no debe exceder de 100 dígitos")]
        [Display(Name = "Ruta")]
        public string Ruta { get; set; }

        //DEFINCION DE RELACIONES HIIJAS
        public virtual ICollection<Menu> Menu { get; set; }
    }
}