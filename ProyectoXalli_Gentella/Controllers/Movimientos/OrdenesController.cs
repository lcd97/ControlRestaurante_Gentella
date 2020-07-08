using ProyectoXalli_Gentella.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoXalli_Gentella.Controllers.Movimientos
{
    public class OrdenesController : Controller
    {
        private DBControl db = new DBControl();
        bool completado = false;
        string mensaje = "";

        // GET: Ordenes
        public ActionResult Index()
        {
            ViewBag.CategoriaId = new SelectList(db.CategoriasMenu, "Id", "DescripcionCategoriaMenu");

            return View();
        }

        /// <summary>
        /// RETORNA EL CODIGO DE ENTRADA AUTOMATICAMENTE
        /// </summary>
        /// <returns></returns>
        public ActionResult OrdenesCode() {
            var code = db.Ordenes.Max(x => x.CodigoOrden.Trim());
            int valor;
            string num;

            if (code != null) {

                valor = int.Parse(code);

                if (valor <= 8)
                    num = "00" + (valor + 1);
                else
                if (valor >= 9 && valor < 100)
                    num = "0" + (valor + 1);
                else
                    num = (valor + 1).ToString();
            } else
                num = "001";

            return Json(new { data = num }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MenuByCategoria(int id) {
            var menu = from obj in db.Menus.ToList()
                       join i in db.Imagenes.ToList() on obj.ImagenId equals i.Id
                       where obj.CategoriaMenuId == id
                       select new {
                           Id = obj.Id,
                           Platillo = obj.DescripcionMenu,
                           Precio = obj.PrecioMenu,
                           Imagen = i.Ruta
                       };

            return Json(new { data = menu }, JsonRequestBehavior.AllowGet);
        }
    }
}