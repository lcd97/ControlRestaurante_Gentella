using ProyectoXalli_Gentella.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoXalli_Gentella.Controllers.Busquedas {
    public class BusquedasController : Controller {

        private DBControl db = new DBControl();

        // GET: Busquedas
        public ActionResult BuscarCliente() {
            return View();
        }

        public ActionResult busquedaClientes(string Nombre = "", string Apellido = "") {

            var cliente = (from obj in db.Datos
                           join c in db.Clientes on obj.Id equals c.DatoId
                           where obj.PNombre.Trim().Contains(Nombre) || obj.PApellido.Trim().Contains(Apellido) 
                           select new {
                               DatoId = obj.Id,
                               ClienteId = obj.Id,
                               Nombres = obj.PNombre + " " + obj.PApellido,
                               RUC = obj.RUC != null ? obj.RUC : "-",
                               Documento = obj.Cedula != null ? obj.Cedula : c.PasaporteCliente
                           }).ToList();

            return Json(cliente, JsonRequestBehavior.AllowGet);
        }
    }
}