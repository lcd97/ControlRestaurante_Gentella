using ProyectoXalli_Gentella.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoXalli_Gentella.Controllers.Movimientos
{
    public class EntradasController : Controller
    {
        private DBControl db = new DBControl();

        /// <summary>
        /// MUESTRA INDEX DE ENTRADAS DE BAR
        /// </summary>
        /// <returns></returns>
        public ActionResult Bar()
        {
            return View();
        }

        /// <summary>
        /// MUESTRA INDEX DE ENTRADAS RESTAURANTE
        /// </summary>
        /// <returns></returns>
        public ActionResult Restaurante() 
        {
            return View();
        }

        /// <summary>
        /// OBTIENE UNA LISTA DE TODOS LOS PROVEEDORES
        /// </summary>
        /// <returns></returns>
        public ActionResult getProveedor() 
        {
            var provider = from obj in db.Proveedores.ToList()
                           join u in db.Datos.ToList() on obj.DatoId equals u.Id
                           select new {
                               //CONSULTA PARA ASIGNARLE A LA VARIABLE PROVEEDOR EL NOMBRE COMERCIAL O NOMBRE DE LA PERSONA NATURAL
                               Proveedor = obj.NombreComercial != null ? obj.NombreComercial : u.PNombre + " " + u.PApellido,
                               Id = obj.Id
                           };

            return Json(new { data = provider }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// OBTIENE UNA LISTA DE TODOS LOS TIPOS DE ENTRADA
        /// </summary>
        /// <returns></returns>
        public ActionResult getTipoEntrada() 
        {

            var entrada = from obj in db.TiposDeEntrada.ToList()
                           select new {
                               //CONSULTA PARA ASIGNARLE A LA VARIABLE PROVEEDOR EL NOMBRE COMERCIAL O NOMBRE DE LA PERSONA NATURAL
                               Entrada = obj.DescripcionTipoEntrada,
                               Id = obj.Id
                           };

            return Json(new { data = entrada }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getProductos() 
        {
            var producto = from obj in db.Productos.ToList()
                           join um in db.UnidadesDeMedida.ToList() on obj.UnidadMedidaId equals um.Id
                          select new {
                              //CONSULTA PARA ASIGNARLE A LA VARIABLE PROVEEDOR EL NOMBRE COMERCIAL O NOMBRE DE LA PERSONA NATURAL
                              Presentacion = obj.DescripcionProducto + " - " + obj.MarcaProducto + " - " + um.AbreviaturaUM,
                              Id = obj.Id
                          };

            return Json(new { data = producto }, JsonRequestBehavior.AllowGet);
        }
    }
}