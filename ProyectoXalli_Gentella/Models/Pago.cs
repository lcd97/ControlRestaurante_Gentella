using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoXalli_Gentella.Models
{
    [Table("Pagos", Schema = "Fact")]
    public partial class Pago
    {
        [Key]
        public int Id { get; set; }

        public DateTime FechaPago { get; set; }
        public double MontoRecibido { get; set; }
    }
}