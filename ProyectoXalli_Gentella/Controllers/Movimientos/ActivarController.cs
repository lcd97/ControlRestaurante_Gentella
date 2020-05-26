using ProyectoXalli_Gentella.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoXalli_Gentella.Controllers.Movimientos
{
    public class ActivarController : Controller
    {
        private DBControl db = new DBControl(); 

        // GET: Activar
        public ActionResult Index()
        {
            return View();
        }
        /**************************************************
         *          MODULO CONTROL DE INSUMOS           */

        /// <summary>
        /// LISTA TODAS LAS BODEGAS DESACTIVADAS
        /// </summary>
        /// <returns></returns>
        public JsonResult getBodegas()
        {
            var bodegas = (from obj in db.Bodegas.ToList()
                             where obj.EstadoBodega == false
                             select new {
                                 Id = obj.Id,
                                 Descripcion = obj.DescripcionBodega,
                                 Codigo = obj.CodigoBodega });

            return Json(new { data = bodegas }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// LISTA TODAS LAS CATEGORIAS DESACTIVADAS
        /// </summary>
        /// <returns></returns>
        public JsonResult getCategorias()
        {
            var categorias = (from obj in db.CategoriasProducto.ToList()
                           where obj.EstadoCategoria == false
                           select new
                           {
                               Id = obj.Id,
                               Descripcion = obj.DescripcionCategoria,
                               Codigo = obj.CodigoCategoria
                           });

            return Json(new { data = categorias }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// LISTA TODAS LAS TIPOS DE ENTRADAS DESACTIVADAS
        /// </summary>
        /// <returns></returns>
        public JsonResult getTiposDeEntrada()
        {
            var tiposEntrada = (from obj in db.TiposDeEntrada.ToList()
                              where obj.EstadoTipoEntrada == false
                              select new
                              {
                                  Id = obj.Id,
                                  Descripcion = obj.DescripcionTipoEntrada,
                                  Codigo = obj.CodigoTipoEntrada
                              });

            return Json(new { data = tiposEntrada }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// LISTA TODAS LAS TIPOS DE SALIDA DESACTIVADAS
        /// </summary>
        /// <returns></returns>
        public JsonResult getTiposDeSalida()
        {
            var tiposSalida = (from obj in db.TiposDeSalida.ToList()
                                where obj.EstadoTipoSalida == false
                                select new
                                {
                                    Id = obj.Id,
                                    Descripcion = obj.DescripcionTipoSalida,
                                    Codigo = obj.CodigoTipoSalida
                                });

            return Json(new { data = tiposSalida }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// LISTA TODAS LAS TIPOS DE ENTRADAS DESACTIVADAS
        /// </summary>
        /// <returns></returns>
        public JsonResult getUnidadesDeMedida()
        {
            var um = (from obj in db.UnidadesDeMedida.ToList()
                                where obj.EstadoUnidadMedida == false
                                select new
                                {
                                    Id = obj.Id,
                                    Descripcion = obj.DescripcionUnidadMedida,
                                    Codigo = obj.CodigoUnidadMedida
                                });

            return Json(new { data = um }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// LISTA TODAS LOS PRODUCTOS DESACTIVADAS
        /// </summary>
        /// <returns></returns>
        public JsonResult getProductos()
        {
            var um = (from obj in db.Productos.ToList()
                      where obj.EstadoProducto == false
                      select new
                      {
                          Id = obj.Id,
                          Descripcion = obj.DescripcionProducto,
                          Codigo = obj.CodigoProducto
                      });

            return Json(new { data = um }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// LISTA TODAS LOS PROVEEDORES DESACTIVADAS
        /// </summary>
        /// <returns></returns>
        public JsonResult getProveedores()
        {
            var um = (from obj in db.Proveedores.ToList()
                      join u in db.Datos.ToList() on obj.DatoId equals u.Id
                      where obj.EstadoProveedor == false
                      select new
                      {
                          Id = obj.Id,
                          Descripcion = obj.NombreComercial != null ? obj.NombreComercial : u.PNombre + " " + u.PApellido,
                          Codigo = u.RUC != null ? u.RUC : u.DNI
                      });
            
            return Json(new { data = um }, JsonRequestBehavior.AllowGet);
        }

        /**************************************************
         *                  MODULO MENU                 */
        /// <summary>
        /// LISTA TODAS LAS CATEGORIAS DESACTIVADAS
        /// </summary>
        /// <returns></returns>
        public JsonResult getCategoriasMenu()
        {
            var categorias = (from obj in db.CategoriasMenu.ToList()
                              where obj.EstadoCategoriaMenu == false
                              select new
                              {
                                  Id = obj.Id,
                                  Descripcion = obj.DescripcionCategoriaMenu,
                                  Codigo = obj.CodigoCategoriaMenu
                              });

            return Json(new { data = categorias }, JsonRequestBehavior.AllowGet);
        }

    }
}