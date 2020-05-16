using ProyectoXalli_Gentella.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoXalli_Gentella.Controllers.Catalogos
{
    public class MenusController : Controller
    {
        private DBControl db = new DBControl();
        private bool completado = false;
        private string mensaje = "";

        // GET: Platillos
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// RECUPERA DATOS PARA LLENAR LA TABLA MENU A TRAVES DE JSON
        /// </summary>
        /// <returns></returns>
        public JsonResult GetData()
        {
            var menu = (from obj in db.Menus.ToList()                             
                             where obj.EstadoMenu == true
                             select new
                             {
                                 Id = obj.Id,
                                 DescripcionPlatillo = obj.DescripcionMenu,
                                 CodigoPlatillo = obj.CodigoMenu,
                                 Precio = obj.PrecioMenu
                             });

            return Json(new { data = menu }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return View();
        }
    }
}