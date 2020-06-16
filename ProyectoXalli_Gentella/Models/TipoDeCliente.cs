using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoXalli_Gentella.Models 
{
    [Table("TiposDeCliente", Schema = "Ord")]
    public partial class TipoDeCliente 
    {
        public TipoDeCliente() 
        {
            this.Clientes = new HashSet<Cliente>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "La longitud debe ser de 3 dígitos")]
        [Display(Name = "Código")]
        public string CodigoTipoCliente { get; set; }

        [Required(ErrorMessage = "La {0} es obligatorio")]
        [StringLength(50, ErrorMessage = "La longitud excede los 50 dígitos")]
        [Display(Name = "Tipo de Cliente")]
        public string DescripcionTipoCliente { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        [Display(Name = "Activo")]
        public bool EstadoTipoCliente { get; set; }

        //DEFINCION DE RELACIONES HIIJAS
        public virtual ICollection<Cliente> Clientes { get; set; }
    }
}