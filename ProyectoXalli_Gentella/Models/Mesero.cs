using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoXalli_Gentella.Models 
{
    [Table("Meseros", Schema = "Ord")]
    public partial class Mesero 
    {
        public Mesero()
        {
            this.Ordenes = new HashSet<Orden>();
        }

        [Key]
        public int Id { get; set; }

        [StringLength(10, ErrorMessage = "El código INSS no debe exceder los 10 caracteres")]
        [Display(Name = "INSS")]
        public string INSS { get; set; }

        [Required]
        [StringLength(5, ErrorMessage = "La Hora de entrada no debe exceder los 5 caracteres")]
        [Display(Name = "Hora Entrada")]
        public string HoraEntrada { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(5, ErrorMessage = "La Hora de entrada no debe exceder los 5 caracteres")]
        [Display(Name = "Hora Salida")]
        public string HoraSalida { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(5, ErrorMessage = "El inicio de Turno no debe exceder los 5 caracteres")]
        [Display(Name = "Inicio Turno")]
        public string InicioTurno { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(5, ErrorMessage = "El fin del turno no debe exceder los 5 caracteres")]
        [Display(Name = "Fin Turno")]
        public string FinTurno { get; set; }

        //FOREIGN KEY
        public int DatoId { get; set; }

        //DECLARACION DE RELACION PADRE
        public virtual Dato Dato { get; set; }

        //DECLARACION DE RELACION HIJA
        public ICollection<Orden> Ordenes { get; set; }
    }
}