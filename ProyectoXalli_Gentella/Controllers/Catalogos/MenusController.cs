using ProyectoXalli_Gentella.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
            ViewBag.CategoriaId = new SelectList(db.CategoriasMenu, "Id", "DescripcionCategoriaMenu");

            return View();
        }

        // POST: Menus/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase urlImage, string codigoMenu, string descripcionMenu, double precio, int categoriaId, string tiempo, string ingredientes)
        {
            //PRIMERO SE ALMACENA LA IMAGEN Y SU DIRECCION EN LA BD

            string path = Server.MapPath("~/images/Menu");

            //CREA EL DIRECTORIO DONDE SE ALMACENARN LAS FOTOS, EN CASO NO EXISTA
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //SI LLEGA AL MENOS UN ARCHIVO
            if (urlImage != null || Request.Files[0].ContentLength > 0)
            {
                var file = Request.Files[0];//OBTENEMOS EL ARCHIVO DE LA VISTA
                var fileName = Path.GetFileName(file.FileName);//OBTENEMOS EL NOMBRE DEL ARCHIVO
                path = Path.Combine(Server.MapPath("~/images/Menu"), fileName);//SE CREA LA DIRECCION DONDE SE ALMACENARA LA IMAGEN
                file.SaveAs(path);//SE MANDA A GUARDAR EL ARCHIVO

                string url = Path.Combine("/images/Menu", fileName);//SE CREA LA DIRECCION PARA ALMACENAR LA IMAGEN
                Imagen obj = new Imagen();//SE CREA EL OBJETO IMAGEN

                //OJO AQUI, SOLO SE ESTA TRABAJANDO LOCAL. CUANDO YA SE SUBA A ALGUN SERVIDOR SE DEBE MODIFICAR//
                obj.Ruta = url;//SE ASIGNA LA RUTA

                db.Imagenes.Add(obj);//SE AGREGA EL OBJETO

                //SI SE GUARDO CORRECTAMENTE SE ALMACENA LOS DEMAS CAMPOS
                if (db.SaveChanges() > 0)
                {
                    //SE CREA UNA INSTANCIA PARA ALMACENAR EL PLATILLO
                    Menu item = new Menu();

                    item.CodigoMenu = codigoMenu;
                    item.DescripcionMenu = descripcionMenu;
                    item.PrecioMenu = precio;
                    item.CategoriaMenuId = categoriaId;
                    item.ImagenId = obj.Id;//SE ASIGNA EL ID RECIEN ALMACENADO
                    item.EstadoMenu = true;

                    db.Menus.Add(item);
                    //SI SE ALMACENO CORRECTAMENTE EL PLATILLO
                    if (db.SaveChanges() > 0)
                    {
                        //GUARDAR RECETA
                        Receta receta = new Receta();

                        receta.TiempoEstimado = tiempo;
                        receta.Ingredientes = ingredientes;
                        receta.MenuId = item.Id;

                        db.Recetas.Add(receta);
                        if (db.SaveChanges() > 0)
                        {
                            completado = true;
                            mensaje = "Se guardó todo";
                        }
                        else
                        {
                            completado = false;
                            mensaje = "No se almacenó la receta ";
                        }
                    }
                    else
                    {
                        completado = false;
                        mensaje = "No se guardó el platillo";
                    }
                }
                else {
                    completado = false;
                    mensaje = "No se guardó bien la imagen";
                }
            }
            else {
                //DECIRLE QUE NO LLEGO NINGUNA IMAGEN
                completado = false;
                mensaje = "¿Qué pasó con la imagen?";
            }

            return Json(new { data = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
        }//FIN POST CREATE


        // POST: Proveedor/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id) {

            var Menu = db.Menus.Find(id);
            var Receta = db.Recetas.FirstOrDefault(r => r.MenuId == Menu.Id);

            db.Recetas.Remove(Receta);
            if (db.SaveChanges() > 0) {
                db.Menus.Remove(Menu);
                completado = await db.SaveChangesAsync() > 0 ? true : false;
                mensaje = completado ? "Eliminado correctamente" : "Error. Vuelva a intentarlo";
            } else {
                completado = false;
                mensaje = "Eliminación incorrecto";
            }

            return Json(new { success = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}