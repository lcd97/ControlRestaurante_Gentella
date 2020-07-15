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

        public ActionResult busquedaCliente(string Identificacion) {

            var cliente = (from obj in db.Datos
                          join c in db.Clientes on obj.Id equals c.DatoId
                          where obj.DNI = Identificacio
                          select new {

                          }).f


            return Json(new { a = 0 }, JsonRequestBehavior.AllowGet);
        }
    }
}