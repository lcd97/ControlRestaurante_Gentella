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
            this.Ordenes = new HashSet<Orden>();
        }

        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "El campo es obligatorio")]
        [DataType(DataType.Date)]
        [Display(Name = "Inicio Turno")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime InicioTurno { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]
        [DataType(DataType.Date)]
        [Display(Name = "Fin Turno")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public string FinTurno { get; set; }

        //FOREIGN KEY
        public int MeseroId { get; set; }

        //DECLARACION DE RELACION PADRE
        public virtual Mesero Mesero { get; set; }

        //DECLARACION DE RELACION HIJA
        public ICollection<Orden> Ordenes { get; set; }
    }
}