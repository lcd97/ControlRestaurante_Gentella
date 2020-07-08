using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProyectoXalli_Gentella.Models {

    [Table("Turnos", Schema = "Inv")]
    public partial class Turno {
        public Turno() {
            this.Salidas = new HashSet<Salida>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]
        [StringLength(10, ErrorMessage = "El inicio de Turno no debe exceder los 5 caracteres")]
        [Display(Name = "Inicio Turno")]
        public string InicioTurno { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]
        [StringLength(10, ErrorMessage = "El fin del turno no debe exceder los 5 caracteres")]
        [Display(Name = "Fin Turno")]
        public string FinTurno { get; set; }

        //FOREIGN KEY
        public int MeseroId { get; set; }

        //DECLARACION DE RELACION PADRE
        public virtual Mesero Mesero { get; set; }

        //DECLARACION DE RELACION HIJA
        public ICollection<Salida> Salidas { get; set; }
    }
}