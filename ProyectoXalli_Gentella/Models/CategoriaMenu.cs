using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoXalli_Gentella.Models
{
    [Table("CategoriasMenu", Schema = "Menu")]
    public class CategoriaMenu
    {
        //CONSTRUCTOR DE LA CLASE HIJA
        public CategoriaMenu() {
            this.Menus = new HashSet<Menu>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "La longitud debe ser de 3 dígitos")]
        [Display(Name = "Código")]
        public string CodigoCategoriaMenu { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        [StringLength(50, ErrorMessage = "La longitud no debe exceder de 50 dígitos")]
        [Display(Name = "Categoría")]
        public string DescripcionCategoriaMenu { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        [Display(Name = "Activo")]
        public bool EstadoCategoriaMenu { get; set; }

        //DEFINCION DE RELACIONES HIIJAS
        public virtual ICollection<Menu> Menus { get; set; }
    }
}